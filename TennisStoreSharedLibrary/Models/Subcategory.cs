using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TennisStoreSharedLibrary.Models
{
    public class Subcategory
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        // Relationship: Many-to-One with Category
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
