using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Entities;



namespace ProductProvider.Infrastructure.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<ProductVariantEntity> ProductVariants { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
     
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<CategoryEntity>().HasKey(c => c.CategoryId);
        modelBuilder.Entity<ProductEntity>().HasKey(p => p.ProductId);
        modelBuilder.Entity<ReviewEntity>().HasKey(p => p.ReviewId);
        modelBuilder.Entity<ProductVariantEntity>().HasKey(p => p.ProductVariantId);
        modelBuilder.Entity<ProductEntity>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
        modelBuilder.Entity<ProductEntity>().HasMany(p => p.Reviews).WithOne(r => r.Product).HasForeignKey(r => r.ProductId);

        base.OnModelCreating(modelBuilder);
    }
}


