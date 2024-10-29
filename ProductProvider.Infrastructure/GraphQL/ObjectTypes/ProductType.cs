using HotChocolate.Types;
using ProductProvider.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.GraphQL.ObjectTypes;

using HotChocolate.Types;
using ProductProvider.Infrastructure.Data.Entities;

public class ProductType : ObjectType<ProductEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ProductEntity> descriptor)
    {
        descriptor.Field(p => p.ProductId).Type<NonNullType<IdType>>();
        descriptor.Field(p => p.Name).Type<NonNullType<StringType>>();
        descriptor.Field(p => p.Description).Type<StringType>();
        descriptor.Field(p => p.Images).Type<ListType<StringType>>();
        descriptor.Field(p => p.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(p => p.Category)
            .Type<CategoryType>()
            .Description("The category this product belongs to.");
        descriptor.Field(p => p.Variants)
            .Type<ListType<ProductVariantType>>()
            .Description("The variants available for this product.");
        descriptor.Field(p => p.Reviews)
            .Type<ListType<ReviewType>>()
            .Description("The reviews given for this product.");
    }
}

public class CategoryType : ObjectType<CategoryEntity>
{
    protected override void Configure(IObjectTypeDescriptor<CategoryEntity> descriptor)
    {
        descriptor.Field(c => c.CategoryId).Type<NonNullType<IdType>>();
        descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
        descriptor.Field(c => c.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(c => c.Products)
            .Type<ListType<ProductType>>()
            .Description("The products that belong to this category.");
    }
}

public class ProductVariantType : ObjectType<ProductVariantEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ProductVariantEntity> descriptor)
    {
        descriptor.Field(v => v.ProductVariantId).Type<NonNullType<IdType>>();
        descriptor.Field(v => v.Size).Type<NonNullType<StringType>>();
        descriptor.Field(v => v.Color).Type<NonNullType<StringType>>();
        descriptor.Field(v => v.Stock).Type<NonNullType<IntType>>();
        descriptor.Field(v => v.Price).Type<NonNullType<DecimalType>>();
        descriptor.Field(v => v.Product)
            .Type<ProductType>()
            .Description("The product this variant is linked to.");
    }
}

public class ReviewType : ObjectType<ReviewEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewEntity> descriptor)
    {
        descriptor.Field(r => r.ReviewId).Type<NonNullType<IdType>>();
        descriptor.Field(r => r.ClientName).Type<NonNullType<StringType>>();
        descriptor.Field(r => r.Rating).Type<NonNullType<IntType>>();
        descriptor.Field(r => r.Comment).Type<StringType>();
        descriptor.Field(r => r.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(r => r.Product)
            .Type<ProductType>()
            .Description("The product this review is linked to.");
    }
}
