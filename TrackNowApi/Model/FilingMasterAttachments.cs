﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class FilingMasterAttachments
    {
        public decimal? FilingId { get; set; }
        public long AttachmentId { get; set; }
        public string? AttachmentPath { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}