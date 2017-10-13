using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Infrastructure.Migrations.Application
{
    public partial class RelaxUniqueOnLinks : Migration
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
                name: "IX_ReviewLink_Url",
                table: "ReviewLink");

            migrationBuilder.DropIndex(
                name: "IX_MediaLink_Url",
                table: "MediaLink");

            migrationBuilder.AlterColumn<string>(
                name: "AgeLimit",
                table: "Movies",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PosterUrl",
                table: "Movies",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLink_Url",
                table: "ReviewLink",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_MediaLink_Url",
                table: "MediaLink",
                column: "Url");

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
                name: "IX_ReviewLink_Url",
                table: "ReviewLink");

            migrationBuilder.DropIndex(
                name: "IX_MediaLink_Url",
                table: "MediaLink");

            migrationBuilder.AlterColumn<string>(
                name: "AgeLimit",
                table: "Movies",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PosterUrl",
                table: "Movies",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLink_Url",
                table: "ReviewLink",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaLink_Url",
                table: "MediaLink",
                column: "Url",
                unique: true);

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
