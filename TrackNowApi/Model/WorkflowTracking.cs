using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class WorkflowTracking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal WorkflowTrackId { get; set; }
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
