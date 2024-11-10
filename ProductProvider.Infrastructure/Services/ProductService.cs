using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Contexts;
using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Infrastructure.Models;

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

    public async Task<ProductEntity?> UpdateProductWithReviewAndVariantHandlingAsync(Guid productId, UpdateProductInput updatedProductInput)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null) return null;

        product.Name = updatedProductInput.Name;
        product.Description = updatedProductInput.Description;
        product.Images = updatedProductInput.Images;

        if (!string.IsNullOrEmpty(updatedProductInput.CategoryName))
        {
            var category = await GetOrCreateCategoryByNameAsync(updatedProductInput.CategoryName);
            product.Category = category;
        }

        var updatedVariantIds = updatedProductInput.Variants.Select(v => v.ProductVariantId).ToHashSet();

        product.Variants = product.Variants.Where(v => updatedVariantIds.Contains(v.ProductVariantId)).ToList();

        foreach (var variantInput in updatedProductInput.Variants)
        {
            var existingVariant = product.Variants.FirstOrDefault(v => v.ProductVariantId == variantInput.ProductVariantId);
            if (existingVariant != null)
            {
                
                existingVariant.Size = variantInput.Size;
                existingVariant.Color = variantInput.Color;
                existingVariant.Stock = variantInput.Stock;
                existingVariant.Price = variantInput.Price;
            }
            else
            {
                product.Variants.Add(new ProductVariantEntity
                {
                    ProductId = product.ProductId,
                    ProductVariantId = Guid.NewGuid(),
                    Size = variantInput.Size,
                    Color = variantInput.Color,
                    Stock = variantInput.Stock,
                    Price = variantInput.Price,
                });
            }
        }
        var updatedReviewIds = updatedProductInput.Reviews.Select(r => r.ReviewId).ToHashSet();

        product.Reviews = product.Reviews.Where(r => updatedReviewIds.Contains(r.ReviewId)).ToList();

        foreach (var reviewInput in updatedProductInput.Reviews)
        {
            var existingReview = product.Reviews.FirstOrDefault(r => r.ReviewId == reviewInput.ReviewId);
            if (existingReview != null)
            {
                existingReview.ClientName = reviewInput.ClientName;
                existingReview.Rating = reviewInput.Rating;
                existingReview.Comment = reviewInput.Comment;
            }
            else
            {
                product.Reviews.Add(new ReviewEntity
                {
                    ProductId = product.ProductId,
                    ReviewId = reviewInput.ReviewId,
                    ClientName = reviewInput.ClientName,
                    Rating = reviewInput.Rating,
                    Comment = reviewInput.Comment,
                    CreatedAt = DateTime.UtcNow,
                });
            }
        }

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
        return _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
            .Include(p => p.Reviews)
            .AsNoTracking();
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

    public async Task<IEnumerable<CategoryEntity>> GetAllCategories()
    {
        var categoryList = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        if (categoryList is null)
            return Enumerable.Empty<CategoryEntity>();

        return categoryList;
    }

    public async Task<ReviewEntity?> AddReviewToProductAsync(Guid productId, ReviewEntity review)
    {
        var product = await _context.Products
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
        {
            return null; 
        }

        product.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return review;
    }
    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        if (review == null) return false;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteVariantAsync(Guid variantId)
    {
        var variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.ProductVariantId == variantId);
        if (variant == null) return false;

        _context.ProductVariants.Remove(variant);
        await _context.SaveChangesAsync();
        return true;
    }
}

