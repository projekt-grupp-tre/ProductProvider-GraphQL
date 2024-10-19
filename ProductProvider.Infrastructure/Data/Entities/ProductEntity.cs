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
    public int ProductId { get; set; }

    [ForeignKey("CategoryEntity")]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public CategoryEntity Category { get; set; }

    public ICollection<ProductImageEntity> Images { get; set; }

    public ICollection<ReviewEntity> Reviews { get; set; }

    public ICollection<ProductToFiltersEntity> ProductFilters { get; set; }
}
