using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class AppConfiguration
    {
        public long ConfigId { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? FilingId { get; set; }
        public string? ConfigItem { get; set; }
        public string? ConfigItemValue { get; set; }
        public string? ApproverLevel { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
