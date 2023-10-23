using Common.DataTransferObjects.Employee;

namespace RnrNominationPeriodStatus.Services.Interfaces
{
    public interface IMicrosoftGraphService
    {
        Task GetUsersEmail(IEnumerable<EmployeeBasicDetail> employeeBasicDetails);
    }
}
