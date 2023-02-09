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
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerFilingMaster>()
                .HasKey(t => new { t.CustomerId, t.FilingId });

            modelBuilder.Entity<CustomerHistory>()
                .HasKey(t => t.CustomerId);

            modelBuilder.Entity<FilingMaster>()
                .HasKey(t => t.FilingId);

            modelBuilder.Entity<BusinessCategoryMaster>()
                .HasKey(t => t.BusinessCatergoryId);

            modelBuilder.Entity<ReferenceDoc>()
               .HasKey(t => t.FilingId);

        }

    }
}
