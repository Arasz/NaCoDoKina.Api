using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Infrastructure.Migrations.Application
{
    public partial class AddIndexOnCinemaAndMovieInMovieShowtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaLink_Movies_MovieDetailsId",
                table: "MediaLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewLink_Movies_MovieDetailsId",
                table: "ReviewLink");

            migrationBuilder.DropIndex(
                name: "IX_MovieShowtimes_CinemaId",
                table: "MovieShowtimes");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtimes_CinemaId_MovieId",
                table: "MovieShowtimes",
                columns: new[] { "CinemaId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MediaLink_Movies_MovieDetailsId",
                table: "MediaLink",
                column: "MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewLink_Movies_MovieDetailsId",
                table: "ReviewLink",
                column: "MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaLink_Movies_MovieDetailsId",
                table: "MediaLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewLink_Movies_MovieDetailsId",
                table: "ReviewLink");

            migrationBuilder.DropIndex(
                name: "IX_MovieShowtimes_CinemaId_MovieId",
                table: "MovieShowtimes");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtimes_CinemaId",
                table: "MovieShowtimes",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaLink_Movies_MovieDetailsId",
                table: "MediaLink",
                column: "MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewLink_Movies_MovieDetailsId",
                table: "ReviewLink",
                column: "MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
