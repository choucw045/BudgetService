namespace BudgetService;

public class DateRangeHelper
{
    public DateRangeHelper()
    {
    }

    public static IEnumerable<(int year, int month, int dayCountOfMonth)> GetDate(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new InvalidDateRangeException();
        }

        if (start.Year == end.Year && start.Month == end.Month)
        {
            var startDay = end.Day - start.Day;
            yield return (start.Year, start.Month, startDay + 1);
        }
        else
        {
            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            var dayCountOfMonth = daysInMonth - start.Day + 1;
            yield return (start.Year, start.Month, dayCountOfMonth);
            var current = start.AddMonths(1);
            while (new DateTime(current.Year,current.Month,1)  <= new DateTime(end.AddMonths(-1).Year, end.AddMonths(-1).Month,1))
            {
                var inMonth = DateTime.DaysInMonth(current.Year, current.Month);
                yield return (current.Year, current.Month, inMonth);

                current = current.AddMonths(1);
            }

            var endDay = end.Day;
            yield return (end.Year, end.Month, endDay);
        }
    }
}