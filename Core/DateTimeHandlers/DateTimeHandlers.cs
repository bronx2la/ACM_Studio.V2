using System;

namespace Core.DateTimeHandlers
{
    public static class DateTimeHandlers
    {
        public static DateTime GetQuarterEndDateFromYearQuarter(string value)
        {
            int mon = 0;
            int day = 0;
            int year = Convert.ToInt32(value.Substring(0, 4));

            switch (value.Substring(5, 2))
            {
                case "Q1":
                    mon = 3;
                    day = 31;
                    break;
                case "Q2":
                    mon = 6;
                    day = 30;
                    break;
                case "Q3":
                    mon = 9;
                    day = 30;
                    break;
                case "Q4":
                    mon = 12;
                    day = 31;
                    break;
            }
            return new DateTime(year, mon, day);
        }
    }
}