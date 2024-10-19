using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Data.Entities;

public class CategoryEntity
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ProductEntity> Products { get; set; }
}
