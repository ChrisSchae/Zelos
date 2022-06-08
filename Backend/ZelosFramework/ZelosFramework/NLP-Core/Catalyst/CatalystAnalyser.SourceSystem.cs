using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZelosFramework.NLP_Core.Catalyst
{
    public partial class CatalystAnalyser
    {
        private void ProcessSourceInformation(Script script)
        {
            if (script.TokenizedDoc.Any(t => t.Word.ToLower().Contains("ftp")))
            {
                ProcessFtpConfig(script);
                return;
            }

            if (script.TokenizedDoc.Any(t => t.Word.ToLower().Contains("website")))
            {
                ProcessWebConfig(script);
                return;
            }
        }

        private void ProcessWebConfig(Script script)
        {
            var url = RecognizeSpecialString("website", script);
            script.WebSource.Url = url;

            var downloadLogic = string.Empty;
            var docAfterBy = script.GetRemainingTokDoc(script.TokenizedDoc.First(tok => tok.Word.ToLower() == "by"));
            downloadLogic = string.Join(" ", docAfterBy.TakeWhile(tok => tok.Word != "." && tok.PartOfSpech != "PUNCT").Select(tok=> tok.Word));
            script.WebSource.DownloadLogic = downloadLogic;

            if (script.TokenizedDoc.Any(t => t.Word.ToLower() == "user"))
            {
                var user = RecognizeSpecialString("user", script);
                script.WebSource.User = user;

                var pw = RecognizeSpecialString("password", script);
                script.WebSource.Password = pw;
            }
        }

        private void ProcessFtpConfig(Script script)
        {

            var url = RecognizeSpecialString("ftp", script);
            script.FtpSource.Url = url;

            var filePath = RecognizeSpecialString("path", script);
            script.FtpSource.FilePath = filePath;

            if (!(script.TokenizedDoc.Any(t => t.Word.ToLower() == "anonymous" && t.NextToken.Word.ToLower() == "user")))
            {
                var user = RecognizeSpecialString("user", script);
                script.FtpSource.User = user;

                var pw = RecognizeSpecialString("password", script);
                script.FtpSource.Password = pw;
            }
        }
    }
}
