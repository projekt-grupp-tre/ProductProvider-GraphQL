using HotChocolate;
using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
