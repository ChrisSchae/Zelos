using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZelosFramework.NLP_Core
{
    public class ScriptRun
    {
        public Script Script { get; set; }
        
        public DateTime ExecutionTime { get; set; }

        public SuccessState SuccessState { get; set; }
    }

    public enum SuccessState
    {
        Running,
        Success,
        Failed,
        Canceled
    }
}
