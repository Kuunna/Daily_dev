namespace DailyDev.Models
{
    public class NewTag
    {
        public int Id { get; set; }
        public int NewId { get; set; }
        public int TagId { get; set; }

        // Navigation Properties
        public virtual Item New { get; set; }
        public virtual Tag Tag { get; set; }
    }

}
