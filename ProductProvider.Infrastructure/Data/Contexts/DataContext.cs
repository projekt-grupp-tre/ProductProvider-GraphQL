using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Entities;



namespace ProductProvider.Infrastructure.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<ProductFilterEntity> ProductFilters { get; set; }
    public DbSet<ProductToFiltersEntity> ProductToFilters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductToFiltersEntity>().HasKey(ptf => new { ptf.ProductId, ptf.FilterId });

        modelBuilder.Entity<CategoryEntity>().HasKey(c => c.CategoryId);

        modelBuilder.Entity<ProductEntity>().HasKey(p => p.ProductId);
        modelBuilder.Entity<ProductEntity>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
        modelBuilder.Entity<ProductEntity>().HasMany(p => p.Images).WithOne(i => i.Product).HasForeignKey(i => i.ProductId);
        modelBuilder.Entity<ProductEntity>().HasMany(p => p.Reviews).WithOne(r => r.Product).HasForeignKey(r => r.ProductId);
        modelBuilder.Entity<ProductEntity>().HasMany(p => p.ProductFilters).WithOne(ptf => ptf.Product).HasForeignKey(ptf => ptf.ProductId);

        modelBuilder.Entity<ProductFilterEntity>().HasMany(pf => pf.ProductLinks).WithOne(ptf => ptf.Filter).HasForeignKey(ptf => ptf.FilterId);

        base.OnModelCreating(modelBuilder);
    }
}

//tes

