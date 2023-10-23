using Common.DataTransferObjects.WordStream;

namespace BoosterTest.Services.Interfaces
{
    public interface IWordStreamService
    {
        Task<WordStreamResultDetail> AddWord(string currentWord);
    }
}
