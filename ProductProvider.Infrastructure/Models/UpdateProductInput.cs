using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Models;

public class UpdateProductInput
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new List<string>();
    public string CategoryName { get; set; } = string.Empty;
    public List<UpdateVariantInput> Variants { get; set; } = new List<UpdateVariantInput>();
    public List<UpdateReviewInput> Reviews { get; set; } = new List<UpdateReviewInput>();
}

public class UpdateVariantInput
{
    public Guid ProductVariantId { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal Price { get; set; }
}

public class UpdateReviewInput
{
    public int ReviewId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}