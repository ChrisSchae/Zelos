﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelosFramework.FileHandling;

namespace ZelosFramework.NLP_Core.FileSettings
{
    public class ZIPFileSetting : FileSetting
    {
        public FileSetting ExtractedFileSettings { get; set; }

        public ZIPFileSetting()
        {
            this.FileType = FileType.ZIP;
        }
    }
}
