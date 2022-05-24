using System;
using System.Collections.Generic;
using System.Text;

namespace ZelosFramework.NLP_Core
{
    public interface INlpAnalyzer
    {
        public Script Analyse(string script);
        public Script Analyse(Script script);
    }
}
