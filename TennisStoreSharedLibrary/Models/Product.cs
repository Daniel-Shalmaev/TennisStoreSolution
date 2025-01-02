using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TennisStoreSharedLibrary.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Inventory { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? SubName { get; set; }

        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }


        [Required, Range(0.1, 99999.99)]
        public double Price { get; set; }

        [Range(0.1, 99999.99)]
        public double OldPrice { get; set; }

        [Required, DisplayName("Product Image")]
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }
        public string? Image6 { get; set; }
        public string? Image7 { get; set; }
        public string? Image8 { get; set; }

        public bool Visible { get; set; }


        [Required, Range(1, 99999)]
        public int Quantity { get; set; }

        public string? Badge { get; set; }

        [Range(1, 5)]
        [HalfStep]
        public double Rating { get; set; }
        public bool Featured { get; set; } = false;
        public DateTime DateUploaded { get; set; } = DateTime.Now;

        public List<ProductAttribute>? Attributes { get; set; }


        // Relationship : Many To One
        public Category? Category { get; set; }
        public int CategoryId { get; set; }

        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }

        public int? SubCategoryId { get; set; }
        public Subcategory? SubCategory { get; set; }

    }

    public class ProductAttribute
    {
        public int ProductId { get; set; }
        public int AttributeId { get; set; }

        [Required]
        public string Value { get; set; } = string.Empty;

        public Product? Product { get; set; }
        public Attribute? Attribute { get; set; }
    }


    public class HalfStepAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is double rating && rating >= 1 && rating <= 5 && rating % 0.5 == 0)
                return ValidationResult.Success!;

            return new ValidationResult("The rating must be between 1 and 5, in increments of 0.5.");
        }
    }
}
