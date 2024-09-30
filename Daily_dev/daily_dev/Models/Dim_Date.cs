namespace daily_dev.Models
{
    public class Dim_Date
    {
        public int DateKey { get; set; }
        public DateTime Date { get; set; }
        public int Weekday { get; set; }
        public string DayName { get; set; }
        public int DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public int DayOfYear { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public string MonthYear { get; set; }
        public string MMYYYY { get; set; }
    }

}
