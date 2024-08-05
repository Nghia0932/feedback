using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> User { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");

    }
}

public class User
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? passwordhash { get; set; }
    public string? email { get; set; }

}


