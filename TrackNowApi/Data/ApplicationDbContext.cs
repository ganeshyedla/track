using Microsoft.EntityFrameworkCore;
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
        public DbSet<CustomerFilingMaster> CustomerFilingMaster { get; set; }
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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApproverConfiguration>()
                .HasKey(t => new { t.ApproverConfigID });

            modelBuilder.Entity<Approvers>()
                .HasKey(t => new { t.ApproverID});

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
           .HasKey(t => new { t.historyid });

            modelBuilder.Entity<CustomerFilingMasterHistory>()
          .HasKey(t => new { t.historyid });

            modelBuilder.Entity<CustomerHistory>()
          .HasKey(t => new { t.historyid });






        }

    }
}
