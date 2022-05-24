using System;
using System.Collections.Generic;
using System.Threading;

namespace ZelosFramework.Scheduling
{
    class SchedulerService
    {
        private static SchedulerService _instance;
        private List<Timer> timers = new List<Timer>();

        private SchedulerService() { }

        public static SchedulerService Instance => _instance ?? (_instance = new SchedulerService());

        public void ScheduleTask(SchedulingConfig config, Action task)
        {
            TimeSpan timeToGo = CalculateTimeUntilFirstRun(config);
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            TimeSpan periodTimeSpan = CaclulatePeriodTimeSpan(config);

            var timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, periodTimeSpan);

            timers.Add(timer);
        }

        private static TimeSpan CaclulatePeriodTimeSpan(SchedulingConfig config)
        {
            switch (config.IntervalType)
            {
                case IntervalType.Minute:
                    return TimeSpan.FromMinutes(1);
                case IntervalType.Hour:
                    return TimeSpan.FromHours(1);
                case IntervalType.Day:
                    return TimeSpan.FromDays(1);
                case IntervalType.Week:
                    return TimeSpanExtension.FromWeeks(1);
                case IntervalType.Month:
                    return TimeSpanExtension.FromMonths(1);
                default: throw new InvalidOperationException("Unknown intervall type");
            }
        }

        private static TimeSpan CalculateTimeUntilFirstRun(SchedulingConfig config)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun;
            switch (config.IntervalType)
            {
                case IntervalType.Minute:
                case IntervalType.Hour:
                case IntervalType.Day:
                    {
                        firstRun = new DateTime(now.Year, now.Month, now.Day, config.Hour, config.Minute, 0, 0);
                        if (now > firstRun)
                        {
                            firstRun = firstRun.AddDays(1);
                        }
                        break;
                    }
                case IntervalType.Week:
                    {
                        int dayOffset = config.DayOffset - (int)now.DayOfWeek;
                        if (dayOffset <= 0)
                        {
                            dayOffset = dayOffset + 7;
                        }
                        firstRun = new DateTime(now.Year, now.Month, now.AddDays(dayOffset).Day, config.Hour, config.Minute, 0, 0);
                        break;
                    }
                case IntervalType.Month:
                    {
                        int dayOffset = config.DayOffset - (int)now.DayOfWeek;
                        if (dayOffset <= 0)
                        {
                            dayOffset = dayOffset + DateTime.DaysInMonth(now.Year, now.Month);
                        }
                        firstRun = new DateTime(now.Year, now.Month, now.AddDays(dayOffset).Day, config.Hour, config.Minute, 0, 0);
                        break;
                    }
                default: throw new InvalidOperationException("Unknown intervall type");
            }

            TimeSpan timeToGo = firstRun - now;
            return timeToGo;
        }
    }
}
