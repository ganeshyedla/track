using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class WorkflowTracking
    {
        public decimal? WorkflowId { get; set; }
        public string? Comments { get; set; }
        public string? Attachments { get; set; }
        public string? Action { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
