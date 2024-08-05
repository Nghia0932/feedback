using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Product> Products { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Product>().ToTable("Product");
    }
}

public class User
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? passwordhash { get; set; }
    public string? email { get; set; }
    public string? role { get; set; }

}
public class Product
{
    public int Id { get; set; }
    public string? TenSanPham { get; set; }
    public int SoLuong { get; set; }
    public string? Description { get; set; } // Cột mới
}

