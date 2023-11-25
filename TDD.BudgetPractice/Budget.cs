namespace BudgetService;

public class Budget
{
    public string YearMonth { get; set; } = null!;
    public int Amount { get; set; }

    public int GetYear()
    {
        return int.Parse(YearMonth.Substring(0, 4));
    }

    public int GetMonth()
    {
        return int.Parse(YearMonth.Substring(4, 2));
    }

    public int GetBudgetPerDay()
    {
        var daysInMonth = DateTime.DaysInMonth(GetYear(), GetMonth());
        return Amount / daysInMonth;
    }

    public bool IsSameYearAndMonth(int year, int month)
    {
        return GetYear() == year && GetMonth() == month;
    }
}