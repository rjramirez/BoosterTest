namespace Common.DataTransferObjects.WordStream
{
    public class WordStreamResultDetail
    {
        public IEnumerable<WordDetail> Words { get; set; }

        public string Message { get; set; }
    }
}
