using System.Collections.Generic;
using Wholesale.BL.Enums;

namespace Wholesale.BL.Models
{
    public class User
    {
        public User() { }

        public int? UserId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Order> OrdersClient { get; set; }
        public virtual ICollection<Order> OrdersCourier { get; set; }
    }
}
