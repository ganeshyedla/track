﻿using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class CustomerFileTracking
    {
        public decimal? FileTrackingId { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? FilingId { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
