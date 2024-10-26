using System.ComponentModel.DataAnnotations;

namespace TennisStoreSharedLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        // Relationship : One To Many
        public List<Product>? Products { get; set; }
    }
}
