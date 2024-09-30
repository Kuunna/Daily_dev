namespace daily_dev.Models
{
    public class Fact_News
    {
        public int NewID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int PublishedDate { get; set; }
        public int SourceID { get; set; }
        public int TopicID { get; set; }
        public string Author { get; set; }
        public string ImageURL { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }

}
