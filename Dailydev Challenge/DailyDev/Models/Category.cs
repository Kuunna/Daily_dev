namespace DailyDev.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProviderId { get; set; }
        public string Source { get; set; }
        public int Ttl { get; set; }
        public string Generator { get; set; }
        public string Docs { get; set; }

        // Navigation Property
        public virtual Provider Provider { get; set; }
    }
}
