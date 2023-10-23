using Common.DataTransferObjects.WordStream;
using Microsoft.Extensions.Primitives;

namespace RnrNominationPeriodStatus.Services.Interfaces
{
    public interface IWordStreamService
    {
       async Task<WordStreamResultDetail> GetTotalNumberOfCharactersAndWords();
    }
}
