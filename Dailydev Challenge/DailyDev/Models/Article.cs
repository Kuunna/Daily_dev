namespace DailyDev.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime PublishedDate { get; set; }
        public int SourceId { get; set; }
    }
}
