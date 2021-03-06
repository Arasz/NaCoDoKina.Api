﻿// <auto-generated />

using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations.Application
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20171009202314_Intialization")]
    partial class Initialization
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("ApplicationCore.Entities.Cinemas.Cinema", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<long?>("CinemaNetworkId");

                    b.Property<string>("CinemaUrl");

                    b.Property<string>("ExternalId");

                    b.Property<string>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.HasIndex("CinemaNetworkId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Cinemas.CinemaNetwork", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CinemaNetworkUrl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CinemaNetworks");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.DisabledMovie", b =>
                {
                    b.Property<long>("MovieId");

                    b.Property<long>("UserId");

                    b.HasKey("MovieId", "UserId");

                    b.ToTable("DisabledMovies");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.ExternalMovie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CinemaNetworkId");

                    b.Property<string>("ExternalId");

                    b.Property<long?>("MovieId")
                        .IsRequired();

                    b.Property<string>("MovieUrl");

                    b.HasKey("Id");

                    b.HasIndex("CinemaNetworkId");

                    b.HasIndex("MovieId");

                    b.ToTable("ExternalMovie");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.Movie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(80)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.MovieDetails", b =>
                {
                    b.Property<long>("Id");

                    b.Property<string>("AgeLimit")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(null)
                        .HasMaxLength(100);

                    b.Property<string>("CrewDescription")
                        .HasMaxLength(300)
                        .IsUnicode(true);

                    b.Property<string>("Description")
                        .IsRequired()
                        .IsUnicode(true);

                    b.Property<string>("Director")
                        .HasMaxLength(100);

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Language")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(100);

                    b.Property<TimeSpan>("Length");

                    b.Property<string>("OriginalTitle")
                        .IsRequired()
                        .HasMaxLength(80)
                        .IsUnicode(true);

                    b.Property<string>("Production");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("MovieDetails_Title")
                        .HasMaxLength(80)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.MovieShowtime", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Available");

                    b.Property<string>("BookingLink");

                    b.Property<long?>("CinemaId")
                        .IsRequired();

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long?>("MovieId")
                        .IsRequired();

                    b.Property<DateTime>("ShowTime");

                    b.Property<string>("ShowType")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CinemaId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieShowtimes");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Resources.MediaLink", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MediaType");

                    b.Property<long?>("MovieDetailsId");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("MovieDetailsId");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("MediaLink");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Resources.ReviewLink", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LogoUrl")
                        .HasMaxLength(300);

                    b.Property<long?>("MovieDetailsId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<double>("Rating")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0.0);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("MovieDetailsId");

                    b.HasIndex("Name");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("ReviewLink");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Cinemas.Cinema", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Cinemas.CinemaNetwork", "CinemaNetwork")
                        .WithMany()
                        .HasForeignKey("CinemaNetworkId");

                    b.OwnsOne("ApplicationCore.Entities.Location", "Location", b1 =>
                        {
                            b1.Property<long>("CinemaId");

                            b1.Property<double>("Latitude");

                            b1.Property<double>("Longitude");

                            b1.ToTable("Cinemas");

                            b1.HasOne("ApplicationCore.Entities.Cinemas.Cinema")
                                .WithOne("Location")
                                .HasForeignKey("ApplicationCore.Entities.Location", "CinemaId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.ExternalMovie", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Cinemas.CinemaNetwork", "CinemaNetwork")
                        .WithMany()
                        .HasForeignKey("CinemaNetworkId");

                    b.HasOne("ApplicationCore.Entities.Movies.Movie")
                        .WithMany("ExternalMovies")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.MovieDetails", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Movies.Movie")
                        .WithOne("Details")
                        .HasForeignKey("ApplicationCore.Entities.Movies.MovieDetails", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApplicationCore.Entities.Movies.MovieShowtime", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Cinemas.Cinema", "Cinema")
                        .WithMany()
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ApplicationCore.Entities.Movies.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApplicationCore.Entities.Resources.MediaLink", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Movies.MovieDetails")
                        .WithMany("MediaResources")
                        .HasForeignKey("MovieDetailsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApplicationCore.Entities.Resources.ReviewLink", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Movies.MovieDetails")
                        .WithMany("MovieReviews")
                        .HasForeignKey("MovieDetailsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
