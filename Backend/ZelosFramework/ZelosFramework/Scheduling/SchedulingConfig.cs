namespace ZelosFramework.Scheduling
{
    public class SchedulingConfig
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int DayOffset { get; set; }
        public IntervalType IntervalType { get; set; }
    }

    public enum IntervalType
    {
        None,
        Minute,
        Hour,
        Day,
        Week,
        Month        
    }
}