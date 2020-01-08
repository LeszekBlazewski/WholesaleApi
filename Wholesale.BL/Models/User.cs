using System.Collections.Generic;
using Wholesale.BL.Enums;

namespace Wholesale.BL.Models
{
    public class User
    {
        public User()
        {
            Deliveries = new HashSet<Delivery>();
        }

        public int? UserId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }

        public Address Address { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
    }
}
