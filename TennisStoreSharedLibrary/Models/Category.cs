using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TennisStoreSharedLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [JsonIgnore]
        // Relationship : One To Many
        public List<Product>? Products { get; set; }
    }
}
