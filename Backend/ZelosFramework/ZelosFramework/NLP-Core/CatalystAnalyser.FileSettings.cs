using System;
using System.Collections.Generic;
using System.Linq;
using ZelosFramework.FileHandling;
using ZelosFramework.NLP_Core.FileSettings;

namespace ZelosFramework.NLP_Core
{
    public partial class CatalystAnalyser
    {
        private void DetermineFileSetting(Script script)
        {
            var fileTypeNames = new List<string>(Enum.GetNames(typeof(FileType)));
            if (script.TokenizedDoc.Any(t => fileTypeNames.Any(typeName => typeName.ToLower() == t.Word.ToLower())))
            {
                var firstTypeToken = script.TokenizedDoc.First(t => fileTypeNames.Any(typeName => typeName.ToLower() == t.Word.ToLower()));
                script.FileSettings = ParseFileSettings(script, firstTypeToken);
            }
        }

        private static FileSetting ParseFileSettings(Script script, AnalysisToken firstTypeToken)
        {
            var fileType = Enum.Parse<FileType>(firstTypeToken.Word.ToUpper());
            switch (fileType)
            {
                case FileType.CSV: return ParseCSVFileSetting(script);
                case FileType.EXCEL: return new EXCELFileSetting();
                case FileType.JSON: return new JSONFileSetting();
                case FileType.PDF: return new PDFFileSetting();
                case FileType.TXT: return new TXTFileSetting();
                case FileType.XML: return new XMLFileSetting();
                case FileType.ZIP: return new ZIPFileSetting();
                default: return new DefaultFileSetting();
            }
        }

        private static FileSetting ParseCSVFileSetting(Script script)
        {
            var numbersInText = script.TokenizedDoc.Where(tok => tok.PartOfSpech == "NUM");
            var result = new CSVFileSetting();

            foreach(var numberTok in numbersInText)
            {
                if((numberTok.PreviousToken.Word.ToLower() == "minimum" || numberTok.PreviousToken.Word.ToLower() == "min") && numberTok.NextToken.Word.ToLower().StartsWith("col"))
                {
                    result.ExpectedMinCols = int.Parse(numberTok.Word);
                }

                if ((numberTok.PreviousToken.Word.ToLower() == "maximum" || numberTok.PreviousToken.Word.ToLower() == "max") && numberTok.NextToken.Word.ToLower().StartsWith("col"))
                {
                    result.ExpectedMaxCols = int.Parse(numberTok.Word);
                }

                if (numberTok.PreviousToken.Word.ToLower() == "row" && numberTok.PreviousToken.PreviousToken.PreviousToken.Word.ToLower() == "header")
                {
                    result.HeaderRow = int.Parse(numberTok.Word);
                }
            }

            var valueToken = script.TokenizedDoc.FirstOrDefault(tok=> tok.Word.ToLower() == "values");
            while(valueToken != null && valueToken.NextToken != null && valueToken?.NextToken?.PartOfSpech != "NUM")
            {
                valueToken = valueToken.NextToken;
            }

            if(valueToken == null || valueToken.NextToken == null)
            {
                return result;
            }

            var columnToken = valueToken.NextToken;
            List<int> valColumns = new List<int>();
            while (columnToken?.PartOfSpech == "NUM" || columnToken?.PartOfSpech == "PUNCT" || columnToken?.PartOfSpech == "CCONJ")
            {
                if(columnToken.PartOfSpech == "NUM")
                {
                    valColumns.Add(int.Parse(columnToken.Word));
                }
                columnToken = columnToken.NextToken;
            }

            result.ValueCols = valColumns.ToArray();



            return result;
        }
    }
}
