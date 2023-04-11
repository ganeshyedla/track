using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json;

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
        public string? FilingType { get; set; }
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
    
    public partial class APIStatus
    {
        public string   Status { get; set; }
        public long     ErrorCode { get; set; }
        public string   ErrorMessage { get; set; }
        public string   Data { get; set; }
    }
    public partial class APIStatusJSON
    {
        public string Status { get; set; }
        public long ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public JsonDocument Data { get; set; }
    }
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
    public class DownloadFileRequest
    {
        public string FileUrl { get; set; }
    }
    public class FilingUploadFilesRequest
    {
        public string? FilingId { get; set; }
        public string? CommentId { get; set; }
        public string? AttachmentId { get; set; }
        public string? DraftId { get; set; }
        public string? WorkflowId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
    public class CustomerFilingUpload
    {
        public string? CustomerId { get; set; }
        public string? FilingId { get; set; }
        public string? CommentId { get; set; }
        public string? AttachmentId { get; set; }
        public string? WorkflowId { get; set; }
        public string? DraftId { get; set; }
        public string? FileTrackingId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
    public class azzureupload
    {
        public string Message { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }

    }
}
