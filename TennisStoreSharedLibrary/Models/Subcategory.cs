using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TennisStoreSharedLibrary.Models
{
    public class Subcategory
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int Stock { get; set; } // Stock count for the subcategory

        // Relationship: Many-to-One with Category
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
