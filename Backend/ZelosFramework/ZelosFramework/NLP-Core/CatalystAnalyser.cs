using System;
using System.Collections.Generic;
using System.Linq;
using Catalyst;
using Mosaik.Core;
using ZelosFramework;
using ZelosFramework.FileHandling;
using ZelosFramework.NLP_Core;
using ZelosFramework.Scheduling;

namespace ZelosFramework.NLP_Core
{
    public class CatalystAnalyser : INlpAnalyzer
    {
        public CatalystAnalyser()
        {
            Catalyst.Models.English.Register();
        }

        public Script Analyse(string script)
        {
            var result = new Script();
            result.ScriptString = script;
            return Analyse(result);
        }

        public Script Analyse(Script script)
        {
            Document doc = ProcessInput(script.ScriptString);
            EnrichTokens(script, doc.TokensData);
            DetermineSourceFileType(script);
            DetermineScheduling(script);
            ProcessFtpConfigIfAvailable(script);

            return script;
        }

        private void DetermineScheduling(Script script)
        {
            if (script.TokenizedDoc.Any(t => t.Word.ToLower().Contains("every")))
            {
                var schedulingConfig = new SchedulingConfig();
                script.SchedulingConfig = schedulingConfig;
                var schedulingConfigStartToken = script.TokenizedDoc.First(t => t.Word.ToLower().Contains("every"));
                schedulingConfig.IntervalType = GetIntevalType(schedulingConfigStartToken);
                schedulingConfig.Hour = GetSchedulingHour(schedulingConfigStartToken, script);

                schedulingConfig.Minute = 0;
                schedulingConfig.DayOffset = GetDayOffset(schedulingConfigStartToken, script);
            }
        }

        private static IntervalType GetIntevalType(AnalysisToken schedulingConfigStartToken)
        {
            var intervalTypeString = schedulingConfigStartToken.NextToken.Word;

            var intervalType = IntervalType.None;
            if (!Enum.TryParse<IntervalType>(intervalTypeString, ignoreCase: true, result: out intervalType))
            {
                throw new InvalidOperationException("Unknown intervall type");
            }

            return intervalType;
        }

        private static int GetSchedulingHour(AnalysisToken schedulingConfigStartToken, Script script)
        {
            var workingToken = schedulingConfigStartToken;
            while (workingToken.NextToken?.PartOfSpech != "NUM")
            {
                workingToken = workingToken.NextToken;
            }
            var result = int.Parse(workingToken.NextToken.Word);
            var remainingTokDoc = script.TokenizedDoc.Skip(workingToken.PositionInText);
            if (remainingTokDoc.Any(tok => tok.PartOfSpech.ToUpper() == "NOUN" && (tok.Word.ToUpper() == "PM" || tok.Word.ToUpper() == "P.M.")))
            {
                result += 12;
            }
            return result;
        }

        private static int GetDayOffset(AnalysisToken schedulingConfigStartToken, Script script)
        {
            var remainingTokDoc = script.TokenizedDoc.Skip(schedulingConfigStartToken.PositionInText + 1);
            if (remainingTokDoc.Any(tok => tok.Word.ToUpper() == "AT"))
            {
                if (script.SchedulingConfig.IntervalType == IntervalType.Week)
                {
                    var intervallToken = remainingTokDoc.First(tok => tok.Word.ToUpper() == "AT");
                    switch (intervallToken.NextToken.Word.ToUpper())
                    {
                        case "MONDAY": return 1;
                        case "TUESDAY": return 2;
                        case "WENDSDAY": return 3;
                        case "THURSDAY": return 4;
                        case "FRIDAY": return 5;
                        case "SATURDAY": return 6;
                        case "SUNDAY": return 7;
                        default: return -1;
                    }
                }
            }
            return -1;
        }

        private void DetermineSourceFileType(Script result)
        {
            var fileTypeNames = new List<string>(Enum.GetNames(typeof(FileType)));
            if (result.TokenizedDoc.Any(t => fileTypeNames.Any(typeName => typeName.ToLower() == t.Word.ToLower())))
            {
                var firstTypeToken = result.TokenizedDoc.First(t => fileTypeNames.Any(typeName => typeName.ToLower() == t.Word.ToLower()));
                result.SourceFileType = Enum.Parse<FileType>(firstTypeToken.Word);
            }
        }

        private void ProcessFtpConfigIfAvailable(Script result)
        {
            if (result.TokenizedDoc.Any(t => t.Word.ToLower().Contains("ftp")))
            {
                var url = RecognizeSpecialString("ftp", result);
                result.FtpSource.Url = url;

                var filePath = RecognizeSpecialString("path", result);
                result.FtpSource.FilePath = filePath;

                if (!(result.TokenizedDoc.Any(t => t.Word.ToLower() == "anonymous" && t.NextToken.Word.ToLower() == "user")))
                {
                    var user = RecognizeSpecialString("user", result);
                    result.FtpSource.User = user;

                    var pw = RecognizeSpecialString("password", result);
                    result.FtpSource.Password = pw;
                }
            }
        }

        private static string RecognizeSpecialString(string specialStringPredefiner, Script doc)
        {
            var result = string.Empty;
            var workingToken = doc.TokenizedDoc.First(t => t.Word.ToLower().Contains(specialStringPredefiner.ToLower()));
            while (workingToken.NextToken != null && workingToken.NextToken.Word != "'")
            {
                workingToken = workingToken.NextToken;
            }
            workingToken = workingToken.NextToken;
            while (workingToken.NextToken != null && workingToken.NextToken.Word != "'")
            {
                result += workingToken.NextToken.Word;
                workingToken = workingToken.NextToken;
            }

            return result;
        }

        private void EnrichTokens(Script result, List<List<TokenData>> tokensData)
        {
            int counter = 0;
            foreach (var toks in tokensData)
            {
                AnalysisToken prevTok = null;
                foreach (var tok in toks)
                {
                    var tokWord = result.ScriptString.Substring(tok.LowerBound, tok.UpperBound - tok.LowerBound + 1);
                    var currTok = new AnalysisToken(tokWord, counter, tok.Tag.ToString(), prevTok);
                    result.TokenizedDoc.Add(currTok);
                    if (prevTok != null)
                    {
                        prevTok.NextToken = currTok;
                    }
                    prevTok = currTok;
                    counter++;
                }
            }
        }

        private Document ProcessInput(string scriptString)
        {
            var nlpPipe = Pipeline.ForAsync(Language.English);
            var nlp = nlpPipe.GetAwaiter().GetResult();
            var doc = new Document(scriptString, Language.English);
            nlp.ProcessSingle(doc);
            return doc;
        }
    }
}
