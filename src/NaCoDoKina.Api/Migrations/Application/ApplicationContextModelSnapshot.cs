﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities.Resources;
using System;

namespace NaCoDoKina.Api.Migrations.Application
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Cinemas.Cinema", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<long?>("CinemaNetworkId");

                    b.Property<string>("ExternalId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<long?>("WebsiteId");

                    b.HasKey("Id");

                    b.HasIndex("CinemaNetworkId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("WebsiteId");

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Cinemas.CinemaNetwork", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<long?>("UrlId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UrlId");

                    b.ToTable("CinemaNetworks");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.DeletedMovies", b =>
                {
                    b.Property<long>("MovieId");

                    b.Property<long>("UserId");

                    b.HasKey("MovieId", "UserId");

                    b.ToTable("DeletedMovies");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.ExternalId", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CinemaNetworkId");

                    b.Property<string>("MovieExternalId");

                    b.Property<long?>("MovieId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CinemaNetworkId");

                    b.HasIndex("MovieId");

                    b.ToTable("ExternalId");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.Movie", b =>
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

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.MovieDetails", b =>
                {
                    b.Property<long>("Id");

                    b.Property<string>("AgeLimit")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("CrewDescription")
                        .HasMaxLength(300)
                        .IsUnicode(true);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(true);

                    b.Property<string>("Director")
                        .HasMaxLength(100);

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<TimeSpan>("Length");

                    b.Property<string>("OriginalTitle")
                        .IsRequired()
                        .HasMaxLength(80)
                        .IsUnicode(true);

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("MovieDetails_Title")
                        .HasMaxLength(80)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.MovieShowtime", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

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

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Resources.ResourceLink", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("ResourceLink");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ResourceLink");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Resources.MediaLink", b =>
                {
                    b.HasBaseType("NaCoDoKina.Api.Entities.Resources.ResourceLink");

                    b.Property<int>("MediaType");

                    b.Property<long?>("MovieDetailsId");

                    b.HasIndex("MovieDetailsId");

                    b.ToTable("MediaLink");

                    b.HasDiscriminator().HasValue("MediaLink");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Resources.ReviewLink", b =>
                {
                    b.HasBaseType("NaCoDoKina.Api.Entities.Resources.ResourceLink");

                    b.Property<long?>("LogoId");

                    b.Property<long?>("MovieDetailsId")
                        .HasColumnName("ReviewLink_MovieDetailsId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<double>("Rating")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0.0);

                    b.HasIndex("LogoId");

                    b.HasIndex("MovieDetailsId");

                    b.ToTable("ReviewLink");

                    b.HasDiscriminator().HasValue("ReviewLink");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Cinemas.Cinema", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Cinemas.CinemaNetwork", "CinemaNetwork")
                        .WithMany()
                        .HasForeignKey("CinemaNetworkId");

                    b.HasOne("NaCoDoKina.Api.Entities.Resources.ResourceLink", "Website")
                        .WithMany()
                        .HasForeignKey("WebsiteId");

                    b.OwnsOne("NaCoDoKina.Api.Entities.Location", "Location", b1 =>
                        {
                            b1.Property<long>("CinemaId");

                            b1.Property<double>("Latitude");

                            b1.Property<double>("Longitude");

                            b1.ToTable("Cinemas");

                            b1.HasOne("NaCoDoKina.Api.Entities.Cinemas.Cinema")
                                .WithOne("Location")
                                .HasForeignKey("NaCoDoKina.Api.Entities.Location", "CinemaId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Cinemas.CinemaNetwork", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Resources.ResourceLink", "Url")
                        .WithMany()
                        .HasForeignKey("UrlId");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.ExternalId", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Cinemas.CinemaNetwork", "CinemaNetwork")
                        .WithMany()
                        .HasForeignKey("CinemaNetworkId");

                    b.HasOne("NaCoDoKina.Api.Entities.Movies.Movie")
                        .WithMany("ExternalIds")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.MovieDetails", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Movies.Movie")
                        .WithOne("Details")
                        .HasForeignKey("NaCoDoKina.Api.Entities.Movies.MovieDetails", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movies.MovieShowtime", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Cinemas.Cinema", "Cinema")
                        .WithMany()
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NaCoDoKina.Api.Entities.Movies.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Resources.MediaLink", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Movies.MovieDetails")
                        .WithMany("MediaResources")
                        .HasForeignKey("MovieDetailsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Resources.ReviewLink", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Resources.ResourceLink", "Logo")
                        .WithMany()
                        .HasForeignKey("LogoId");

                    b.HasOne("NaCoDoKina.Api.Entities.Movies.MovieDetails")
                        .WithMany("MovieReviews")
                        .HasForeignKey("MovieDetailsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
