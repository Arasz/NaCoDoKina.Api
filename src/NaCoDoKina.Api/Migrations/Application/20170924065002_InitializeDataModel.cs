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
                name: "DeletedMovieMarks",
                columns: table => new
                {
                    MovieId = table.Column<long>(type: "int8", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedMovieMarks", x => new { x.MovieId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    PosterUrl = table.Column<string>(type: "text", nullable: true),
                    AgeLimit = table.Column<string>(type: "text", nullable: true),
                    CrewDescription = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Director = table.Column<string>(type: "text", nullable: true),
                    Genre = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    Length = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MovieId = table.Column<long>(type: "int8", nullable: false),
                    OriginalTitle = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceUrl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MovieDetailsId = table.Column<long>(type: "int8", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceUrl_Movies_MovieDetailsId",
                        column: x => x.MovieDetailsId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_CinemaNetworks_ServiceUrl_UrlId",
                        column: x => x.UrlId,
                        principalTable: "ServiceUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CinemaNetworkId = table.Column<long>(type: "int8", nullable: true),
                    Name = table.Column<string>(type: "varchar(225)", maxLength: 225, nullable: false),
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
                        name: "FK_Cinemas_ServiceUrl_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "ServiceUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovieShowtimes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CinemaId = table.Column<long>(type: "int8", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    MovieId = table.Column<long>(type: "int8", nullable: true),
                    ShowTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    ShowType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShowtimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieShowtimes_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovieShowtimes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_MovieShowtimes_CinemaId",
                table: "MovieShowtimes",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtimes_MovieId",
                table: "MovieShowtimes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUrl_MovieDetailsId",
                table: "ServiceUrl",
                column: "MovieDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUrl_Name",
                table: "ServiceUrl",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedMovieMarks");

            migrationBuilder.DropTable(
                name: "MovieShowtimes");

            migrationBuilder.DropTable(
                name: "Cinemas");

            migrationBuilder.DropTable(
                name: "CinemaNetworks");

            migrationBuilder.DropTable(
                name: "ServiceUrl");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
