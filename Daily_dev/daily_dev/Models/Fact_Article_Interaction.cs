namespace daily_dev.Models
{
    public class Fact_Article_Interaction
    {
        public int InteractionID { get; set; }
        public int UserID { get; set; }  // Referencing Dim_User.UserID
        public int NewsID { get; set; }  // Referencing Fact_News.NewID
        public string InteractionType { get; set; }  // E.g., "Like", "Bookmark", "Comment"
        public int InteractionDate { get; set; }  // Referencing Dim_Time.TimeID
        public string CommentText { get; set; }
        public int UpvoteCount { get; set; }
    }

}
