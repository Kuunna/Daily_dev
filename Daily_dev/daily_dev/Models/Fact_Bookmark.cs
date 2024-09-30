namespace daily_dev.Models
{
    public class Fact_Bookmark
    {
        public int BookmarkID { get; set; }
        public int UserID { get; set; }  // Referencing Dim_User.UserID
        public int NewsID { get; set; }  // Referencing Fact_News.NewID
        public int BookmarkDate { get; set; }  // Referencing Dim_Time.TimeID
    }

}
