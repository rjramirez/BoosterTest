using Common.DataTransferObjects.ErrorLog;
using Newtonsoft.Json;

namespace RnrNominationPeriodStatus.Extensions
{
    public static class ResponseMessageExtension
    {
        public static async Task<ErrorMessage> GetErrorMessage(this HttpResponseMessage httpResponseMessage)
        {
            ErrorMessage errorMessage = JsonConvert.DeserializeObject<ErrorMessage>(await httpResponseMessage.Content.ReadAsStringAsync());
            if (errorMessage == null)
            {
                errorMessage = new ErrorMessage()
                {
                    Message = $"{httpResponseMessage.StatusCode} - {httpResponseMessage.RequestMessage.RequestUri.AbsoluteUri}"
                };
            }
            return errorMessage;
        }
    }
}
