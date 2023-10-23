namespace RnrNominationPeriodStatus.Services.Interfaces
{
    public interface IApplicationCountryService
    {
        Task<IEnumerable<string>> GetAffectedCountries();
    }
}
