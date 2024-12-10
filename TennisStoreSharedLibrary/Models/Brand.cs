using System.ComponentModel.DataAnnotations;

namespace TennisStoreSharedLibrary.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        public string? HebName { get; set; }

        [Required]
        public string? EnName { get; set; }

        [Required]
        public int ProductsCount{ get; set; }

        [Required]
        public string? LogoPath { get; set; }

        // Relationships
        public List<Product>? Products { get; set; }

    }
}
