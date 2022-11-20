namespace UgLottery;

public class MonthEntries
{
    public MonthEntries(string month)
    {
        Month = month;
    }
    public string Month { get; set; }
    public List<string> SponsorNames { get; } = new List<string>();
}