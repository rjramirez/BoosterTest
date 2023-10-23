using NLipsum.Core;
using System.Reflection;
using WordStream.Services;

namespace BoosterTesting
{
    public class Tests
    {
        private WordStreamService _wordStreamService;
        private List<string> lipsumWords = new();

        [SetUp]
        public void Setup()
        {
            _wordStreamService = new WordStreamService();

            //Get the lipsum words and add it to the list
            Type type = typeof(Lipsums);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);

            properties.ToList().ForEach(property =>
            {
                lipsumWords.Add(property.Name);
            });
        }

        [Test]
        public async Task HasLimpsumWordCheckAsync()
        {
            bool result = await _wordStreamService.HasLimpsumWord(lipsumWords, lipsumWords.First());

            Assert.IsTrue(result, "Part of the list");
        }
    }
}