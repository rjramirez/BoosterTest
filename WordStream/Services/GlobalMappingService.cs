using Common.Constants;
using Common.DataTransferObjects.Employee;
using Common.DataTransferObjects.ErrorLog;
using Newtonsoft.Json;
using RnrNominationPeriodStatus.Extensions;
using RnrNominationPeriodStatus.Services.Interfaces;
using Serilog;

namespace RnrNominationPeriodStatus.Services
{
    public class GlobalMappingService : IGlobalMappingService
    {
        private readonly HttpClient _httpClient;
        public GlobalMappingService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpNamedClientConstant.GlobalMappingApiClient);
        }

        public async Task<IEnumerable<EmployeeBasicDetail>> GetSupervisorBasicDetailsByCountry(IEnumerable<string> countries)
        {
            DateTime dateStarted = DateTime.Now;
            List<EmployeeBasicDetail> supervisorBasicDetails = new();

            foreach (string country in countries)
            {
                int pageNumber = 1;
                int pageSize = 100;
                int pageResult = 100;

                while (pageResult != 0)
                {
                    var response = await _httpClient.GetAsync($"api/Supervisor/CountryName/{country}?PageNumber={pageNumber}&PageSize={pageSize}");
                    if (response.IsSuccessStatusCode)
                    {
                        IEnumerable<EmployeeBasicDetail> employeeBasicDetails = JsonConvert.DeserializeObject<IEnumerable<EmployeeBasicDetail>>(await response.Content.ReadAsStringAsync());
                        supervisorBasicDetails.AddRange(employeeBasicDetails);
                        pageNumber++;
                        pageResult = employeeBasicDetails.Count();
                    }
                    else
                    {
                        ErrorMessage errorMessage = await response.GetErrorMessage();
                        throw new ArgumentException($"Status Code: {response.StatusCode}, Reason Phrase: {response.ReasonPhrase}, TraceId: {errorMessage?.TraceId}, Message: {errorMessage?.Message}");
                    }
                }
            }

            TimeSpan timeSpan = DateTime.Now - dateStarted;
            Log.Logger.Information($"Completed getting supervisors({supervisorBasicDetails.Count}) from API: {timeSpan}");

            return supervisorBasicDetails;
        }
    }
}