using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Migrations.Application
{
    public partial class AddColumnsToMovieShowtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaLink_Movies_MovieDetailsId",
                table: "MediaLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewLink_Movies_MovieDetailsId",
                table: "ReviewLink");

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "MovieShowtimes",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BookingLink",
                table: "MovieShowtimes",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AgeLimit",
                table: "Movies",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

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

            migrationBuilder.DropColumn(
                name: "Available",
                table: "MovieShowtimes");

            migrationBuilder.DropColumn(
                name: "BookingLink",
                table: "MovieShowtimes");

            migrationBuilder.AlterColumn<string>(
                name: "AgeLimit",
                table: "Movies",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

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
