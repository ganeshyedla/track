﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

namespace TrackNowApi.Model
{
    public partial class FilingDraftBusinessCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }
        public decimal? DraftId { get; set; }
        public decimal BusinessCategoryId { get; set; }
        public string? State { get; set; }
    }
}