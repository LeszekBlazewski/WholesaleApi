﻿namespace Wholesale.BL.Models.Views
{
    public class CourierStatsV
    {
        public int CourierId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? NumberOfOrders { get; set; }
        public decimal? TotalWorth { get; set; }
    }
}
