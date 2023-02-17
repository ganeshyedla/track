using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class FilingMasterDraft
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal DraftId { get; set; }
        public decimal? FilingId { get; set; }
        public string? FilingDescription { get; set; }
        public string? FilingFrequency { get; set; }
        public string? StateInfo { get; set; }
        public string? RuleInfo { get; set; }
        public bool? Required { get; set; }
        public decimal FilingCategoryId { get; set; }
        public string? Jsidept { get; set; }
        public string? JsicontactName { get; set; }
        public string? JsicontactEmail { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? Juristiction { get; set; }
        public string? Notes { get; set; }
        public bool? ChangesInprogress { get; set; }
    }
}
