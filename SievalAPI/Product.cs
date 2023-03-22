using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SievalAPI.Models
{
    public class Product
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(20)]
        public string? SKU { get; set; } 

        [StringLength(100)]
        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public DateTime? ChangeDate { get; set;  }
    }

    class SievalDB : DbContext
    {
        public SievalDB(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products { get; set; } = null!;
    }

}
