using System;
using System.Globalization;

namespace FinanceBot
{
    public static class DateParser
    {
        public static bool TryParseDateRange(string dateRange, out DateTime startDate, out DateTime endDate)
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MaxValue;

            if (dateRange.Equals("today", StringComparison.OrdinalIgnoreCase))
            {
                startDate = DateTime.Today;
                endDate = DateTime.Today;
                return true;
            }
            if (dateRange.Equals("month", StringComparison.OrdinalIgnoreCase))
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate = startDate.AddMonths(1).AddDays(-1);
                return true;
            }
            if (dateRange.Equals("year", StringComparison.OrdinalIgnoreCase))
            {
                startDate = new DateTime(DateTime.Now.Year, 1, 1);
                endDate = new DateTime(DateTime.Now.Year, 12, 31);
                return true;
            }
            if (dateRange.Contains('-'))
            {
                var dates = dateRange.Split('-');
                if (dates.Length == 2 &&
                    DateTime.TryParseExact(dates[0].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var start) &&
                    DateTime.TryParseExact(dates[1].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var end))
                {
                    startDate = start;
                    endDate = end;
                    return true;
                }
            }

            return false;
        }
    }
}
