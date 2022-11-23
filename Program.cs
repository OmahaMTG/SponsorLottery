using System;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;
using UgLottery;

var result = ParseCsv(@".\Signups.csv");

var monthSponsorsList = AssociateMonthsToSponsors(result);

ShowStatistics(monthSponsorsList);

ValidateSponsorsDoNotHaveTooManySignups(monthSponsorsList);

SelectAndShowWinners(monthSponsorsList);

List<CsvMappingResult<LotterySignup>> ParseCsv(string filePath)
{
    var tokenizer = new RFC4180Tokenizer(new Options('"', '\\', ','));
    var csvParserOptions = new CsvParserOptions(true, tokenizer);
    var csvMapper = new CsvLotteryMapping();
    var csvParser = new CsvParser<LotterySignup>(csvParserOptions, csvMapper);
    var csvMappingResults = csvParser
        .ReadFromFile(filePath, Encoding.ASCII)
        .ToList();
    return csvMappingResults;
}



List<MonthEntries> AssociateMonthsToSponsors(List<CsvMappingResult<LotterySignup>> signupsBySponsor)
{
    var monthEntriesList = new List<MonthEntries>();
    //Loop over each sponsorship month
    foreach (var month in Enum.GetNames(typeof(Months)))
    {
        var monthSponsors = new MonthEntries(month);
        var prop = typeof(LotterySignup).GetProperty(month);
        if (prop == null)
            throw new InvalidOperationException(
                $"The property \"{month}\" could not be found in object of type \"{typeof(LotterySignup)}\".");

        //Find all of the sponsors that requested this month
        foreach (var sponsor in signupsBySponsor)
        {
            var lotterySignup = sponsor.Result;
            var company = lotterySignup.Company;
            var numberOfEntries = prop.GetValue(lotterySignup) as int?;
            if (numberOfEntries == null)
                throw new InvalidOperationException($"The number of entries for sponsor \"{company}\" in month \"{month}\" could not be parsed.");
            var monthValue = numberOfEntries.Value;
            monthSponsors.SponsorNames.AddRange(Enumerable.Range(0, monthValue).Select(_ => company));
        }

        monthSponsors.RandomizeSponsorOrder();

        monthEntriesList.Add(monthSponsors);
    }

    return monthEntriesList;
}

void SelectAndShowWinners(List<MonthEntries> monthSponsorsList1)
{
    ConsoleWriteLineWithColor("****************************", ConsoleColor.Yellow);
    ConsoleWriteLineWithColor("** And The Winners Are... **", ConsoleColor.Yellow);
    ConsoleWriteLineWithColor("****************************", ConsoleColor.Yellow);

    var rand = new Random();
    var winners = new List<string>();
    foreach (var month in monthSponsorsList1)
    {
        string winnerName;
        Console.Write($"  {month.Month,-9} -- ");
        do
        {
            //Select the next winner
            var winnerIndex = rand.Next(month.SponsorNames.Count);
            winnerName = month.SponsorNames[winnerIndex];

            //Companies cannot sponsor more than one month per year
            if (!winners.Contains(winnerName))
                break;
        } while (true);

        winners.Add(winnerName);
        
        ConsoleWriteLineWithColor($"{winnerName}", ConsoleColor.Cyan);
    }
}

void ConsoleWriteLineWithColor(string message, ConsoleColor color)
{
    var currentColor = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ForegroundColor = currentColor;
}

void ValidateSponsorsDoNotHaveTooManySignups(List<MonthEntries> list)
{
    var sponsorSignupCount = new Dictionary<string, int>();
    foreach (var monthSponsors in list)
    {
        foreach (var sponsor in monthSponsors.SponsorNames)
        {
            if (sponsorSignupCount.ContainsKey(sponsor))
                sponsorSignupCount[sponsor]++;
            else
                sponsorSignupCount.Add(sponsor, 1);
        }
    }

    var sponsorsWithTooManySignups = sponsorSignupCount.Where(_ => _.Value > 11)
        .Select(_ => new { SponsorName = _.Key, SignUpCount = _.Value }).ToList();

    if (sponsorsWithTooManySignups.Any())
    {
        foreach (var sponsorWithTooManySignups in sponsorsWithTooManySignups)
        {
            Console.WriteLine(
                $"Sponsor {sponsorWithTooManySignups.SponsorName} has {sponsorWithTooManySignups.SignUpCount} lottery entries.  This must be less than 12.");
        }

        throw new InvalidOperationException("One or more sponsors had too many lottery entries.");
    }


}

void ShowStatistics(List<MonthEntries> monthEntriesList1)
{
    var totalSignUps = 0;
    ConsoleWriteLineWithColor("****************", ConsoleColor.Yellow);
    ConsoleWriteLineWithColor("** Statistics **", ConsoleColor.Yellow);
    ConsoleWriteLineWithColor("****************", ConsoleColor.Yellow);

    foreach (var monthSponsor in monthEntriesList1)
    {
        var monthSignups = monthSponsor.SponsorNames.Count;
        totalSignUps += monthSignups;
        Console.WriteLine($"  {monthSponsor.Month} has {monthSignups} sign ups");
    }

    Console.WriteLine();
    Console.WriteLine($"  Total Signups: {totalSignUps}");
    Console.WriteLine();
}