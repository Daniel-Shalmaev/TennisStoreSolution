using System.ComponentModel.DataAnnotations;

namespace TennisStoreSharedLibrary.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Foreign Key ל-UserAccount
        public int UserAccountId { get; set; }
        public UserAccount? UserAccount { get; set; } // Navigation Property
    }
}
