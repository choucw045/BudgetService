using FluentAssertions;

namespace BudgetService;

public class DateRangeHelperTests
{
    [Test]
    public void TestPeriod()
    {
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));
        valueTuples.Count().Should().Be(1);
        valueTuples.First().DayCount.Should().Be(1);
    }

    [Test]
    public void GetDateCanHandleCrossMonth()
    {
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 2, 1));
        valueTuples.Count().Should().Be(2);
        valueTuples.First().DayCount.Should().Be(31);
        valueTuples.Last().DayCount.Should().Be(1);
    }

    [Test]
    public void GetDateCanHandleCrossMultipleMonth()
    {
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 1), new DateTime(2023, 3, 1));
        valueTuples.Count().Should().Be(3);
        valueTuples.First().DayCount.Should().Be(31);
        valueTuples.Skip(1).First().DayCount.Should().Be(28);
        valueTuples.Last().DayCount.Should().Be(1);
    }

    [Test]
    public void GetDateCanHandleStartDate()
    {
        var valueTuples = DateRangeHelper.GetDate(new DateTime(2023, 1, 15), new DateTime(2023, 3, 1));
        valueTuples.Count().Should().Be(3);
        valueTuples.First().DayCount.Should().Be(17);
        
        
        valueTuples.Skip(1).First().DayCount.Should().Be(28);
        valueTuples.Last().DayCount.Should().Be(1);
    }
}