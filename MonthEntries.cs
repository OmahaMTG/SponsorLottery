namespace UgLottery;

public class MonthEntries
{
    public MonthEntries(string month)
    {
        Month = month;
    }
    public string Month { get; set; }

    public void RandomizeSponsorOrder()
    {
        SponsorNames = SponsorNames.OrderBy(_ => Guid.NewGuid().ToString()).ToList();
    }
    public List<string> SponsorNames { get; private set; } = new List<string>();


}