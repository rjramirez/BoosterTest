using Common.DataTransferObjects.WordStream;

namespace WordStream.Services.Interfaces
{
    public interface IWordStreamService
    {
        Task<WordStreamResultDetail> AddWord(string currentWord);
        Task<bool> HasLimpsumWord(IEnumerable<string> lipsumWords, string currentWord);
    }
}
