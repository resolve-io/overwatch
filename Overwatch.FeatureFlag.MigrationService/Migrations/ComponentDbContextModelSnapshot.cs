﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Overwatch.FeatureFlag.Api.Persistence;

#nullable disable

namespace Overwatch.FeatureFlag.Data.Migrations
{
    [DbContext(typeof(ComponentDbContext))]
    partial class ComponentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Overwatch.FeatureFlag.Api.Persistence.EnvironmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Environment", (string)null);
                });

            modelBuilder.Entity("Overwatch.FeatureFlag.Api.Persistence.FeatureEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Feature", (string)null);
                });

            modelBuilder.Entity("Overwatch.FeatureFlag.Api.Persistence.RuleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("EnvironmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Tenant")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("EnvironmentId");

                    b.HasIndex("FeatureId");

                    b.ToTable("Rule", (string)null);
                });

            modelBuilder.Entity("Overwatch.FeatureFlag.Api.Persistence.RuleEntity", b =>
                {
                    b.HasOne("Overwatch.FeatureFlag.Api.Persistence.EnvironmentEntity", "Environment")
                        .WithMany("Rules")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Overwatch.FeatureFlag.Api.Persistence.FeatureEntity", "Feature")
                        .WithMany("Rules")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Environment");

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("Overwatch.FeatureFlag.Api.Persistence.EnvironmentEntity", b =>
                {
                    b.Navigation("Rules");
                });

            modelBuilder.Entity("Overwatch.FeatureFlag.Api.Persistence.FeatureEntity", b =>
                {
                    b.Navigation("Rules");
                });
#pragma warning restore 612, 618
        }
    }
}
