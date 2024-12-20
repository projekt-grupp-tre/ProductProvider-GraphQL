﻿using System;
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

    public List<string> Images { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual CategoryEntity? Category { get; set; }

    public virtual ICollection<ProductVariantEntity>? Variants { get; set; }

    public virtual ICollection<ReviewEntity>? Reviews { get; set; }
}

