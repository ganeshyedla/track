﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class MasterFilingAttachments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FollowupId { get; set; }
        public long? AttachmentId { get; set; }
    }
}