using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Data.Entities;

public class ProductToFiltersEntity
{
    [Key]
    public int ProductId { get; set; }

    [Key]
    public int FilterId { get; set; }

    public ProductEntity Product { get; set; }
    public ProductFilterEntity Filter { get; set; }
}
