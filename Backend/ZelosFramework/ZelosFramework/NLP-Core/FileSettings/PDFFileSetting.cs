using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelosFramework.FileHandling;

namespace ZelosFramework.NLP_Core.FileSettings
{
    public class PDFFileSetting : FileSetting
    {
        public PDFFileSetting()
        {
            this.FileType = FileType.PDF;
        }
    }
}
