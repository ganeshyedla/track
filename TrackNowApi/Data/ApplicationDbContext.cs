using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Reflection.Metadata;
using TrackNowApi.Model;
//using System.Data.Entity;


namespace TrackNowApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerHistory> CustomerHistory { get; set; }
        public DbSet<FilingMaster> FilingMaster { get; set; }
        public DbSet<ReferenceDoc> ReferenceDoc { get; set; }
        public DbSet<BusinessCategoryMaster> BusinessCategoryMaster { get; set; }
       // public DbSet<CustomerFilingMaster> CustomerFilingMaster { get; set; }
        public DbSet<FilingMasterDraft> FilingMasterDraft { get; set; }
        public DbSet<FilingMasterWorkflow> FilingMasterWorkflow { get; set; }
        public DbSet<WorkflowTracking> WorkflowTracking { get; set; }
        public DbSet<SystemFilingFollowup> SystemFilingFollowup { get; set; }
        public DbSet<FilingMasterHistory> FilingMasterHistory { get; set; }
        public DbSet<CustomerFilingMasterHistory> CustomerFilingMasterHistory { get; set; }
        public DbSet<FilingBusinessCategory> FilingBusinessCategory { get; set; }
        public DbSet<CustomerBusinessCategory> CustomerBusinessCategory { get; set; }
        public DbSet<ApproverConfiguration> ApproverConfiguration { get; set; }
        public DbSet<Approvers> Approvers { get; set; }
        public DbSet<CustomerFilingWorkflowComments> CustomerFilingWorkflowComments { get; set; }
        public DbSet<CustomerComments> CustomerComments { get; set; }
        public DbSet<CustomerApprovalStatus> CustomerApprovalStatus { get; set; }
        public DbSet<CustomerFilingDraftComments> CustomerFilingDraftComments { get; set; }
        public DbSet<CustomerFilingComments> CustomerFilingComments { get; set; }
        public DbSet<FilingMasterWorkflowComments> FilingMasterWorkflowComments { get; set; }
        public DbSet<FilingMasterDraftComments> FilingMasterDraftComments { get; set; }
        public DbSet<FilingMasterComments> FilingMasterComments { get; set; }
        //public DbSet<CustomerFilingMasterDraft> CustomerFilingMasterDraft { get; set; }
        public DbSet<CustomerFilingMasterWorkflow> CustomerFilingMasterWorkflow { get; set; }
        public DbSet<CustomerFilingTrackingComments> CustomerFilingTrackingComments { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UsersRoles> UsersRoles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<FilingApprovalStatus> FilingApprovalStatus { get; set; }
        public DbSet<FilingMasterAttachments> FilingMasterAttachments { get; set; }
        public DbSet<CustomerFilingTrackingCommentsAttachments> CustomerFilingTrackingCommentsAttachments { get; set; }
        public DbSet<CustomerFilingWorkflowNotifications> CustomerFilingWorkflowNotifications { get; set; }
        public DbSet<AppConfiguration> AppConfiguration { get; set; }
        public DbSet<CustomerFilingWorkflowCommentsAttachments> CustomerFilingWorkflowCommentsAttachments { get; set; }
		public DbSet<FilingMasterDraftAttachments> FilingMasterDraftAttachments { get; set; }
        public DbSet<FilingMasterDraftCommentsAttachments> FilingMasterDraftCommentsAttachments { get; set; }
        public DbSet<FilingMasterWorkflowCommentsAttachments> FilingMasterWorkflowCommentsAttachments { get; set; }
        public DbSet<MasterFilingAttachments> MasterFilingAttachments { get; set; }
        public DbSet<FilingMasterWorkflowNotifications> FilingMasterWorkflowNotifications { get; set; }
        public DbSet<CustomerAttachments> CustomerAttachments { get; set; }
        public DbSet<CustomerFilingAttachments> CustomerFilingAttachments { get; set; }
        public DbSet<CustomerFilingCommentsAttachments> CustomerFilingCommentsAttachments { get; set; }
        public DbSet<CustomerFilingDraftAttachments> CustomerFilingDraftAttachments { get; set; }
        public DbSet<CustomerFilingDraftCommentsAttachments> CustomerFilingDraftCommentsAttachments { get; set; }
        public DbSet<CustomerFilingTrackingAttachments> CustomerFilingTrackingAttachments { get; set; }
        public DbSet<CustomerFilingTrackingNotifications> CustomerFilingTrackingNotifications { get; set; }
        public DbSet<CustomerFileTracking> CustomerFileTracking { get; set; }
        public DbSet<CustomerFilingMaster> CustomerFilingMaster { get; set; }
        public DbSet<CustomerDraftBusinessCategory> CustomerDraftBusinessCategory { get; set; }
        public DbSet<CustomerFilingMasterDraft> CustomerFilingMasterDraft { get; set; }
        public DbSet<FilingDraftBusinessCategory> FilingDraftBusinessCategory { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<FilingDraftBusinessCategory>()
            .HasKey(t => new { t.Id });

            modelBuilder.Entity<AppConfiguration>()
            .HasKey(t => new { t.ConfigId });

            modelBuilder.Entity<FilingApprovalStatus>()
           .HasKey(t => new { t.FilingApprovalId });

           modelBuilder.Entity<CustomerFilingWorkflowNotifications>()
           .HasKey(t => new { t.NotificationId });

            modelBuilder.Entity<CustomerFilingTrackingCommentsAttachments>()
           .HasKey(t => new { t.AttachmentId });

            modelBuilder.Entity<FilingMasterAttachments>()
           .HasKey(t => new { t.AttachmentId });

 			modelBuilder.Entity<CustomerFilingWorkflowCommentsAttachments>()
          .HasKey(t => new { t.AttachmentId });
          
            modelBuilder.Entity<Roles>()
            .HasKey(t => new { t.RoleId });
            
            modelBuilder.Entity<Users>()
           .HasKey(t => new { t.UserId });

            modelBuilder.Entity<UsersRoles>()
             .HasKey(t => new { t.UserRoleId });

            modelBuilder.Entity<CustomerFilingTrackingComments>()
          .HasKey(t => new { t.CommentsId });

            modelBuilder.Entity<CustomerFilingComments>()
             .HasKey(t => new { t.CommentsId});
             

            modelBuilder.Entity<CustomerFilingDraftComments>()
             .HasKey(t => new { t.CommentsId });

            modelBuilder.Entity<FilingMasterComments>()
             .HasKey(t => new { t.CommentsId });

            modelBuilder.Entity<FilingMasterDraftComments>()
             .HasKey(t => new { t.CommentsId });

            modelBuilder.Entity<FilingMasterWorkflowComments>()
             .HasKey(t => new { t.CommentsId });

            modelBuilder.Entity<CustomerFilingWorkflowComments>()
             .HasKey(t => new { t.CommentsId });

            modelBuilder.Entity<CustomerComments>()
             .HasKey(t => new { t.CommentsId });
            
 			modelBuilder.Entity<CustomerApprovalStatus>()
             .HasKey(t => new { t.CustomerApprovalId });
            modelBuilder.Entity<CustomerFilingMasterWorkflow>()
                .HasKey(t => new { t.WorkflowId });

            modelBuilder.Entity<CustomerFilingMasterDraft>()
                            .HasKey(t => new { t.DraftId });

            modelBuilder.Entity<ApproverConfiguration>()
                .HasKey(t => new { t.ApproverConfigId });

            modelBuilder.Entity<Approvers>()
                .HasKey(t => new { t.Id});

            modelBuilder.Entity<CustomerFilingMaster>()
                .HasKey(t => new { t.CustomerId, t.FilingId });

            modelBuilder.Entity<CustomerBusinessCategory>()
             .HasKey(t => new { t.CustomerId, t.BusinessCategoryId });

            modelBuilder.Entity<FilingBusinessCategory>()
             .HasKey(t => new { t.FilingId, t.BusinessCategoryId });

            modelBuilder.Entity<CustomerHistory>()
                .HasKey(t => t.CustomerId);

            modelBuilder.Entity<FilingMaster>()
                .HasKey(t => t.FilingId);

            modelBuilder.Entity<BusinessCategoryMaster>()
                .HasKey(t => t.BusinessCategoryId);

            modelBuilder.Entity<ReferenceDoc>()
               .HasKey(t => t.FilingId);
            
            modelBuilder.Entity<FilingMasterDraft>()
                .HasKey(t => new { t.DraftId });

            modelBuilder.Entity<FilingMasterWorkflow>()
            .HasKey(t => new { t.WorkflowId });

            modelBuilder.Entity<WorkflowTracking>()
            .HasKey(t => new { t.WorkflowTrackId });

            modelBuilder.Entity<SystemFilingFollowup>()
            .HasKey(t => new { t.FileTrackingId });

            modelBuilder.Entity<FilingMasterHistory>()
           .HasKey(t => new { t.Historyid });

            modelBuilder.Entity<CustomerFilingMasterHistory>()
          .HasKey(t => new { t.historyid });

            modelBuilder.Entity<CustomerHistory>()
          .HasKey(t => new { t.historyid });

            modelBuilder.Entity<FilingMasterDraftAttachments>()
          .HasKey(t => new { t.AttachmentId});

            modelBuilder.Entity<FilingMasterDraftCommentsAttachments>()
              .HasKey(t => new { t.AttachmentId });

            modelBuilder.Entity<FilingMasterWorkflowCommentsAttachments>()
               .HasKey(t => new { t.AttachmentId });

            modelBuilder.Entity<MasterFilingAttachments>()
               .HasKey(t => new { t.FollowupId });

            modelBuilder.Entity<FilingMasterWorkflowNotifications>()
              .HasKey(t => new { t.NotificationId});

           
            modelBuilder.Entity<CustomerAttachments>()
              .HasKey(t => new { t.CustomerId});

            modelBuilder.Entity<CustomerFilingAttachments>()
              .HasKey(t => new { t.FollowupId });

            modelBuilder.Entity<CustomerFilingCommentsAttachments>()
              .HasKey(t => new { t.AttachmentId });

            modelBuilder.Entity<CustomerFilingDraftAttachments>()
              .HasKey(t => new { t.DraftId });

            modelBuilder.Entity<CustomerFilingDraftCommentsAttachments>()
              .HasKey(t => new { t.AttachmentId });

            modelBuilder.Entity<CustomerFilingTrackingAttachments>()
              .HasKey(t => new { t.FileTrackingId });

            modelBuilder.Entity<CustomerFilingTrackingNotifications>()
              .HasKey(t => new { t.WorkflowId });

            modelBuilder.Entity<CustomerFileTracking>()
             .HasKey(t => new { t.FileTrackingId });

            modelBuilder.Entity<CustomerFilingMaster>()
            .HasKey(t => new { t.Id});

            modelBuilder.Entity<CustomerDraftBusinessCategory>()
            .HasKey(t => new { t.Id });

            modelBuilder.Entity<CustomerFilingMasterDraft>()
            .HasKey(t => new { t.DraftId });
        }

    }
}
