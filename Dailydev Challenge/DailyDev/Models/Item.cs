namespace DailyDev.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public DateTime PubDate { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public string Comments { get; set; }

        // Navigation Property
        public virtual Category Category { get; set; }
    }

}
