namespace DailyDev.Models
{
    public class TableConfig
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MostLiked { get; set; }
        public int MostRead { get; set; }
        public int MostTagged { get; set; }
        public int FavoriteCategory { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Category FavoriteCategoryNavigation { get; set; }
    }

}
