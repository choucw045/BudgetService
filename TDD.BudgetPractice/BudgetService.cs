namespace BudgetService;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        try
        {
            var budgets = _budgetRepo.GetAll();
            return DateRangeHelper.SplitDaysCountByMonth(start, end).Sum(c =>
            {
                var budget = budgets.FirstOrDefault(b => b.IsSameYearAndMonth(c.Year, c.Month));
                var budgetPerDay = budget?.GetBudgetPerDay() ?? 0;
                return budgetPerDay * c.DayCount;
            });
        }
        catch (InvalidDateRangeException)
        {
            return 0;
        }
    }
}