﻿using Wholesale.BL.Enums;

namespace Wholesale.BL.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public AddressDto Address { get; set; }
    }
}
