using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Contexts;
using ProductProvider.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Services;

public class ProductService
{
    private readonly DataContext _context;

    public ProductService(DataContext context)
    {
        _context = context;
    }

    public async Task<ProductEntity> AddProductAsync(ProductEntity product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<ProductEntity?> UpdateProductAsync(ProductEntity updatedProduct)
    {
        var product = await _context.Products
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

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var product = await _context.Products.Include(p => p.Variants).Include(p => p.Reviews).FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ProductEntity?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.ProductId == productId);
    }
    public async Task<IEnumerable<ProductEntity>> GetProductsByIdsAsync(IEnumerable<Guid> productIds)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
            .Include(p => p.Reviews)
            .Where(p => productIds.Contains(p.ProductId))
            .ToListAsync();
    }
    public IQueryable<ProductEntity> GetAllProducts()
    {
        return _context.Products.Include(p => p.Category).Include(p => p.Variants).Include(p => p.Reviews);
    }
    public async Task<CategoryEntity> GetOrCreateCategoryByNameAsync(string categoryName)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
        if (category == null)
        {
            category = new CategoryEntity { Name = categoryName, CreatedAt = DateTime.UtcNow };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        return category;
    }
}

