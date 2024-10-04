namespace DailyDev.Models
{
    public class UserTag
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TagId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Tag Tag { get; set; }
    }

}
