using Common.DataTransferObjects.Employee;

namespace RnrNominationPeriodStatus.Services.Interfaces
{
    public interface IGlobalMappingService
    {
        Task<IEnumerable<EmployeeBasicDetail>> GetSupervisorBasicDetailsByCountry(IEnumerable<string> countries);
    }
}
