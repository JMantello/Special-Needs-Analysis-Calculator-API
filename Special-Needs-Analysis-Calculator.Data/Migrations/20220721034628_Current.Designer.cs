﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Special_Needs_Analysis_Calculator.Data.Database;
using Special_Needs_Analysis_Calculator.Data.Models.People;

#nullable disable

namespace Special_Needs_Analysis_Calculator.Data.Migrations
{
    [DbContext(typeof(SpecialNeedsAnalysisDbContext))]
    [Migration("20220721034628_Current")]
    partial class Current
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Special_Needs_Analysis_Calculator.Data.Models.Login.SessionTokenModel", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("SessionToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Email");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Special_Needs_Analysis_Calculator.Data.Models.Login.UserLogin", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Email");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Special_Needs_Analysis_Calculator.Data.UserDocument", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<UserModel>("User")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
