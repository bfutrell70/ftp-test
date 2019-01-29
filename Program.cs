using System;
using System.IO;
using System.Net;

namespace ftp_test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            FtpListDirectoryExample();

            //var result = ListFilesOnServerSsl(new Uri("ftp://ftp.stayonline.com"));
        }

        static void FtpDownloadExample()
        {
            // code from https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-download-files-with-ftp

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.contoso.com/test.htm");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous","janeDoe@contoso.com");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            Console.WriteLine(reader.ReadToEnd());

            Console.WriteLine($"Download Complete, status {response.StatusDescription}");

            reader.Close();
            response.Close();
        }

        static void FtpListDirectoryExample()
        {
            // Get the object used to communicate with the server.
            
            // NOTE: FTP server on MyBookLive - no option for encryption
            // disabled the FTP server for now.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.0.51");
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential ("brian","qmpzla");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            Console.WriteLine(reader.ReadToEnd());

            Console.WriteLine($"Directory List Complete, status {response.StatusDescription}");

            reader.Close();
            response.Close();
        }

        public static bool ListFilesOnServerSsl(Uri serverUri)
        {
            // copied from https://docs.microsoft.com/en-us/dotnet/api/system.net.ftpwebrequest.enablessl?view=netframework-4.7.2

            // The serverUri should start with the ftp:// scheme.
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return false;
            }
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.EnableSsl = true;
            
            // Get the ServicePoint object used for this request, and limit it to one connection.
            // In a real-world application you might use the default number of connections (2),
            // or select a value that works best for your application.
            
            ServicePoint sp = request.ServicePoint;
            Console.WriteLine("ServicePoint connections = {0}.", sp.ConnectionLimit);
            sp.ConnectionLimit = 1;
            
            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            Console.WriteLine("The content length is {0}", response.ContentLength);
            // The following streams are used to read the data returned from the server.
            Stream responseStream = null;
            StreamReader readStream = null;
            try
            {
                responseStream = response.GetResponseStream(); 
                readStream = new StreamReader(responseStream, System.Text.Encoding.UTF8);

                if (readStream != null)
                {
                    // Display the data received from the server.
                    Console.WriteLine(readStream.ReadToEnd());
                } 
                Console.WriteLine("List status: {0}",response.StatusDescription);            
            }
            finally
            {
                if (readStream != null)
                {
                    readStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        

            Console.WriteLine("Banner message: {0}", 
                response.BannerMessage);

            Console.WriteLine("Welcome message: {0}", 
                response.WelcomeMessage);

            Console.WriteLine("Exit message: {0}", 
                response.ExitMessage);
            return true;
        }
    }
}
