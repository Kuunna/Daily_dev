namespace DailyDev.Models
{
    /*public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
    }*/

    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public int? ProcessId { get; set; } 
        public string Status { get; set; } = "Not Started";  
        public DateTime? ProcessAt { get; set; }  
    }

}
