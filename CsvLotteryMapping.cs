using TinyCsvParser.Mapping;

namespace UgLottery;

internal class CsvLotteryMapping : CsvMapping<LotterySignup>
{
    public CsvLotteryMapping()
        : base()
    {
        MapProperty(7, x => x.Company);
        MapProperty(8, x => x.January);
        MapProperty(9, x => x.February);
        MapProperty(10, x => x.March);
        MapProperty(11, x => x.April);
        MapProperty(12, x => x.May);
        MapProperty(13, x => x.June);
        MapProperty(14, x => x.July);
        MapProperty(15, x => x.August);
        MapProperty(16, x => x.September);
        MapProperty(17, x => x.October);
        MapProperty(18, x => x.December);
    }
}