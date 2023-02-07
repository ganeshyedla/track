using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class ReferenceDoc
    {
        public decimal FilingId { get; set; }
        public decimal DraftId { get; set; }
        public string? AttachmentPath { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public bool? DeletedFlag { get; set; }
    }
}
