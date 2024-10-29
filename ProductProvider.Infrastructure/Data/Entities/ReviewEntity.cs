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
    public Guid ProductId { get; set; }

    public string ClientName { get; set; } = null!;

    //[Range(1, 5)]
    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ProductEntity Product { get; set; } = null!;
}
