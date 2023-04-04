using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class AppConfiguration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ConfigId { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? FilingId { get; set; }
        public string? ConfigItem { get; set; }
        public string? ConfigItemValue { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
    public partial class Approvers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ApproverId { get; set; }
        public string? ApproverName { get; set; }
        public decimal? CustomerId { get; set; }
        public string? Juristiction { get; set; }
        public string? State { get; set; }
        public bool? Isdefault { get; set; }
        public long ApproverGroupId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
    public partial class ApproverConfiguration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? ApproverConfigId { get; set; }
        public long ApproverGroupId { get; set; }
        public long? ApproverLevel { get; set; }
        public string? FilingType { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
    public partial class Roles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
    public partial class UsersRoles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserRoleId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
    public partial class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? LoginId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
