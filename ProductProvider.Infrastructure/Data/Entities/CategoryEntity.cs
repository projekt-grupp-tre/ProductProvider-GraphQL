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

    public string Name { get; set; } = null!;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ProductEntity>? Products { get; set; }
}
