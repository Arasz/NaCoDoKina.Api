using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Migrations.Application
{
    public partial class AddGroupIdToCinema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaLink_Movies_MovieDetailsId",
                table: "MediaLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewLink_Movies_MovieDetailsId",
                table: "ReviewLink");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Cinemas",
                type: "text",
                nullable: true);

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
                name: "GroupId",
                table: "Cinemas");

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
