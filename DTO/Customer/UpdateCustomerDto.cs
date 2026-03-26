using System.ComponentModel.DataAnnotations;

namespace CRM.API.DTO.Customer
{
    public class UpdateCustomerDto
    {
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        [StringLength(30)]
        public string Phone { get; set; } = string.Empty;
        [StringLength(150)]
        public string CompanyName { get; set; } = string.Empty;
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;
    }
}
