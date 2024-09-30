namespace daily_dev.Models
{
    public class Dim_User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Store hashed password
        public DateTime JoinDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string Preferences { get; set; }
        public string Role { get; set; }
    }
}
