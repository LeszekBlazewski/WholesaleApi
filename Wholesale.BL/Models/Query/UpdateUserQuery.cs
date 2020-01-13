using Wholesale.BL.Enums;
using Wholesale.BL.Models.Dto;

namespace Wholesale.BL.Models.Query
{
    public class UpdateUserQuery
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public AddressDto Address { get; set; }
    }
}
