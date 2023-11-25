using FluentAssertions;
using NSubstitute;

namespace TDD.BudgetPractice;

public class Tests
{
    private IBudgetRepo _budgetRepo;
    private BudgetService _budgetService;

    [SetUp]
    public void Setup()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }


    [Test]
    public void TestPeriod()
    {
        var valueTuples = _budgetService.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));
        valueTuples.Count().Should().Be(1);
        valueTuples.First().dayCountOfMonth.Should().Be(1);
    }

    [Test]
    public void TestCrossMonth()
    {
        var valueTuples = _budgetService.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 2, 1));
        valueTuples.Count().Should().Be(2);
        valueTuples.First().dayCountOfMonth.Should().Be(31);
        valueTuples.Last().dayCountOfMonth.Should().Be(1);
    }


    [Test]
    public void QueryFullMonth()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>()
        {
            new Budget()
            {
                YearMonth = "202311",
                Amount = 300
            }
        });
        var result = _budgetService.Query(new DateTime(2023, 11, 1), new DateTime(2023, 11, 30));
        result.Should().Be(300);
    }
}

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        GetDate(start, end);

        var budgets = _budgetRepo.GetAll();
        // var b = budgets.Select(budget => { return budget.GetBudgetPerDay(); }).ToList();
        throw new NotImplementedException();
    }

    public IEnumerable<(int year, int month, int dayCountOfMonth)> GetDate(DateTime start, DateTime end)
    {
        if (start.Year == end.Year && start.Month == end.Month)
        {
            var startDay = end.Day - start.Day;
            yield return (start.Year, start.Month, startDay + 1);
        }
        else
        {
            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            var dayCountOfMonth = daysInMonth - start.Day;
            yield return (start.Year, start.Month, dayCountOfMonth);
            var current = start.AddMonths(1);
            while (current <= end.AddMonths(-1))
            {
                var inMonth = DateTime.DaysInMonth(current.Year, current.Month);
                yield return (current.Year, current.Month, inMonth + 1);

                current = current.AddMonths(1);
            }

            var endDay = end.Day;
            yield return (end.Year, end.Month, endDay);
        }
    }
}

public interface IBudgetRepo
{
    List<Budget> GetAll();
}

public class Budget
{
    public string YearMonth { get; set; }
    public int Amount { get; set; }

    public int GetYear()
    {
        return int.Parse(YearMonth.Substring(4));
    }

    public int GetMongth()
    {
        return int.Parse(YearMonth.Substring(4, 2));
    }

    public void GetBudgetPerDay()
    {
        var daysInMonth = DateTime.DaysInMonth(GetYear(), GetMongth());
        var budgetAmount = Amount / daysInMonth;
    }
}