using HotChocolate;
using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Infrastructure.Models;
using ProductProvider.Infrastructure.Services;


namespace ProductProvider.Infrastructure.GraphQL.Mutations;

public class ProductMutation
{
    public async Task<ProductEntity> AddFullProductAsync(AddProductInput input, ProductService productService)
    {
        // Skapa ny produkt med alla dess egenskaper
        var product = new ProductEntity
        {
            Name = input.Name,
            Description = input.Description,
            Images = input.Images,
            Category = new CategoryEntity { Name = input.CategoryName, CreatedAt = DateTime.UtcNow },
            CreatedAt = DateTime.UtcNow,
            Variants = input.Variants.Select(v => new ProductVariantEntity
            {
                Size = v.Size,
                Color = v.Color,
                Stock = v.Stock,
                Price = v.Price
            }).ToList(),
            Reviews = input.Reviews.Select(r => new ReviewEntity
            {
                ClientName = r.ClientName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = DateTime.UtcNow
            }).ToList()
        };

        return await productService.AddFullProductAsync(product);
    }

    public async Task<bool> DeleteFullProductAsync(Guid productId, ProductService productService)
    {
        return await productService.DeleteFullProductAsync(productId);
    }

    public async Task<ProductEntity?> UpdateFullProductAsync(Guid productId, AddProductInput input, ProductService productService)
    {
        var product = new ProductEntity
        {
            ProductId = productId,
            Name = input.Name,
            Description = input.Description,
            Images = input.Images,
            Category = new CategoryEntity { Name = input.CategoryName, CreatedAt = DateTime.UtcNow },
            Variants = input.Variants.Select(v => new ProductVariantEntity
            {
                Size = v.Size,
                Color = v.Color,
                Stock = v.Stock,
                Price = v.Price
            }).ToList(),
            Reviews = input.Reviews.Select(r => new ReviewEntity
            {
                ClientName = r.ClientName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = DateTime.UtcNow
            }).ToList()
        };

        return await productService.UpdateFullProductAsync(product);
    }
}
