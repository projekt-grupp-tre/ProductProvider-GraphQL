using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Data.Entities;

public class ProductImageEntity
{
    [Key]
    public int ImageId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ImageUrl { get; set; }

    [MaxLength(255)]
    public string AltText { get; set; }

    public ProductEntity Product { get; set; }
}
