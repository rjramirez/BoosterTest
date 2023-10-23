using Booster.CodingTest.Library;
using BoosterTest.Services.Interfaces;
using Common.DataTransferObjects.WordStream;

namespace BoosterTest.Services
{
    public class WordStreamService : IWordStreamService
    {
        public async Task<WordStreamResultDetail> AddWord(string currentWord) 
        {
            WordStreamResultDetail wordStreamResultDetail = new();

            // Update word frequency
            if (wordStreamResultDetail.WordFrequency.ContainsKey(currentWord))
                wordStreamResultDetail.WordFrequency[currentWord]++;
            else
                wordStreamResultDetail.WordFrequency[currentWord] = 1;


            // Update largest and smallest words lists
            if (wordStreamResultDetail.LargestWords.Count() < 5 || currentWord.Length > wordStreamResultDetail.LargestWords.Min(w => w.Length))
            {
                if (wordStreamResultDetail.LargestWords.Count() >= 5)
                    wordStreamResultDetail.LargestWords.Remove(wordStreamResultDetail.LargestWords.First(w => w.Length == wordStreamResultDetail.LargestWords.Min(w => w.Length)));

                wordStreamResultDetail.LargestWords.Add(currentWord);
            }

            if (wordStreamResultDetail.SmallestWords.Count < 5 || currentWord.Length < wordStreamResultDetail.SmallestWords.Max(w => w.Length))
            {
                if (wordStreamResultDetail.SmallestWords.Count >= 5)
                    wordStreamResultDetail.SmallestWords.Remove(wordStreamResultDetail.SmallestWords.First(w => w.Length == wordStreamResultDetail.SmallestWords.Max(w => w.Length)));

                wordStreamResultDetail.SmallestWords.Add(currentWord);
            }

            return wordStreamResultDetail;
        }
    }
}