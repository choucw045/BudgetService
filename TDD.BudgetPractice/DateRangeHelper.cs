namespace BudgetService;

public class DateRangeHelper
{
    public DateRangeHelper()
    {
    }

    public static IEnumerable<MonthlyDayCount> SplitDaysCountByMonth(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new InvalidDateRangeException();
        }

        if (start.Year == end.Year && start.Month == end.Month)
        {
            var startDay = end.Day - start.Day + 1;
            yield return new MonthlyDayCount(start.Year, start.Month, startDay);
        }
        else
        {
            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            var dayCountOfMonth = daysInMonth - start.Day + 1;
            yield return new MonthlyDayCount(start.Year, start.Month, dayCountOfMonth);
            var current = start.AddMonths(1);
            while (new DateTime(current.Year, current.Month, 1) <= new DateTime(end.AddMonths(-1).Year, end.AddMonths(-1).Month, 1))
            {
                var inMonth = DateTime.DaysInMonth(current.Year, current.Month);
                yield return new MonthlyDayCount(current.Year, current.Month, inMonth);

                current = current.AddMonths(1);
            }

            var endDay = end.Day;
            yield return new MonthlyDayCount(end.Year, end.Month, endDay);
        }
    }
}

public class MonthlyDayCount
{
    public int Year { get; }
    public int Month { get; }
    public int DayCount { get; }

    public MonthlyDayCount(int year, int month, int dayCount)
    {
        Year = year;
        Month = month;
        DayCount = dayCount;
    }
}