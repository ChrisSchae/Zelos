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
    public partial class CatalystAnalyser : INlpAnalyzer
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
            DetermineFileSetting(script);
            DetermineScheduling(script);
            ProcessSourceInformation(script);

            return script;
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
