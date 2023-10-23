namespace Common.DataTransferObjects.WordStream
{
    public class WordStreamResultDetail
    {
        public int CharacterCount { get; set; } = 0;
        public int WordCount { get; set; } = 0;
        public Dictionary<string, int> WordFrequency { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<char, int> CharFrequency { get; set; }
        public List<string> LargestWords { get; set; }
        public List<string> SmallestWords { get; set; }
        public List<string> LipsumWords { get; set; }
    }
}
