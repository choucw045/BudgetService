namespace BudgetService;

public class InvalidDateRangeException : Exception
{
    public InvalidDateRangeException() : base("Invalid DateRange")
    {
    }
}