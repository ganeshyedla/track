using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TrackNowApi.Model
{
    public partial class TrackNowContext : DbContext
    {
        public TrackNowContext()
        {
        }

        public TrackNowContext(DbContextOptions<TrackNowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppConfiguration> AppConfigurations { get; set; } = null!;
        public virtual DbSet<BusinessCategoryMaster> BusinessCategoryMasters { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerFileTracking> CustomerFileTrackings { get; set; } = null!;
        public virtual DbSet<CustomerFilingBusiness> CustomerFilingBusinesses { get; set; } = null!;
        public virtual DbSet<CustomerFilingFollowup> CustomerFilingFollowups { get; set; } = null!;
        public virtual DbSet<CustomerFilingMaster> CustomerFilingMasters { get; set; } = null!;
        public virtual DbSet<CustomerFilingMasterHistory> CustomerFilingMasterHistories { get; set; } = null!;
        public virtual DbSet<CustomerHistory> CustomerHistories { get; set; } = null!;
        public virtual DbSet<FilingMaster> FilingMasters { get; set; } = null!;
        public virtual DbSet<FilingMasterDraft> FilingMasterDrafts { get; set; } = null!;
        public virtual DbSet<FilingMasterHistory> FilingMasterHistories { get; set; } = null!;
        public virtual DbSet<FilingMasterWorkflow> FilingMasterWorkflows { get; set; } = null!;
        public virtual DbSet<ReferenceDoc> ReferenceDocs { get; set; } = null!;
        public virtual DbSet<SystemFilingFollowup> SystemFilingFollowups { get; set; } = null!;
        public virtual DbSet<WorkflowTracking> WorkflowTrackings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=qa-jsi-db.database.windows.net;Database=TrackNow; User id=dbadmin;Password=qaPMTcpr@peer1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfiguration>(entity =>
            {
                entity.HasKey(e => e.ConfigId)
                    .HasName("PK__AppConfi__C3BC333C666E83B1");

                entity.ToTable("AppConfiguration");

                entity.Property(e => e.ConfigId).HasColumnName("ConfigID");

                entity.Property(e => e.ApproverLevel)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigItem)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigItemValue)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BusinessCategoryMaster>(entity =>
            {
                entity.HasKey(e => e.BusinessCatergoryId)
                    .HasName("PK__Business__19093A2B8C184FB8");

                entity.ToTable("BusinessCategoryMaster");

                entity.Property(e => e.BusinessCatergoryId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BusinessCatergoryId");

                entity.Property(e => e.BusinessCategoryDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCategoryName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Customer");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCatergoryId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BusinessCatergoryID");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CustomerID");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Juristiction)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Mail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Poc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("POC");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentCustomerID)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ParentCustomerID");

            });

            modelBuilder.Entity<CustomerFileTracking>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CustomerFileTracking");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.FileTrackingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FileTrackingID");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerFilingBusiness>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CustomerFilingBusiness");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CategoryID");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CustomerID");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerFilingFollowup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CustomerFilingFollowup");

                entity.Property(e => e.AttachmentPath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CommText)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.CommunicationType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FileTrackingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FileTrackingID");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");
            });

            modelBuilder.Entity<CustomerFilingMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CustomerFilingMaster");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerFilingMasterHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CustomerFilingMasterHistory");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Dboperations)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DBOperations");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CustomerHistory");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CatergoryId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CatergoryID");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CustomerID");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dboperation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DBOPeration");

                entity.Property(e => e.Juristiction)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Mail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Poc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("POC");

                entity.Property(e => e.Source)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FilingMaster>(entity =>
            {
                entity.HasKey(e => e.FilingId)
                    .HasName("PK__FilingMa__60E4ECFB417D9B16");

                entity.ToTable("FilingMaster");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FilingID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilingCategoryId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FilingDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FilingFrequency)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.JsicontactEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIContactEmail");

                entity.Property(e => e.JsicontactName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIContactName");

                entity.Property(e => e.Jsidept)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIDept");

                entity.Property(e => e.Juristiction)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RuleInfo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StateInfo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FilingMasterDraft>(entity =>
            {
                entity.HasKey(e => e.DraftId)
                    .HasName("PK__FilingMa__3E93D63B4757A6B7");

                entity.ToTable("FilingMasterDraft");

                entity.Property(e => e.DraftId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DraftID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilingCategoryId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FilingDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FilingFrequency)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");

                entity.Property(e => e.JsicontactEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIContactEmail");

                entity.Property(e => e.JsicontactName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIContactName");

                entity.Property(e => e.Jsidept)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIDept");

                entity.Property(e => e.Juristiction)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RuleInfo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StateInfo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FilingMasterHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FilingMasterHistory");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dboperation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DBOPeration");

                entity.Property(e => e.FilingCategoryId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FilingDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FilingFrequency)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FilingID");

                entity.Property(e => e.JsicontactEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIContactEmail");

                entity.Property(e => e.JsicontactName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIContactName");

                entity.Property(e => e.Jsidept)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("JSIDept");

                entity.Property(e => e.Juristiction)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RuleInfo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StateInfo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FilingMasterWorkflow>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FilingMasterWorkflow");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DraftId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DraftID");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WorkflowId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("WorkflowID");
            });

            modelBuilder.Entity<ReferenceDoc>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AttachmentPath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DraftId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DraftID");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SystemFilingFollowup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SystemFilingFollowup");

                entity.Property(e => e.AttachmentPath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CommText)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.CommunicationType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FileTrackingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FileTrackingId");

                entity.Property(e => e.FilingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FilingID");
            });

            modelBuilder.Entity<WorkflowTracking>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("WorkflowTracking");

                entity.Property(e => e.Action)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Attachments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WorkflowId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("WorkflowID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
