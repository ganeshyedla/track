﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class FilingMasterWorkflowNotifications
    {
       
        public decimal WorkflowId { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailSubject { get; set; }
        public string NotificationType { get; set; }
        public string NotificationText { get; set; }
        public bool? InformationRead { get; set; }
        public bool? InformationDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal NotificationId { get; set; }
    }
}