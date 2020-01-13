using System;
using System.ComponentModel.DataAnnotations;

namespace Wholesale.BL.Models.Query
{
    public class OrderTotalWorthQuery
    {
        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }
    }
}
