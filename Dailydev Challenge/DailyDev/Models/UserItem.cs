namespace DailyDev.Models
{
    public class UserItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public bool IsLiked { get; set; } = false;        
        public bool IsBookmarked { get; set; } = false;   
        public DateTime? LikeDate { get; set; }           
        public DateTime? BookmarkDate { get; set; }    
    }
}
