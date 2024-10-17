namespace DailyDev.Models
{
    public class ItemComment
    {
        public int Id { get; set; }         
        public int UserId { get; set; }      
        public int ItemId { get; set; }      
        public string Content { get; set; }  
        public int? ParentId { get; set; }   
        public DateTime CreateAt { get; set; } 
    }

}
