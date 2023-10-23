using Common.DataTransferObjects.Period;

namespace RnrNominationPeriodStatus.Services.Interfaces
{
    public interface IPeriodService
    {
        Task<PeriodStatusChangeDetail> GetAffectedPeriod();
        Task SendNotificationForPeriodOpening(PeriodStatusChangeNotification periodStatusChangeNotification);
    }
}
