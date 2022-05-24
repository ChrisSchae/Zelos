namespace ZelosFramework.NLP_Core
{
    public class AnalysisToken
    {        
        public AnalysisToken(string word, int positionInText, string pos, AnalysisToken previousToken)
        {
            Word = word;
            PositionInText = positionInText;
            PartOfSpech = pos;
            PreviousToken = previousToken;
        }

        public string Word { get; }
        public string PartOfSpech { get; }
        public int PositionInText { get; }
        public AnalysisToken PreviousToken { get; }
        public AnalysisToken NextToken { get; internal set; }
    }
}
