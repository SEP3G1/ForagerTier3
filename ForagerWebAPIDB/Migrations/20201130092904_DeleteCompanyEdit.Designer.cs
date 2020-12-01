﻿// <auto-generated />
using ForagerWebAPIDB.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ForagerWebAPIDB.Migrations
{
    [DbContext(typeof(ForagerDBContext))]
    [Migration("20201130092904_DeleteCompanyEdit")]
    partial class DeleteCompanyEdit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ForagerWebAPIDB.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConnectionAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cvr")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfVotes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostCode")
                        .HasColumnType("TEXT");

                    b.Property<double>("TrustScore")
                        .HasColumnType("REAL");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Listing", b =>
                {
                    b.Property<int>("ListingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BestBefore")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasDelivery")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfViews")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PickupAddress")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PictureList")
                        .HasColumnType("TEXT");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<string>("StartDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ListingId");

                    b.HasIndex("ProductId");

                    b.ToTable("listings");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImagesString")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductCategory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ListingId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReportType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReportId");

                    b.HasIndex("ListingId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(16)
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("SecurityLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Employee", b =>
                {
                    b.HasBaseType("ForagerWebAPIDB.Models.User");

                    b.HasIndex("CompanyId");

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Listing", b =>
                {
                    b.HasOne("ForagerWebAPIDB.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Report", b =>
                {
                    b.HasOne("ForagerWebAPIDB.Models.Listing", null)
                        .WithMany("Reports")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Employee", b =>
                {
                    b.HasOne("ForagerWebAPIDB.Models.Company", null)
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Company", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("ForagerWebAPIDB.Models.Listing", b =>
                {
                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}