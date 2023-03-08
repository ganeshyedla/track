using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public class CustomerComments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CommentsID { get; set; }
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
        public decimal CommentsID { get; set; }
        public decimal? FilingID { get; set; }
        public decimal? CustomerID { get; set; }
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
        public decimal CommentsID { get; set; }
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
        public decimal CommentsID { get; set; }
        public decimal? FileTrackingID { get; set; }
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
        public decimal CommentsID { get; set; }
        public decimal? WorkflowID { get; set; }
        public string? CommentsText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
