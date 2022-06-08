using System;
using java.io;
using opennlp.tools.postag;
using opennlp.tools.tokenize;

namespace ZelosFramework.NLP_Core.OpenNLP
{
    public class OpenNLPAnalyser : INlpAnalyzer
    {
        public Script Analyse(string script)
        {
            var result = new Script();
            result.ScriptString = script;
            return Analyse(result);
        }

        public Script Analyse(Script script)
        {
            TokenizerModel tokModel = null;
            POSModel posModel = null;

            using (var modelIn = new FileInputStream("C:\\Users\\ch-sc\\Documents\\Studium\\BA\\OpenNLP\\Models\\en-token.bin"))
            {
                tokModel = new TokenizerModel(modelIn); 
            }

            using (var modelIn = new FileInputStream("C:\\Users\\ch-sc\\Documents\\Studium\\BA\\OpenNLP\\Models\\en-pos.bin"))
            {
                posModel = new POSModel(modelIn);
            }
            var tokenizer = new TokenizerME(tokModel);
            var tokenizedDoc = tokenizer.tokenize(script.ScriptString);
            POSTaggerME tagger = new POSTaggerME(posModel);

            var tags = tagger.tag(tokenizedDoc);
            AnalysisToken prevToken = null;
            for (int i = 0; i < tokenizedDoc.Length; i++)
            {
                var workingToken = new AnalysisToken(tokenizedDoc[0], i, tags[i], prevToken);
                prevToken.NextToken = workingToken;
                script.TokenizedDoc.Add(workingToken);
                prevToken = workingToken;
            }


            throw new NotImplementedException();
        }
    }
}
