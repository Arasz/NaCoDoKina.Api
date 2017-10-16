using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Infrastructure.Migrations.Application
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CinemaNetworks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CinemaNetworkUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaNetworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisabledMovies",
                columns: table => new
                {
                    MovieId = table.Column<long>(type: "int8", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisabledMovies", x => new { x.MovieId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PosterUrl = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    AgeLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CrewDescription = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Director = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Genre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Language = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValue: ""),
                    Length = table.Column<TimeSpan>(type: "interval", nullable: false),
                    OriginalTitle = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    Production = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    MovieDetails_Title = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Address = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CinemaNetworkId = table.Column<long>(type: "int8", nullable: true),
                    CinemaUrl = table.Column<string>(type: "text", nullable: true),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    GroupId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "ExternalMovie",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CinemaNetworkId = table.Column<long>(type: "int8", nullable: true),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    MovieId = table.Column<long>(type: "int8", nullable: false),
                    MovieUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalMovie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalMovie_CinemaNetworks_CinemaNetworkId",
                        column: x => x.CinemaNetworkId,
                        principalTable: "CinemaNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaLink",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MediaType = table.Column<int>(type: "int4", nullable: false),
                    MovieDetailsId = table.Column<long>(type: "int8", nullable: true),
                    Url = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaLink_Movies_MovieDetailsId",
                        column: x => x.MovieDetailsId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewLink",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LogoUrl = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    MovieDetailsId = table.Column<long>(type: "int8", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Rating = table.Column<double>(type: "float8", nullable: false, defaultValue: 0.0),
                    Url = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewLink_Movies_MovieDetailsId",
                        column: x => x.MovieDetailsId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieShowtimes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Available = table.Column<bool>(type: "bool", nullable: false),
                    BookingLink = table.Column<string>(type: "text", nullable: true),
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
                name: "IX_CinemaNetworks_Name",
                table: "CinemaNetworks",
                column: "Name",
                unique: true);

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
                name: "IX_ExternalMovie_CinemaNetworkId",
                table: "ExternalMovie",
                column: "CinemaNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalMovie_MovieId",
                table: "ExternalMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaLink_MovieDetailsId",
                table: "MediaLink",
                column: "MovieDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaLink_Url",
                table: "MediaLink",
                column: "Url",
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
                name: "IX_ReviewLink_MovieDetailsId",
                table: "ReviewLink",
                column: "MovieDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLink_Name",
                table: "ReviewLink",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLink_Url",
                table: "ReviewLink",
                column: "Url",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisabledMovies");

            migrationBuilder.DropTable(
                name: "ExternalMovie");

            migrationBuilder.DropTable(
                name: "MediaLink");

            migrationBuilder.DropTable(
                name: "MovieShowtimes");

            migrationBuilder.DropTable(
                name: "ReviewLink");

            migrationBuilder.DropTable(
                name: "Cinemas");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "CinemaNetworks");
        }
    }
}