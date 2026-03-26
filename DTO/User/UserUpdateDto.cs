using System.ComponentModel.DataAnnotations;

namespace CRM.API.DTO.User
{
    public class UserUpdateDto
    {
        [StringLength(100)]
        public string FirstName { get; set; } = String.Empty;
        

        [StringLength(100)]
        public string LastName { get; set; } = String.Empty;
        

        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = String.Empty;
        

        [StringLength(250)]
        public string Address { get; set; } = String.Empty;
        


        [StringLength(30)]
        public string Phone { get; set; } = String.Empty;
    }
}
