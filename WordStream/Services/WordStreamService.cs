using Booster.CodingTest.Library;
using Common.DataTransferObjects.WordStream;
using RnrNominationPeriodStatus.Services.Interfaces;

namespace RnrNominationPeriodStatus.Services
{
    public class WordStreamService : IWordStreamService
    {
        public async Task<WordStreamResultDetail> GetTotalNumberOfCharactersAndWords(string val) 
        {
            WordStreamResultDetail wsDetail = new();
            WordStream ws = new();
            var lsU = NLipsum.Core.LipsumUtilities.GetTextFromRawXml(val);
            ws.

            return wsDetail;
        }
    }
}