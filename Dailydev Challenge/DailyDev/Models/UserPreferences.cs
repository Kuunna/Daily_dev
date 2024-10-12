namespace DailyDev.Models
{
    public class UserPreferences
    {
        public List<int> Categories { get; set; } = new List<int>();
        public List<int> Tags { get; set; } = new List<int>();
    }
}
