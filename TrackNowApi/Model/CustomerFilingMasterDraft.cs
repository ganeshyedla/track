using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public class CustomerFilingMasterDraft
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal DraftId { get; set; }
        public decimal? CustomerID { get; set; }
        public decimal? FilingID { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? Status { get; set; }
    }
}
