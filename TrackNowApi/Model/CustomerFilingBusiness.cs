using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class CustomerFilingBusiness
    {
        public decimal? CustomerId { get; set; }
        public decimal? FilingId { get; set; }
        public decimal? CategoryId { get; set; }
        public string? State { get; set; }
    }
}
