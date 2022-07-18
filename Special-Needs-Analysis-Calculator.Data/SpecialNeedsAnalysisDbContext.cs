using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public class SpecialNeedsAnalysisDbContext : DbContext
    {
        public SpecialNeedsAnalysisDbContext(DbContextOptions<SpecialNeedsAnalysisDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(u => new { u.FirstName, u.LastName, u.Email } )
                .HasColumnType("jsonb");
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
