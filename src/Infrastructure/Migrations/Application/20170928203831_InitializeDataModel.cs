using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Migrations.Application
{
    public partial class InitializeDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedMovies",
                columns: table => new
                {
                    MovieId = table.Column<long>(type: "int8", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedMovies", x => new { x.MovieId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "MediaLink",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MediaType = table.Column<int>(type: "int4", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaLink", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PosterUrlId = table.Column<long>(type: "int8", nullable: false),
                    Title = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    AgeLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CrewDescription = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Director = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Genre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Language = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Length = table.Column<TimeSpan>(type: "interval", nullable: false),
                    OriginalTitle = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    MovieDetails_Title = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_MediaLink_PosterUrlId",
                        column: x => x.PosterUrlId,
                        principalTable: "MediaLink",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceLink",
                columns: table => new
                {
                    MediaType = table.Column<int>(type: "int4", nullable: true),
                    MovieDetailsId = table.Column<long>(type: "int8", nullable: true),
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    LogoId = table.Column<long>(type: "int8", nullable: true),
                    ReviewLink_MovieDetailsId = table.Column<long>(type: "int8", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Rating = table.Column<double>(type: "float8", nullable: true, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceLink_Movies_MovieDetailsId",
                        column: x => x.MovieDetailsId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceLink_ResourceLink_LogoId",
                        column: x => x.LogoId,
                        principalTable: "ResourceLink",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceLink_Movies_ReviewLink_MovieDetailsId",
                        column: x => x.ReviewLink_MovieDetailsId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CinemaNetworks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    UrlId = table.Column<long>(type: "int8", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaNetworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CinemaNetworks_ResourceLink_UrlId",
                        column: x => x.UrlId,
                        principalTable: "ResourceLink",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Address = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CinemaNetworkId = table.Column<long>(type: "int8", nullable: true),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    WebsiteId = table.Column<long>(type: "int8", nullable: true),
                    Location_Latitude = table.Column<double>(type: "float8", nullable: false),
                    Location_Longitude = table.Column<double>(type: "float8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cinemas_CinemaNetworks_CinemaNetworkId",
                        column: x => x.CinemaNetworkId,
                        principalTable: "CinemaNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cinemas_ResourceLink_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "ResourceLink",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovieShowtimes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CinemaId = table.Column<long>(type: "int8", nullable: false),
                    Language = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MovieId = table.Column<long>(type: "int8", nullable: false),
                    ShowTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    ShowType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShowtimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieShowtimes_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieShowtimes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CinemaNetworks_UrlId",
                table: "CinemaNetworks",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Cinemas_CinemaNetworkId",
                table: "Cinemas",
                column: "CinemaNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Cinemas_Name",
                table: "Cinemas",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cinemas_WebsiteId",
                table: "Cinemas",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_PosterUrlId",
                table: "Movies",
                column: "PosterUrlId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtimes_CinemaId",
                table: "MovieShowtimes",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtimes_MovieId",
                table: "MovieShowtimes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLink_MovieDetailsId",
                table: "ResourceLink",
                column: "MovieDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLink_Url",
                table: "ResourceLink",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLink_LogoId",
                table: "ResourceLink",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLink_ReviewLink_MovieDetailsId",
                table: "ResourceLink",
                column: "ReviewLink_MovieDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedMovies");

            migrationBuilder.DropTable(
                name: "MovieShowtimes");

            migrationBuilder.DropTable(
                name: "Cinemas");

            migrationBuilder.DropTable(
                name: "CinemaNetworks");

            migrationBuilder.DropTable(
                name: "ResourceLink");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "MediaLink");
        }
    }
}
