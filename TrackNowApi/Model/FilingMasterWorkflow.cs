using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class FilingMasterWorkflow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal WorkflowId { get; set; }
        public long CurrentApproverID { get; set; }
        public decimal? DraftId { get; set; }
        public string? WorkflowStatus { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
