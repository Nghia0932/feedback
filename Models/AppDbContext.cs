using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<File> Files { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<File>().ToTable("Files");
    }
}


public class User
{
    
    public int id { get; set; }
    public required string username { get; set; }
    public required string password { get; set; }
    public required string email { get; set; }


}


public class File
{
    public int Id { get; set; }
    public required string TenFile { get; set; } // FileName in Vietnamese
    public required string LoaiFile { get; set; } // FileType in Vietnamese
    public required byte[] NoiDungFile { get; set; } // FileContent in Vietnamese
    public DateTime NgayTao { get; set; } // UploadDate in Vietnamese
}
