using FluentAssertions;
using NSubstitute;

namespace BudgetService;

public class BudgetServiceTests
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
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));
        valueTuples.Count().Should().Be(1);
        valueTuples.First().dayCountOfMonth.Should().Be(1);
    }

    [Test]
    public void GetDateCanHandleCrossMonth()
    {
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 2, 1));
        valueTuples.Count().Should().Be(2);
        valueTuples.First().dayCountOfMonth.Should().Be(31);
        valueTuples.Last().dayCountOfMonth.Should().Be(1);
    }

    [Test]
    public void GetDateCanHandleCrossMultipleMonth()
    {
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 3, 1));
        valueTuples.Count().Should().Be(3);
        valueTuples.First().dayCountOfMonth.Should().Be(31);
        valueTuples.Skip(1).First().dayCountOfMonth.Should().Be(28);
        valueTuples.Last().dayCountOfMonth.Should().Be(1);
    }

    [Test]
    public void GetDateCanHandleStartDate()
    {
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 15), new DateTime(2023, 3, 1));
        valueTuples.Count().Should().Be(3);
        valueTuples.First().dayCountOfMonth.Should().Be(17);
        valueTuples.Skip(1).First().dayCountOfMonth.Should().Be(28);
        valueTuples.Last().dayCountOfMonth.Should().Be(1);
    }

    [Test]
    public void QueryFullMonth()
    {
        GivenBudget(
            CreateBudget(2023, 11, 300));
        var result = _budgetService.Query(new DateTime(2023, 11, 1), new DateTime(2023, 11, 30));
        result.Should().Be(300);
    }

    private void GivenBudget(params Budget[] budget)
    {
        _budgetRepo.GetAll().Returns(budget.ToList());
    }

    private static Budget CreateBudget(int year, int month, int amount)
    {
        return new Budget()
        {
            YearMonth = $"{year:0000}{month:00}",
            Amount = amount
        };
    }

    [Test]
    public void QueryCrossYear()
    {
        GivenBudget(
            CreateBudget(2023, 12, 310),
            CreateBudget(2024, 01, 620));
        var result = _budgetService.Query(new DateTime(2023, 12, 1), new DateTime(2024, 1, 10));
        result.Should().Be(510);
    }

    [Test]
    public void QueryCrossMonthWithMissingBudge()
    {
        GivenBudget(
            CreateBudget(2024, 01, 620));
        var result = _budgetService.Query(new DateTime(2023, 12, 1), new DateTime(2024, 1, 10));
        result.Should().Be(200);
    }

    [Test]
    public void QueryInvalidDateRange()
    {
        GivenBudget(
            CreateBudget(2024, 01, 620));
        var result = _budgetService.Query(new DateTime(2024, 1, 10), new DateTime(2023, 12, 1));
        result.Should().Be(0);
    }
}