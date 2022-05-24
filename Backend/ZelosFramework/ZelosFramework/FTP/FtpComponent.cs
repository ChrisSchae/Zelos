using System;
using System.IO;
using FluentFTP;

namespace ZelosFramework.FTP
{
    public class FtpComponent
    {
        private readonly FtpClient client;

        public FtpComponent(string host, string user, string pass)
        {
            client = new FtpClient(host, user, pass);
        }

        public FtpComponent(string host)
        {
            client = new FtpClient(host);
        }

        public void Connect()
        {
            client.Connect();
        }

        public bool IsConnected()
        {
            return client.IsConnected;
        }

        internal FileInfo GetFile(string filePath, string fileExtension)
        {
            var tempFile = new FileInfo(GetTempFilePathWithExtension(fileExtension));
            if(!this.IsConnected())
            {
                this.Connect();
            }
            if (!client.FileExists(filePath))
            {
                throw new FtpException("File not found");
            }

            var ftpState = false;
            using (FileStream outStream = File.Create(tempFile.FullName))
            {
                ftpState = client.Download(outStream, filePath);
            }

            if (!ftpState)
            {
                throw new FtpException("Download was not possible.");
            }

            return tempFile;
        }

        private static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), extension);
            string tempFilePath = Path.Combine(path, fileName);
            return tempFilePath;
        }
    }
}
