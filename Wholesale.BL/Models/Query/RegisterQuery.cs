using System.ComponentModel.DataAnnotations;
using Wholesale.BL.Enums;
using Wholesale.BL.Models.Dto;

namespace Wholesale.BL.Models.Query
{
    public class RegisterQuery
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }

        [Required]
        public AddressDto Address { get; set; }
    }
}
