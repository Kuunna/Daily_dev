namespace daily_dev.Models
{
    public class Dim_Source
    {
        public int SourceID { get; set; }
        public string SourceName { get; set; }
        public string RSS_URL { get; set; }
        public int LastUpdated { get; set; }  // Referencing Dim_Time.TimeID
        public int ArticleCount { get; set; }
        public bool IsActive { get; set; }
    }

}
