﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Database
{
    public class SpecialNeedsAnalysisDbContext : DbContext
    {
        public SpecialNeedsAnalysisDbContext(DbContextOptions<SpecialNeedsAnalysisDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDocument>()
                .Property(d => d.User)
                .HasColumnType("jsonb");
        }

        public DbSet<UserDocument> Users { get; set; }
    }
}