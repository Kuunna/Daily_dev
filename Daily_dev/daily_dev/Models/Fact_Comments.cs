namespace daily_dev.Models
{
    public class Fact_Comments
    {
        public int CommentID { get; set; }
        public int UserID { get; set; }  // Referencing Dim_User.UserID
        public int NewsID { get; set; }  // Referencing Fact_News.NewID
        public string CommentText { get; set; }
        public int CommentDate { get; set; }  // Referencing Dim_Time.TimeID
    }

}
