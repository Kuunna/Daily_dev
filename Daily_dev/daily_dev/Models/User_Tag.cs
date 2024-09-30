namespace daily_dev.Models
{
    public class User_Tag
    {
        public int UserID { get; set; }  // Referencing Dim_User.UserID
        public int TagID { get; set; }   // Referencing Dim_Tag.TagID
        public string InterestLevel { get; set; }
        public int FollowDate { get; set; }  // Referencing Dim_Time.TimeID
    }
}
