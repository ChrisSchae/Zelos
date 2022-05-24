using System;

namespace ZelosFramework.Scheduling
{
    public static class TimeSpanExtension
    {
        public static TimeSpan FromMonths(double value)
        {
            // ToDo: Inacurate needs to get fixed
            return TimeSpan.FromDays(value * 30);
        }

        public static TimeSpan FromWeeks(double value)
        {
            return TimeSpan.FromDays(value * 7);
        }
    }
}
