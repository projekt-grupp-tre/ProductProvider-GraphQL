using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Data.Entities;

public class ProductFilterEntity
{
    [Key]
    public int FilterId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string FilterType { get; set; }

    [MaxLength(255)]
    public string Value { get; set; }

    public ICollection<ProductToFiltersEntity> ProductLinks { get; set; }
}
