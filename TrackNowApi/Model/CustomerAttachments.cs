﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class CustomerAttachments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CustomerId { get; set; }
        public long? AttachmentId { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
    }
}