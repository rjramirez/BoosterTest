﻿using Booster.CodingTest.Library;
using RnrNominationPeriodStatus.Services.Interfaces;
using Serilog;

namespace RnrNominationPeriodStatus.Services
{
    public class WordStreamService : IWordStreamService
    {
        public async Task<string> GetTotalNumberOfCharactersAndWords(string val) 
        {


            WordStream ws = new();
            var ls = NLipsum.Core.LipsumGenerator.Generate(2);
            var lsU = NLipsum.Core.LipsumUtilities.GetTextFromRawXml(ls);
            ws.;

            return test;
        }

        //private readonly HttpClient _httpClient;
        //public ApplicationCountryService(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClient = httpClientFactory.CreateClient(HttpNamedClientConstant.HrSuiteApiClient);
        //}

        //public async Task<IEnumerable<string>> GetAffectedCountries()
        //{
        //    DateTime dateStarted = DateTime.Now;
        //    var response = await _httpClient.GetAsync($"api/hrs/applicationCountry/application/{ApplicationConstant.RnRApplicationId}");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        IEnumerable<string> countries = JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());

        //        TimeSpan timeSpan = DateTime.Now - dateStarted;
        //        Log.Logger.Information($"Completed getting countries({countries.Count()}) from API: {timeSpan}");

        //        return countries;
        //    }
        //    else
        //    {
        //        ErrorMessage errorMessage = await response.GetErrorMessage();
        //        throw new ArgumentException($"Status Code: {response.StatusCode}, Reason Phrase: {response.ReasonPhrase}, TraceId: {errorMessage?.TraceId}, Message: {errorMessage?.Message}");
        //    }
        //}

    }
}