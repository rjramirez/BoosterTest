using Common.Constants;
using Common.DataTransferObjects.Employee;
using Common.DataTransferObjects.MicrosoftGraph;
using Newtonsoft.Json;
using RnrNominationPeriodStatus.Services.Interfaces;
using Serilog;

namespace RnrNominationPeriodStatus.Services
{
    public class MicrosoftGraphService : IMicrosoftGraphService
    {
        private readonly HttpClient _httpClient;
        public MicrosoftGraphService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpNamedClientConstant.MicrosoftGraphApiClient);
        }

        public async Task GetUsersEmail(IEnumerable<EmployeeBasicDetail> employeeBasicDetails)
        {
            DateTime dateStarted = DateTime.Now;

            Parallel.ForEach(employeeBasicDetails, employeeBasicDetail =>
            {
                MsGraphUserDetail msGraphUserDetail = GetEmployeeDetailByEmployeeId(employeeBasicDetail.EmployeeId).Result;

                if (msGraphUserDetail != null)
                {
                    employeeBasicDetail.EmailAddress = msGraphUserDetail.Mail;
                }
            });

            TimeSpan timeSpan = DateTime.Now - dateStarted;
            Log.Logger.Information($"Completed getting emails({employeeBasicDetails.Count(e => !String.IsNullOrEmpty(e.EmailAddress))}) from MS Graph API: {timeSpan}");
        }

        public async Task<MsGraphUserDetail> GetEmployeeDetailByEmployeeId(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                var response = await _httpClient.GetAsync($"v1.0/users?$top=1&$filter=employeeId eq '{employeeId}'" +
                    $"&$select=id,givenName,surname,mail,employeeId,userPrincipalName");

                if (response.IsSuccessStatusCode)
                {
                    MsGraphUserDetailResult msGraphUserSearchResult = JsonConvert.DeserializeObject<MsGraphUserDetailResult>(await response.Content.ReadAsStringAsync());
                    if (msGraphUserSearchResult.Value.Any())
                    {
                        return msGraphUserSearchResult.Value.First();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }
    }
}