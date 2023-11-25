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
            var validDaysCountByMonth = DateRangeHelper.GetDate(start, end);
            var budgets = _budgetRepo.GetAll();
            return validDaysCountByMonth.Select(c =>
            {
                var budget = budgets.FirstOrDefault(b => b.GetYear() == c.year && b.GetMonth() == c.month);
                var budgetPerDay = budget?.GetBudgetPerDay() ?? 0;
                return budgetPerDay * c.dayCountOfMonth;
            }).Sum();
        }
        catch (InvalidDateRangeException)
        {
            return 0;
        }
    }
}