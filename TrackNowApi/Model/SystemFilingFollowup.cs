using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class SystemFilingFollowup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal FileTrackingId { get; set; }
        public decimal CustomerId { get; set; }
        public decimal FilingId { get; set; }
        public string? CommunicationType { get; set; }
        public string? CommText { get; set; }
        public string? AttachmentPath { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
    }
}
