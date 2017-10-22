using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Infrastructure.Migrations.Application
{
    public partial class AddDefaultValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaLink_Movies_MovieDetailsId",
                table: "MediaLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewLink_Movies_MovieDetailsId",
                table: "ReviewLink");

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Movies",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "Brak danych",
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Director",
                table: "Movies",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "Brak danych",
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "text",
                nullable: true,
                defaultValue: "Brak opisu",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "No description");

            migrationBuilder.AlterColumn<string>(
                name: "CrewDescription",
                table: "Movies",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true,
                defaultValue: "Brak danych",
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AgeLimit",
                table: "Movies",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "Nieokreślono",
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: "Unspecified");

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

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Movies",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: "Brak danych");

            migrationBuilder.AlterColumn<string>(
                name: "Director",
                table: "Movies",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: "Brak danych");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                nullable: true,
                defaultValue: "No description",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Brak opisu");

            migrationBuilder.AlterColumn<string>(
                name: "CrewDescription",
                table: "Movies",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldDefaultValue: "Brak danych");

            migrationBuilder.AlterColumn<string>(
                name: "AgeLimit",
                table: "Movies",
                maxLength: 100,
                nullable: true,
                defaultValue: "Unspecified",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: "Nieokreślono");

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
