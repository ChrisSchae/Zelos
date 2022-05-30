using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using ZelosFramework.FileHandling;
using ZelosFramework.FTP;
using ZelosFramework.NLP_Core.FileSettings;
using ZelosFramework.Scheduling;

namespace ZelosFramework.NLP_Core
{
    public partial class Script
    {
        [JsonIgnore]
        public List<AnalysisToken> TokenizedDoc { get; private set; } = new List<AnalysisToken>();

        public string Name { get; set; }

        public string ScriptString { get; set; }

        public FtpSource FtpSource { get; private set; } = new FtpSource();

        public bool IsFtpServerScript { get
            {
                if (this.FtpSource?.Url != string.Empty && this.FtpSource.Url?.Trim().Length > 0) { return true; }
                return false;
            }
        }

        public FileSetting FileSettings { get; set; }

        public SchedulingConfig SchedulingConfig { get; set; }
        public bool? IsWebDownloadScript
        {
            get
            {
                if (this.WebSource?.Url != string.Empty && this.WebSource.Url?.Trim().Length > 0) { return true; }
                return false;
            }
        }

        public WebSource WebSource { get; set; } = new WebSource();

        internal IEnumerable<AnalysisToken> GetRemainingTokDoc(AnalysisToken token)
        {
            return this.TokenizedDoc.Skip(token.PositionInText + 1);
        }
    }
}
