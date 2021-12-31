﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Data.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20211231192941_AddThePostModel")]
    partial class AddThePostModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Shared.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ThumbnailImagePath")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Description = "A description of category 1",
                            Name = "Category 1",
                            ThumbnailImagePath = "uploads/placeholder.jpg"
                        },
                        new
                        {
                            CategoryId = 2,
                            Description = "A description of category 2",
                            Name = "Category 2",
                            ThumbnailImagePath = "uploads/placeholder.jpg"
                        },
                        new
                        {
                            CategoryId = 3,
                            Description = "A description of category 3",
                            Name = "Category 3",
                            ThumbnailImagePath = "uploads/placeholder.jpg"
                        });
                });

            modelBuilder.Entity("Shared.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(65536)
                        .HasColumnType("TEXT");

                    b.Property<string>("Excerpt")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Published")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ThumbnailImagePath")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("PostId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            PostId = 1,
                            Author = "Ryan Bertram",
                            CategoryId = 1,
                            Content = "",
                            Excerpt = "This is an except for 1. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.",
                            PublishDate = new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9873),
                            Published = true,
                            ThumbnailImagePath = "uploads/placeholders.jpg",
                            Title = "First Post"
                        },
                        new
                        {
                            PostId = 2,
                            Author = "Ryan Bertram",
                            CategoryId = 2,
                            Content = "",
                            Excerpt = "This is an except for 2. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.",
                            PublishDate = new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9880),
                            Published = true,
                            ThumbnailImagePath = "uploads/placeholders.jpg",
                            Title = "Second Post"
                        },
                        new
                        {
                            PostId = 3,
                            Author = "Ryan Bertram",
                            CategoryId = 3,
                            Content = "",
                            Excerpt = "This is an except for 3. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.",
                            PublishDate = new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9881),
                            Published = true,
                            ThumbnailImagePath = "uploads/placeholders.jpg",
                            Title = "Third Post"
                        },
                        new
                        {
                            PostId = 4,
                            Author = "Ryan Bertram",
                            CategoryId = 1,
                            Content = "",
                            Excerpt = "This is an except for 4. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.",
                            PublishDate = new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9882),
                            Published = true,
                            ThumbnailImagePath = "uploads/placeholders.jpg",
                            Title = "Fourth Post"
                        },
                        new
                        {
                            PostId = 5,
                            Author = "Ryan Bertram",
                            CategoryId = 2,
                            Content = "",
                            Excerpt = "This is an except for 5. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.",
                            PublishDate = new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9884),
                            Published = true,
                            ThumbnailImagePath = "uploads/placeholders.jpg",
                            Title = "Fifth Post"
                        },
                        new
                        {
                            PostId = 6,
                            Author = "Ryan Bertram",
                            CategoryId = 3,
                            Content = "",
                            Excerpt = "This is an except for 6. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.",
                            PublishDate = new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9885),
                            Published = true,
                            ThumbnailImagePath = "uploads/placeholders.jpg",
                            Title = "Sixth Post"
                        });
                });

            modelBuilder.Entity("Shared.Models.Post", b =>
                {
                    b.HasOne("Shared.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Shared.Models.Category", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
