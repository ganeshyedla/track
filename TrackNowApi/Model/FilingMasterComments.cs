using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{

    public partial class FilingMasterComments
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal? CommentsId { get; set; }
        public decimal? FilingId { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
    public class FilingMasterDraftComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal? CommentsId { get; set; }
        public decimal? DraftId { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class FilingMasterWorkflowComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal? CommentsId { get; set; }
        public decimal? WorkflowId { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

