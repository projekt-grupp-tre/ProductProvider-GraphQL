using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Data.Entities;

public class ProductEntity
{
    [Key]
    public Guid ProductId { get; set; }

    [ForeignKey("CategoryEntity")]
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<(string Url, string Alt)> Images { get; set; } = new List<(string Url, string Alt)>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public CategoryEntity? Category { get; set; }

    public ICollection<ProductVariantEntity>? Variants { get; set; }

    public ICollection<ReviewEntity>? Reviews { get; set; }
}


//public class ProductEntity
//{
//    [Key]
//    public Guid ProductId { get; set; }

//    [ForeignKey("CategoryEntity")]
//    public int CategoryId { get; set; }

//    public string Name { get; set; } = null!;


//    public string Description { get; set; } = null!;


//    public decimal Price { get; set; }
//    public List<(string Url, string Alt)> Images { get; set; } = new List<(string Url, string Alt)>();

//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//    public CategoryEntity? Category { get; set; }


//    public ICollection<ReviewEntity>? Reviews { get; set; }


//}
