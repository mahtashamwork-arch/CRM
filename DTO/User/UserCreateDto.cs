using System.ComponentModel.DataAnnotations;

namespace CRM.API.DTO.User
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = String.Empty;
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = String.Empty;
        [Required]
        [StringLength(250)]
        public string Address { get; set; } = String.Empty;
        [Required]
        [StringLength(30)]
        public string Phone { get; set; } = String.Empty;
    }
}
