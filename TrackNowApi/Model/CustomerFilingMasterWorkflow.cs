﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public class CustomerFilingMasterWorkflow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal WorkflowId { get; set; }
        public decimal WorkflowInitiatorId { get; set; }
        public long CurrentApproverId { get; set; }
        public decimal? DraftId { get; set; }
        public string? WorkflowStatus { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? Notes { get; set; }
    }
}
