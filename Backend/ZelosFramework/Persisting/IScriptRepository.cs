using System.Collections.Generic;
using ZelosFramework.NLP_Core;

namespace Persisting
{
    public interface IScriptRepository
    {
        Script GetScriptByName(string name);
        Script AddScript(Script doc);
        IEnumerable<Script> GetScripts(int maxCount = 10);
    }
}
