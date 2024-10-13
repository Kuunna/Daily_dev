namespace DailyDev.Models
{
    public class ItemTag
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int TagId { get; set; }

        // Navigation Properties
        public virtual Item Item { get; set; }
        public virtual Tag Tag { get; set; }
    }

}
