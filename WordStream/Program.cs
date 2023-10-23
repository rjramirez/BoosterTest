using WordStream.Services;
using WordStream.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLipsum.Core;
using Serilog;
using System.Reflection;

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

StartProcess(host);

static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
{
    Exception ex = (Exception)args.ExceptionObject;
    Log.Logger.Error("Error Message: {message}, Stack Trace: {stackTace}", ex.Message, ex.StackTrace);
}

static async void StartProcess(IHost host)
{
    IWordStreamService wordStreamService = ActivatorUtilities.CreateInstance<WordStreamService>(host.Services);

    var stream = new Booster.CodingTest.Library.WordStream();

    int characterCount = 0;
    int wordCount = 0;
    Dictionary<string, int> wordFrequency = new(StringComparer.OrdinalIgnoreCase);
    Dictionary<char, int> charFrequency = new();
    List<string> largestWords = new();
    List<string> smallestWords = new();
    List<string> lipsumWords = new();

    //Get the lipsum words and add it to the list
    Type type = typeof(Lipsums);
    PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);

    properties.ToList().ForEach(property =>
    {
        lipsumWords.Add(property.Name);
    });

    string currentWord = string.Empty;

    while (stream.CanRead)
    {
        int nextChar = stream.ReadByte();

        if (nextChar == -1)
            break;

        char character = (char)nextChar;
        characterCount++;

        // Update character frequency
        if (charFrequency.ContainsKey(character))
            charFrequency[character]++;
        else
            charFrequency[character] = 1;

        // Process words
        if (char.IsLetterOrDigit(character) || character == '\'')
        {
            currentWord += character;
            wordCount++;
        }
        else if (!string.IsNullOrEmpty(currentWord))
        {
            /*
             TODO: I intentionally cut it to 11 since the stream could be endless. Update as necessary
             */
            if (wordFrequency.Count() > 10)
                break;

            // Update word frequency
            if (wordFrequency.ContainsKey(currentWord))
                wordFrequency[currentWord]++;
            else
                wordFrequency[currentWord] = 1;


            // Update largest and smallest words lists
            if (largestWords.Count < 5 || currentWord.Length > largestWords.Min(w => w.Length))
            {
                if (largestWords.Count >= 5)
                    largestWords.Remove(largestWords.First(w => w.Length == largestWords.Min(w => w.Length)));

                largestWords.Add(currentWord);
            }

            if (smallestWords.Count < 5 || currentWord.Length < smallestWords.Max(w => w.Length))
            {
                if (smallestWords.Count >= 5)
                    smallestWords.Remove(smallestWords.First(w => w.Length == smallestWords.Max(w => w.Length)));

                smallestWords.Add(currentWord);
            }

            //TODO: Check if maybe contained in given lipsum words? Remove if not needed.
            bool hasLimpsumWord = await wordStreamService.HasLimpsumWord(lipsumWords, currentWord);

            if (!hasLimpsumWord)
            {
                currentWord = string.Empty;
                continue;
            }
        }
    }

    // Sort the word frequency dictionary by value (descending)
    var sortedWordFrequency = wordFrequency.OrderByDescending(pair => pair.Value)
                                           .Take(10)
                                           .ToDictionary(pair => pair.Key, pair => pair.Value);

    Log.Information($"Total number of characters: {characterCount}");
    Log.Information($"Total number of words: {wordCount}");
    Log.Information("5 Largest Words: " + string.Join(", ", largestWords));
    Log.Information("5 Smallest Words: " + string.Join(", ", smallestWords));
    Log.Information("10 Most frequently appearing words:");

    foreach (var entry in sortedWordFrequency)
    {
        Log.Information($"{entry.Key}: {entry.Value} times");
    }

    //Skip empty char, since it is the word separator in the stream
    foreach (var entry in charFrequency.OrderByDescending(pair => pair.Value).Where(pair => pair.Key != ' '))
    {
        Log.Information($"{entry.Key}: {entry.Value} times");
    }

    //TODO: Open console. can be removed
    Console.ReadLine();
}
