using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Contexts;
using ProductProvider.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Services
{
    public class ProductService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;

        public ProductService(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ProductEntity> AddFullProductAsync(ProductEntity product)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductEntity?> UpdateFullProductAsync(ProductEntity updatedProduct)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == updatedProduct.ProductId);

            if (product == null) return null;

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Images = updatedProduct.Images;

            if (product.Category != null && updatedProduct.Category != null)
            {
                product.Category.Name = updatedProduct.Category.Name;
            }

            product.Variants.Clear();
            product.Variants = updatedProduct.Variants;

            product.Reviews.Clear();
            product.Reviews = updatedProduct.Reviews;

            await context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteFullProductAsync(Guid productId)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products.Include(p => p.Variants).Include(p => p.Reviews).FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null) return false;

            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductEntity?> GetProductByIdAsync(Guid productId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public IQueryable<ProductEntity> GetAllProducts()
        {
            var context = _contextFactory.CreateDbContext();
            return context.Products.Include(p => p.Category).Include(p => p.Variants).Include(p => p.Reviews);
        }
    }
}
