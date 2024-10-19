using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProvider.Infrastructure.Data.Entities;

public class ReviewEntity
{
    [Key]
    public int ReviewId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ClientName { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ProductEntity Product { get; set; }
}
