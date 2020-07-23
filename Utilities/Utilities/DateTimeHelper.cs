using System;

namespace ista.Utilities
{
    public static class DateTimeHelper
    {
        public static string DateForIsecString(this DateTime date)
        {
            return string.Concat(date.Day.ToString("00"), "/", date.Month.ToString("00"), "/", date.Year.ToString("0000"));
        }

        public static string FileNameWithDate(this DateTime date, string fileName)
        {
            return string.Concat(date.Year.ToString("0000"), date.Month.ToString("00"), date.Day.ToString("00"), "-",
                fileName, ".txt");
        }
    }
}
