using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace SievalAPI.Models
{
    public class TProduct
    {
        private string? _sku;
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //.Compute mag niet omdat het veld de primary key is.
        //.Identity werkt alleen niet om invoer te negeren; value wordt wel toegekend en dat mag
        //bij create het ID veld in json weglaten zorgt voor wel voor juiste NewID() afhandeling in SQL-server
        public Guid ID { get; set; }

        [Required]
        [StringLength(20)]
        public string? SKU      //{ get; set; }
        {     // experimentje: SKU zonder spaties
            get { return _sku; }
            set 
            {  
                _sku = (string.IsNullOrEmpty(value)) 
                        ? Guid.NewGuid().ToString().Substring(0, 18) 
                        : value.Trim().Replace(' ', '-'); 
            }
        } 

        [StringLength(100)]
        public string? Name { get; set; }

        [Precision(18, 2)]
        public decimal? Price { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]   
        //eigen invoer values worden hiermee keurig genegeerd door EF
        public DateTime? ChangeDate { get; set;  }

    }

    class TSievalDB : DbContext
    {
        public TSievalDB(DbContextOptions options) : base(options) { }
        public DbSet<TProduct> Products { get; set; } = null!;

        //EF7+ nieuwe update techniek kan niet meer met triggers overweg (zucht)
        //zie https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes
        //dus EF7 weten welke tables triggers hebben
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TProduct>()
                .ToTable(tb => tb.HasTrigger("TR_Products_U"));
        }

    }

}
