using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Data.Migrations
{
    public partial class UpdateMovieModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "MovieShowtimes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Cinemas");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CinemaNetworks");

            migrationBuilder.RenameColumn(
                name: "Center_Longitude",
                table: "Cinemas",
                newName: "Location_Longitude");

            migrationBuilder.RenameColumn(
                name: "Center_Latitude",
                table: "Cinemas",
                newName: "Location_Latitude");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "MovieShowtimes",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cinemas",
                type: "varchar(225)",
                maxLength: 225,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CinemaNetworks",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "MovieShowtimes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cinemas");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CinemaNetworks");

            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "Cinemas",
                newName: "Center_Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Latitude",
                table: "Cinemas",
                newName: "Center_Latitude");

            migrationBuilder.AddColumn<DateTime>(
                name: "Value",
                table: "MovieShowtimes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Cinemas",
                maxLength: 225,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CinemaNetworks",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
