using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class CustomerFilingMasterHistory
    {
        public decimal CustomerId { get; set; }
        public decimal FilingId { get; set; }
        public string? Notes { get; set; }
        public string? Dboperations { get; set; }
        public string? Source { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
