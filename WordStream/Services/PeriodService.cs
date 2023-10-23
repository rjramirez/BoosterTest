using Common.Constants;
using Common.DataTransferObjects.ErrorLog;
using Common.DataTransferObjects.Period;
using Newtonsoft.Json;
using RnrNominationPeriodStatus.Extensions;
using RnrNominationPeriodStatus.Services.Interfaces;
using Serilog;

namespace RnrNominationPeriodStatus.Services
{
    public class PeriodService : IPeriodService
    {
        private readonly HttpClient _httpClient;
        public PeriodService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpNamedClientConstant.HrSuiteApiClient);
        }

        public async Task<PeriodStatusChangeDetail> GetAffectedPeriod()
        {
            DateTime dateStarted = DateTime.Now;

            var response = await _httpClient.PostAsync($"api/rnr/period/ActivateOrDeactivate", null);
            if (response.IsSuccessStatusCode)
            {
                PeriodStatusChangeDetail periodStatusChangeDetail = JsonConvert.DeserializeObject<PeriodStatusChangeDetail>(await response.Content.ReadAsStringAsync());

                TimeSpan timeSpan = DateTime.Now - dateStarted;
                Log.Logger.Information($"Completed getting period status change, Opened({periodStatusChangeDetail.PeriodsMovedToOpen.Count()}) and Deliberation({periodStatusChangeDetail.PeriodsMovedToDeliberation.Count()}) from API: {timeSpan}");
                return periodStatusChangeDetail;
            }
            else
            {
                ErrorMessage errorMessage = await response.GetErrorMessage();
                throw new ArgumentException($"Status Code: {response.StatusCode}, Reason Phrase: {response.ReasonPhrase}, TraceId: {errorMessage?.TraceId}, Message: {errorMessage?.Message}");
            }
        }

        public async Task SendNotificationForPeriodOpening(PeriodStatusChangeNotification periodStatusChangeNotification)
        {
            IEnumerable<string[]> chunkedRecipientEmails = periodStatusChangeNotification.RecipientEmails.Chunk(100);
            int emailSent = 0;
            foreach (string[] chunkedRecipientEmail in chunkedRecipientEmails)
            {
                DateTime dateStarted = DateTime.Now;

                PeriodStatusChangeNotification chunckedPeriodStatusChangeNotification = new PeriodStatusChangeNotification()
                {
                    PeriodId = periodStatusChangeNotification.PeriodId,
                    RecipientEmails = chunkedRecipientEmail.ToList()
                };

                var response = await _httpClient.PostAsync($"api/rnr/period/NotifyLeadsForOpenNomination", chunckedPeriodStatusChangeNotification.GetStringContent());
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage errorMessage = await response.GetErrorMessage();
                    throw new ArgumentException($"Status Code: {response.StatusCode}, Reason Phrase: {response.ReasonPhrase}, TraceId: {errorMessage?.TraceId}, Message: {errorMessage?.Message}");
                }

                emailSent += chunkedRecipientEmail.Length;

                TimeSpan timeSpan = DateTime.Now - dateStarted;
                Log.Logger.Information($"Completed sending email({emailSent}/{periodStatusChangeNotification.RecipientEmails.Count}) to API: {timeSpan}");
            }
        }
    }
}