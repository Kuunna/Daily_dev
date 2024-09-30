namespace daily_dev.Models
{
    public class User_Source
    {
        public int UserID { get; set; }  // Referencing Dim_User.UserID
        public int SourceID { get; set; }  // Referencing Dim_Source.SourceID
        public DateTime FollowDate { get; set; }
    }
}
