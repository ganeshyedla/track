using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public class FilingApprovalStatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal FilingApprovalId { get; set; }
        public decimal WorkflowId { get; set; }
        public string? ApproverName { get; set; }
        public string? AlternateApprovers { get; set; }
        public string? Comments { get; set; }
        public string? Attachments { get; set; }
        public string? Status { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? DoneBy { get; set; }
        public DateTime? DoneOn { get; set; }
    }
}
