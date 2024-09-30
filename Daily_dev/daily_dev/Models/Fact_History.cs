namespace daily_dev.Models
{
    public class Fact_History
    {
        public int HistoryID { get; set; }
        public int UserID { get; set; }  // Referencing Dim_User.UserID
        public int NewsID { get; set; }  // Referencing Fact_News.NewID
        public int ReadDate { get; set; }  // Referencing Dim_Time.TimeID
        public int ReadDuration { get; set; }  // Duration in seconds
    }

}
