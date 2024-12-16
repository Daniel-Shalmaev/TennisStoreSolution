using System.ComponentModel.DataAnnotations;

namespace TennisStoreSharedLibrary.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public Customer? Customer { get; set; } // Navigation Property
    }
}
