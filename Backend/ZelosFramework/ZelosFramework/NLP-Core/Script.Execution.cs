using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelosFramework.FTP;

namespace ZelosFramework.NLP_Core
{
    public partial class Script
    {

        public object ExecuteScript()
        {
            if (this.IsFtpServerScript)
            {
                return this.RunFTPDownload();
            }
            return null;
        }

        private FileInfo RunFTPDownload()
        {
            var url = this.FtpSource.Url;
            var conn = SetUpFTPConnection(url);
            conn.Connect();

            return conn.GetFile(this.FtpSource.FilePath, '.' + this.SourceFileSettings.FileType.ToString().ToLower());
        }

        private FtpComponent SetUpFTPConnection(string url)
        {
            if (this.FtpSource.User != null && this.FtpSource.Password != null)
            {
                return new FtpComponent(url, this.FtpSource.User, this.FtpSource.Password);
            }

            return new FtpComponent(url);
        }
    }
}
