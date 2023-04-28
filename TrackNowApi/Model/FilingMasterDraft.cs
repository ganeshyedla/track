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
        public string? FilingName { get; set; }
        public string? FilingDescription { get; set; }
        public string? FilingFrequency { get; set; }
        public string? StateInfo { get; set; }
        public string? RuleInfo { get; set; }
        public bool? Required { get; set; }
        public decimal FilingCategoryId { get; set; }
        public string? Jsidept { get; set; }
        public string? JsicontactName { get; set; }
        public string? JsicontactEmail { get; set; }
        public string? JSIContactNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? Juristiction { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public bool? ChangesInprogress { get; set; }
		public decimal? DueDayofFrequency { get; set; }
		public string? BusinessOperation { get; set; }
    }
    public partial class FilingMasterAudit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Process { get; set; }
        public string? RequestBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? ResponseStatus { get; set; }
        public string? ResponsedBy { get; set; }
        public DateTime? ResponseDate { get; set; }
        public decimal DraftId { get; set; }
        public decimal? FilingId { get; set; }
        public string? FilingName { get; set; }
        public string? FilingDescription { get; set; }
        public string? FilingFrequency { get; set; }
        public string? StateInfo { get; set; }
        public string? RuleInfo { get; set; }
        public bool? Required { get; set; }
        public string? Jsidept { get; set; }
        public string? JsicontactName { get; set; }
        public string? JsicontactEmail { get; set; }
        public string? JSIContactNumber { get; set; }
        public string? Juristiction { get; set; }
        public string? Notes { get; set; }
        public decimal? DueDayofFrequency { get; set; }
        public string? NewFilingName { get; set; }
        public string? NewFilingDescription { get; set; }
        public string? NewFilingFrequency { get; set; }
        public string? NewStateInfo { get; set; }
        public string? NewRuleInfo { get; set; }
        public bool? NewRequired { get; set; }
        public string? NewJsidept { get; set; }
        public string? NewJsicontactName { get; set; }
        public string? NewJsicontactEmail { get; set; }
        public string? NewJSIContactNumber { get; set; }
        public string? NewJuristiction { get; set; }
        public string? NewNotes { get; set; }
        public decimal? NewDueDayofFrequency { get; set; }
    }
}
