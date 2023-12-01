using TinyCsvParser.Mapping;

namespace UgLottery;

internal class CsvLotteryMapping : CsvMapping<LotterySignup>
{
    public CsvLotteryMapping()
        : base()
    {
        MapProperty(11, x => x.Company);
        MapProperty(13, x => x.January);
        MapProperty(14, x => x.February);
        MapProperty(15, x => x.March);
        MapProperty(16, x => x.April);
        MapProperty(17, x => x.May);
        MapProperty(18, x => x.June);
        MapProperty(19, x => x.July);
        MapProperty(20, x => x.August);
        MapProperty(21, x => x.September);
        MapProperty(22, x => x.October);
        MapProperty(23, x => x.December);
    }
}