namespace RnrNominationPeriodStatus.Services.Interfaces
{
    public interface IWordStreamService
    {
        Task<string> GetTotalNumberOfCharactersAndWords();
    }
}
