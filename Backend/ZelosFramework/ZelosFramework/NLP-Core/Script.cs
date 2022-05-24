using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using ZelosFramework.FileHandling;
using ZelosFramework.FTP;
using ZelosFramework.Scheduling;

namespace ZelosFramework.NLP_Core
{
    public class Script
    {
        public string Name { get; set; }

        public string ScriptString { get; set; }

        [JsonIgnore]
        public List<AnalysisToken> TokenizedDoc { get; private set; } = new List<AnalysisToken>();
        public FtpSource FtpSource { get; private set; } = new FtpSource();

        public bool IsFtpServerScript { get
            {
                if (this.FtpSource?.Url != string.Empty && this.FtpSource.Url?.Trim().Length > 0) { return true; }
                return false;
            }
        }

        public FileType SourceFileType { get; set; }
        public SchedulingConfig SchedulingConfig { get; set; }

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

            return conn.GetFile(this.FtpSource.FilePath, '.' + this.SourceFileType.ToString().ToLower());
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
