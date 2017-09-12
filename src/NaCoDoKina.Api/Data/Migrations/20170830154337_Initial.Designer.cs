﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NaCoDoKina.Api.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20170830154337_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Cinema", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<long?>("CinemaNetworkId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(225);

                    b.HasKey("Id");

                    b.HasIndex("CinemaNetworkId");

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.CinemaNetwork", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("CinemaNetworks");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Movie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.MovieDetails", b =>
                {
                    b.Property<long>("Id");

                    b.Property<string>("AgeLimit");

                    b.Property<string>("Crew");

                    b.Property<string>("Description");

                    b.Property<string>("Director");

                    b.Property<string>("Genre");

                    b.Property<string>("Language");

                    b.Property<string>("Length");

                    b.Property<string>("Link");

                    b.Property<string>("OriginalTitle");

                    b.Property<string>("PosterLink");

                    b.Property<string>("Production");

                    b.Property<string>("ReleaseDate");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.MovieShowtime", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CanBeWatched");

                    b.Property<long?>("CinemaId");

                    b.Property<DateTime>("Value");

                    b.Property<long?>("MovieId");

                    b.HasKey("Id");

                    b.HasIndex("CinemaId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieShowtimes");
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.Cinema", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.CinemaNetwork", "CinemaNetwork")
                        .WithMany("Cinemas")
                        .HasForeignKey("CinemaNetworkId");

                    b.OwnsOne("NaCoDoKina.Api.Entities.Center", "Center", b1 =>
                        {
                            b1.Property<long>("CinemaId");

                            b1.Property<double>("Latitude");

                            b1.Property<double>("Longitude");

                            b1.ToTable("Cinemas");

                            b1.HasOne("NaCoDoKina.Api.Entities.Cinema")
                                .WithOne("Center")
                                .HasForeignKey("NaCoDoKina.Api.Entities.Center", "CinemaId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.MovieDetails", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Movie", "Movie")
                        .WithOne("Details")
                        .HasForeignKey("NaCoDoKina.Api.Entities.MovieDetails", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NaCoDoKina.Api.Entities.MovieShowtime", b =>
                {
                    b.HasOne("NaCoDoKina.Api.Entities.Cinema", "Cinema")
                        .WithMany()
                        .HasForeignKey("CinemaId");

                    b.HasOne("NaCoDoKina.Api.Entities.Movie", "Movie")
                        .WithMany("MovieShowtimes")
                        .HasForeignKey("MovieId");
                });
#pragma warning restore 612, 618
        }
    }
}