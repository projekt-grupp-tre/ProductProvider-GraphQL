using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductProvider.Infrastructure.Data.Contexts;
using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ProductProvider.Tests.UnitTests;

public class ProductServiceTests
{
    private readonly DbContextOptions<DataContext> _options;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "ProductDatabase")
            .Options;

        using (var context = new DataContext(_options))
        {
            SeedDatabase(context);
        }

        var contextForService = new DataContext(_options);
        _productService = new ProductService(contextForService);
    }

    private void SeedDatabase(DataContext context)
    {
        var category = new CategoryEntity
        {
            CategoryId = 1,
            Name = "Test Category 1"
        };

        context.Categories.Add(category);

        context.Products.AddRange(
            new ProductEntity
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product 1",
                Description = "Test Description 1",
                Images = new List<string> { "image1.jpg" },
                Category = category,
                CreatedAt = DateTime.UtcNow,
                Variants = new List<ProductVariantEntity>
                {
                        new ProductVariantEntity
                        {
                            ProductVariantId = Guid.NewGuid(),
                            Size = "M",
                            Color = "Red",
                            Stock = 10,
                            Price = 99.99M
                        }
                },
                Reviews = new List<ReviewEntity>
                {
                        new ReviewEntity
                        {
                            ReviewId = 1,
                            ClientName = "John Doe",
                            Rating = 4,
                            Comment = "Great product!",
                            CreatedAt = DateTime.UtcNow
                        }
                }
            },
            new ProductEntity
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product 2",
                Description = "Test Description 2",
                Images = new List<string> { "image2.jpg" },
                Category = category,
                CreatedAt = DateTime.UtcNow,
                Variants = new List<ProductVariantEntity>
                {
                        new ProductVariantEntity
                        {
                            ProductVariantId = Guid.NewGuid(),
                            Size = "L",
                            Color = "Blue",
                            Stock = 5,
                            Price = 149.99M
                        }
                },
                Reviews = new List<ReviewEntity>
                {
                        new ReviewEntity
                        {
                            ReviewId = 2,
                            ClientName = "Jane Doe",
                            Rating = 5,
                            Comment = "Excellent product!",
                            CreatedAt = DateTime.UtcNow
                        }
                }
            }
        );
        context.SaveChanges();
    }

    private void CleanUpDatabase()
    {
        using (var context = new DataContext(_options))
        {
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);
            context.ProductVariants.RemoveRange(context.ProductVariants);
            context.Reviews.RemoveRange(context.Reviews);
            context.SaveChanges();
        }
    }

    [Fact]
    public async Task Test_CreateAndRetrieveProduct_Returns_CorrectAttributes()
    {
        try
        {
            // Arrange
            var newProduct = new ProductEntity
            {
                ProductId = Guid.NewGuid(),
                Name = "New Test Product",
                Description = "New Test Description",
                Images = new List<string> { "newImage.jpg" },
                CreatedAt = DateTime.UtcNow,
                Variants = new List<ProductVariantEntity>
                    {
                        new ProductVariantEntity
                        {
                            ProductVariantId = Guid.NewGuid(),
                            Size = "S",
                            Color = "Green",
                            Stock = 20,
                            Price = 79.99M
                        }
                    },
                Reviews = new List<ReviewEntity>
                    {
                        new ReviewEntity
                        {
                            ReviewId = 3,
                            ClientName = "Alice Smith",
                            Rating = 3,
                            Comment = "Good but could be better.",
                            CreatedAt = DateTime.UtcNow
                        }
                    }
            };

            using (var context = new DataContext(_options))
            {
                context.Products.Add(newProduct);
                context.SaveChanges();
            }

            // Act
            ProductEntity? retrievedProduct;
            using (var context = new DataContext(_options))
            {
                retrievedProduct = await context.Products
                    .Include(p => p.Variants)
                    .Include(p => p.Reviews)
                    .FirstOrDefaultAsync(p => p.ProductId == newProduct.ProductId);
            }

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.Equal(newProduct.Name, retrievedProduct?.Name);
            Assert.Equal(newProduct.Description, retrievedProduct?.Description);
            Assert.NotNull(retrievedProduct?.Images);
            Assert.Equal(newProduct.Images, retrievedProduct?.Images);
            Assert.Equal(newProduct.CreatedAt, retrievedProduct?.CreatedAt);
            Assert.NotNull(retrievedProduct?.Variants);
            Assert.Single(retrievedProduct?.Variants);
            Assert.Equal(newProduct.Variants.First().Size, retrievedProduct?.Variants.First().Size);
            Assert.Equal(newProduct.Variants.First().Color, retrievedProduct?.Variants.First().Color);
            Assert.Equal(newProduct.Variants.First().Stock, retrievedProduct?.Variants.First().Stock);
            Assert.Equal(newProduct.Variants.First().Price, retrievedProduct?.Variants.First().Price);
            Assert.NotNull(retrievedProduct?.Reviews);
            Assert.Single(retrievedProduct?.Reviews);
            Assert.Equal(newProduct.Reviews.First().ClientName, retrievedProduct?.Reviews.First().ClientName);
            Assert.Equal(newProduct.Reviews.First().Rating, retrievedProduct?.Reviews.First().Rating);
            Assert.Equal(newProduct.Reviews.First().Comment, retrievedProduct?.Reviews.First().Comment);
            Assert.Equal(newProduct.Reviews.First().CreatedAt, retrievedProduct?.Reviews.First().CreatedAt);
        }
        finally
        {
            CleanUpDatabase();
        }
    }

    [Fact]
    public async Task Test_GetAllProducts_Returns_ProductsInJsonFormat()
    {
        try
        {
            // Act
            var products = _productService.GetAllProducts();

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
            Assert.All(products, product =>
            {
                Assert.False(string.IsNullOrEmpty(product.Name));
                Assert.False(string.IsNullOrEmpty(product.Description));
                Assert.NotNull(product.Images);
                Assert.NotEmpty(product.Images);
                Assert.NotNull(product.Variants);
                Assert.NotEmpty(product.Variants);
                Assert.NotNull(product.Reviews);
                Assert.NotEmpty(product.Reviews);
            });
        }
        finally
        {
            CleanUpDatabase();
        }
    }

    [Fact]
    public async Task Test_ProductCategoryRelation_IsCorrect()
    {
        try
        {
            // Act
            ProductEntity? product;
            using (var context = new DataContext(_options))
            {
                product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Name == "Test Product 1");
            }

            // Assert
            Assert.NotNull(product);
            Assert.NotNull(product?.Category);
            Assert.Equal("Test Category 1", product?.Category?.Name);
        }
        finally
        {
            CleanUpDatabase();
        }
    }
    [Fact]
    public async Task Test_AddProduct_WithMissingInformation_ThrowsValidationError()
    {
        try
        {
            // Arrange
            var incompleteProduct = new ProductEntity
            {
                ProductId = Guid.NewGuid(),
                Description = "Missing Name and Images",
                CreatedAt = DateTime.UtcNow
            };

            // Act & Assert
            using (var context = new DataContext(_options))
            {
                context.Products.Add(incompleteProduct);
                await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
            }
        }
        finally
        {
            CleanUpDatabase();
        }

    }    
}
