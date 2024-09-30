namespace daily_dev.Models
{
    public class News_Tag
    {
        public int NewsID { get; set; }  // Referencing Fact_News.NewID
        public int TagID { get; set; }   // Referencing Dim_Tag.TagID
    }

}
