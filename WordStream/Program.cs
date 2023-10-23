using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RnrNominationPeriodStatus.Services;
using RnrNominationPeriodStatus.Services.Interfaces;
using Serilog;

//App settings
var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

IConfiguration config = builder.Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .Enrich.FromLogContext()
    .CreateLogger();

AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        
        services.AddScoped<IWordStreamService, WordStreamService>();
    })
    .UseSerilog()
    .Build();

await StartProcess(host);

static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
{
    Exception ex = (Exception)args.ExceptionObject;
    Log.Logger.Error("Error Message: {message}, Stack Trace: {stackTace}", ex.Message, ex.StackTrace);
}

static async Task StartProcess(IHost host)
{
    //IApplicationCountryService applicationCountryService = ActivatorUtilities.CreateInstance<ApplicationCountryService>(host.Services);
    //IPeriodService periodService = ActivatorUtilities.CreateInstance<PeriodService>(host.Services);
    //IGlobalMappingService globalMappingService = ActivatorUtilities.CreateInstance<GlobalMappingService>(host.Services);
    //IMicrosoftGraphService microsoftGraphService = ActivatorUtilities.CreateInstance<MicrosoftGraphService>(host.Services);

    //PeriodStatusChangeDetail periodStatusChangeDetail = await periodService.GetAffectedPeriod();

    //if (periodStatusChangeDetail.PeriodsMovedToOpen.Any())
    //{
    //    IEnumerable<string> affectedCountries = await applicationCountryService.GetAffectedCountries();
    //    foreach (short periodId in periodStatusChangeDetail.PeriodsMovedToOpen)
    //    {
    //        if (affectedCountries.Any())
    //        {
    //            IEnumerable<EmployeeBasicDetail> supervisorBasicDetails = await globalMappingService.GetSupervisorBasicDetailsByCountry(affectedCountries);
    //            await microsoftGraphService.GetUsersEmail(supervisorBasicDetails);
    //            if (supervisorBasicDetails.Any(x => !String.IsNullOrEmpty(x.EmailAddress!)))
    //            {
    //                PeriodStatusChangeNotification periodStatusChangeNotification = new PeriodStatusChangeNotification()
    //                {
    //                    PeriodId = periodId,
    //                    RecipientEmails = supervisorBasicDetails.Where(x => !String.IsNullOrEmpty(x.EmailAddress!)).Select(x => x.EmailAddress).ToList()
    //                };

    //                await periodService.SendNotificationForPeriodOpening(periodStatusChangeNotification);
    //            }
    //        }
    //    }
    //}

    IWordStreamService wordStreamService = ActivatorUtilities.CreateInstance<WordStreamService>(host.Services);
}