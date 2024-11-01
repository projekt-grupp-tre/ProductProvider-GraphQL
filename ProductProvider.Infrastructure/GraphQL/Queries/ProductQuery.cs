using HotChocolate;
using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Infrastructure.Services;

namespace ProductProvider.Infrastructure.GraphQL.Queries
{
    public class ProductQuery
    {
        [GraphQLName("getProducts")]
        public IQueryable<ProductEntity> GetProducts([Service] ProductService productService)
        {
            return productService.GetAllProducts();
        }
        [GraphQLName("getProductById")]
        public async Task<ProductEntity?> GetProductById(Guid productId, [Service] ProductService productService)
        {
            return await productService.GetProductByIdAsync(productId);
        }
        [GraphQLName("getProductsByCategory")]
        public IQueryable<ProductEntity> GetProductsByCategory(string categoryName, [Service] ProductService productService)
        {
            return productService.GetAllProducts().Where(p => p.Category != null && p.Category.Name == categoryName);
        }
        [GraphQLName("getProductsByIds")]
        public async Task<IEnumerable<ProductEntity>> GetProductsByIds(IEnumerable<Guid> productIds, [Service] ProductService productService)
        {
            var products = await productService.GetProductsByIdsAsync(productIds);
            return products;
        }

        [GraphQLName("getCategories")]
        public async Task<IEnumerable<CategoryEntity>> GetCategories([Service] ProductService productService)
        {
            return await productService.GetAllCategories();
        }
    }
}
