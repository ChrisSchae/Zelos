using ZelosFramework.FileHandling;

namespace ZelosFramework.NLP_Core.FileSettings
{
    public class CSVFileSetting : FileSetting
    {
        public int HeaderRow { get; set; } = -1;

        public int ExpectedMinCols { get; set; } = -1;

        public int ExpectedMaxCols { get; set; } = -1;

        public int[] ValueCols { get; set; }

        public char Delimiter { get; set; } = ',';

        public CSVFileSetting()
        {
            this.FileType = FileType.CSV;
        }
    }
}
