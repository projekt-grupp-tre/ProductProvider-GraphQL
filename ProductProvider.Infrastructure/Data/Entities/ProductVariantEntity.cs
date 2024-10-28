using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Data.Entities;

public class ProductVariantEntity
{
    [Key]
    public Guid ProductVariantId { get; set; }

    [ForeignKey("ProductEntity")]
    public Guid ProductId { get; set; }

    public string Size { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int Stock { get; set; } 

    public decimal Price { get; set; } 

    public ProductEntity Product { get; set; } = null!;
}
