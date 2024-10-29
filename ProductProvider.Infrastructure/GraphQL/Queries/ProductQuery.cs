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
        public IQueryable<ProductEntity> GetProducts(ProductService productService)
        {
            return productService.GetAllProducts();
        }

        public async Task<ProductEntity?> GetProductById(Guid productId, ProductService productService)
        {
            return await productService.GetProductByIdAsync(productId);
        }
    }
}
