using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public class CustomerComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CommentsId { get; set; }
        public decimal? CustomerId { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class CustomerFilingComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CommentsId { get; set; }
        public decimal? FilingId { get; set; }
        public decimal? CustomerId { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
    public class CustomerFilingDraftComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CommentsId { get; set; }
        public decimal? DraftId { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
    public class CustomerFilingTrackingComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CommentsId { get; set; }
        public decimal? FileTrackingId { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
    public class CustomerFilingWorkflowComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CommentsId { get; set; }
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
