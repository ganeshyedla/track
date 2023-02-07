using Microsoft.EntityFrameworkCore;
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

    }
}
