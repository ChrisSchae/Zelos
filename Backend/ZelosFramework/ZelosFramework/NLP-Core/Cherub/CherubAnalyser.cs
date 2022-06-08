using System;
using System.Collections.Generic;
using CherubNLP;
using CherubNLP.Tag;
using CherubNLP.Tokenize;

namespace ZelosFramework.NLP_Core.Cherub
{
    public class CherubAnalyser : INlpAnalyzer
    {
        public Script Analyse(string script)
        {
            var result = new Script();
            result.ScriptString = script;
            return Analyse(result);
        }

        public Script Analyse(Script script)
        {
            var tokenizer = new TokenizerFactory(new TokenizationOptions
            {
            }, SupportedLanguage.English);
            tokenizer.GetTokenizer<TreebankTokenizer>();
            var tokens = tokenizer.Tokenize(script.ScriptString);
            //var tagger = new TaggerFactory(new TagOptions(), SupportedLanguage.English);

            //tagger.GetTagger<NGramTagger>();
            var sentence = new Sentence { Words = tokens };
            //tagger.Tag(sentence);
            EnrichTokens(script, sentence.Words);
            return script;
        }

        private void EnrichTokens(Script script, List<Token> tokenData)
        {
            int counter = 0;

            AnalysisToken prevTok = null;
            foreach (var tok in tokenData)
            {
                var tokWord = tok.Text;
                var currTok = new AnalysisToken(tokWord, counter, "" /*tok.Pos.ToString()*/, prevTok);
                script.TokenizedDoc.Add(currTok);
                if (prevTok != null)
                {
                    prevTok.NextToken = currTok;
                }
                prevTok = currTok;
                counter++;
            }

        }
    }
}
