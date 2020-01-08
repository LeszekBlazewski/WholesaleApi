using System.ComponentModel.DataAnnotations;

namespace Wholesale.BL.Models.Query
{
    public class LoginQuery
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
