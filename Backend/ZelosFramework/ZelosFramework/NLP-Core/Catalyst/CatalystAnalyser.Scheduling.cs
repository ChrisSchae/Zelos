using System;
using System.Linq;
using ZelosFramework.Scheduling;

namespace ZelosFramework.NLP_Core.Catalyst
{
    public partial class CatalystAnalyser
    {
        private void DetermineScheduling(Script script)
        {
            if (script.TokenizedDoc.Any(t => t.Word.ToLower().Contains("every")))
            {
                var schedulingConfig = new SchedulingConfig();
                script.SchedulingConfig = schedulingConfig;
                var schedulingConfigStartToken = script.TokenizedDoc.First(t => t.Word.ToLower().Contains("every"));
                schedulingConfig.IntervalType = GetIntevalType(schedulingConfigStartToken);
                schedulingConfig.Hour = GetSchedulingHour(schedulingConfigStartToken, script);

                schedulingConfig.Minute = GetSchedulingMinute(schedulingConfigStartToken, script);
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

        private static int GetSchedulingMinute(AnalysisToken schedulingConfigStartToken, Script script)
        {
            var remainingTokDoc = script.GetRemainingTokDoc(schedulingConfigStartToken);
            var numbersInSchedulingDescription = remainingTokDoc.Where(tok => tok.PartOfSpech == "NUM");
            if (numbersInSchedulingDescription.Count() > 1)
            {
                return int.Parse(numbersInSchedulingDescription.ToList()[1].Word);
            }
            return 0;
        }

        private static int GetSchedulingHour(AnalysisToken schedulingConfigStartToken, Script script)
        {
            var remainingTokDoc = script.GetRemainingTokDoc(schedulingConfigStartToken);
            var workingToken = remainingTokDoc.First(tok => tok.PartOfSpech == "NUM");
            var result = int.Parse(workingToken.Word);
            remainingTokDoc = script.GetRemainingTokDoc(workingToken);
            if (remainingTokDoc.Any(tok => tok.PartOfSpech.ToUpper() == "NOUN" && (tok.Word.ToUpper() == "PM" || tok.Word.ToUpper() == "P.M.")))
            {
                result += 12;
            }
            return result;
        }

        private static int GetDayOffset(AnalysisToken schedulingConfigStartToken, Script script)
        {
            var remainingTokDoc = script.GetRemainingTokDoc(schedulingConfigStartToken);
            if (remainingTokDoc.Any(tok => tok.Word.ToUpper() == "AT") || remainingTokDoc.Any(tok => tok.Word.ToUpper() == "ON"))
            {
                if (script.SchedulingConfig.IntervalType == IntervalType.Week)
                {
                    var intervallToken = remainingTokDoc.First(tok => tok.Word.ToUpper() == "ON");
                    switch (intervallToken.NextToken.Word.ToUpper())
                    {
                        case "MONDAY": return 1;
                        case "TUESDAY": return 2;
                        case "WENDSDAY": return 3;
                        case "THURSDAY": return 4;
                        case "FRIDAY": return 5;
                        case "SATURDAY": return 6;
                        case "SUNDAY": return 7;
                        default: return -2;
                    }
                }
                if (script.SchedulingConfig.IntervalType == IntervalType.Month)
                {
                    remainingTokDoc = script.GetRemainingTokDoc(remainingTokDoc.First(tok => tok.Word.ToUpper() == "AT"));

                    return IntegerExtension.ParseEnglish(remainingTokDoc.First(tok => tok.PartOfSpech == "ADJ").Word);
                }
            }
            return -1;
        }


    }
}
