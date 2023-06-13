﻿// <auto-generated />
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlazorEcommerce.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BlazorEcommerce.Shared.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "A textbook that introduces topics with an intuitive geometrical or physical description and ties mathematical concepts to the students' experience. It can be used with or without technology, and special symbols indicate when a particular type of machine is required. In this revised edition, Stewart increases his emphasis on technology and innovation, and expands his focus on problem solving and applications.",
                            ImageUrl = "data:image/jpeg;base64,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",
                            Price = 1500.0,
                            Title = "Calculus"
                        },
                        new
                        {
                            Id = 2,
                            Description = "The Animator's Survival Kit: A Manual of Methods, Principles, and Formulas for Classical, Computer, Games, Stop Motion, and Internet Animators is an instructional book by Academy Award-winning animator and director Richard Williams.",
                            ImageUrl = "data:image/jpeg;base64,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",
                            Price = 300.0,
                            Title = "The Animator's Survival Kit"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Les Misérables is a French historical novel by Victor Hugo, first published in 1862, that is considered one of the greatest novels of the 19th century. Les Misérables has been popularized through numerous adaptations for film, television and the stage, including a musical.",
                            ImageUrl = "data:image/jpeg;base64,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",
                            Price = 650.0,
                            Title = "Les Miserables"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
