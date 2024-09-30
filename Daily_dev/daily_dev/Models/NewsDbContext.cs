using daily_dev.Models;
using Microsoft.EntityFrameworkCore;

public class NewsDbContext : DbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
    }

    public DbSet<Fact_News> Fact_News { get; set; }
    public DbSet<Dim_Source> Dim_Source { get; set; }
    public DbSet<Dim_Tag> Dim_Tag { get; set; }
    public DbSet<Dim_User> Dim_User { get; set; }
    public DbSet<Fact_Article_Interaction> Fact_Article_Interaction { get; set; }
    public DbSet<Fact_Comments> Fact_Comments { get; set; }
    public DbSet<Fact_Bookmark> Fact_Bookmark { get; set; }
    public DbSet<Fact_History> Fact_History { get; set; }
    public DbSet<Dim_Category> Dim_Category { get; set; }
    public DbSet<Dim_Date> Dim_Date { get; set; }
    public DbSet<News_Tag> News_Tag { get; set; }
    public DbSet<User_Source> User_Source { get; set; }
    public DbSet<User_Tag> User_Tag { get; set; }

}


