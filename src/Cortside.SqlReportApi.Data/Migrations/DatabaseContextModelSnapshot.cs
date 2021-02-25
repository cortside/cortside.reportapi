﻿// <auto-generated />
using System;
using Cortside.SqlReportApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cortside.SqlReportApi.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PermissionId");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<int>("ReportGroupId")
                        .HasColumnType("int");

                    b.HasKey("ReportId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("ReportGroupId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.ReportArgument", b =>
                {
                    b.Property<int>("ReportArgumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArgName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArgType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReportArgumentQueryId")
                        .HasColumnType("int");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.HasKey("ReportArgumentId");

                    b.HasIndex("ReportArgumentQueryId");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportArgument");
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.ReportArgumentQuery", b =>
                {
                    b.Property<int>("ReportArgumentQueryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArgQuery")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReportArgumentQueryId");

                    b.ToTable("ReportArgumentQuery");
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.ReportGroup", b =>
                {
                    b.Property<int>("ReportGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReportGroupId");

                    b.ToTable("ReportGroup");
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.SqlReportApi", b =>
                {
                    b.Property<Guid>("SqlReportApiId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreateSubjectId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedSubjectId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SqlReportApiId");

                    b.HasIndex("CreateSubjectId");

                    b.HasIndex("LastModifiedSubjectId");

                    b.ToTable("SqlReportApi");
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.Subject", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FamilyName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("GivenName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("UserPrincipalName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("SubjectId");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.Report", b =>
                {
                    b.HasOne("Cortside.SqlReportApi.Domain.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cortside.SqlReportApi.Domain.ReportGroup", "ReportGroup")
                        .WithMany()
                        .HasForeignKey("ReportGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.ReportArgument", b =>
                {
                    b.HasOne("Cortside.SqlReportApi.Domain.ReportArgumentQuery", "ReportArgumentQuery")
                        .WithMany()
                        .HasForeignKey("ReportArgumentQueryId");

                    b.HasOne("Cortside.SqlReportApi.Domain.Report", null)
                        .WithMany("ReportArguments")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cortside.SqlReportApi.Domain.SqlReportApi", b =>
                {
                    b.HasOne("Cortside.SqlReportApi.Domain.Subject", "CreatedSubject")
                        .WithMany()
                        .HasForeignKey("CreateSubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cortside.SqlReportApi.Domain.Subject", "LastModifiedSubject")
                        .WithMany()
                        .HasForeignKey("LastModifiedSubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
