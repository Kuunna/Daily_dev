namespace DailyDev.Models
{
    public class UserCategory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
    }

}
