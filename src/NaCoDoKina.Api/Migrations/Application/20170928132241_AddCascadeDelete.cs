using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Migrations.Application
{
    public partial class AddCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceLink_Movies_MovieDetailsId",
                table: "ResourceLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceLink_Movies_ReviewLink_MovieDetailsId",
                table: "ResourceLink");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceLink_Movies_MovieDetailsId",
                table: "ResourceLink",
                column: "MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceLink_Movies_ReviewLink_MovieDetailsId",
                table: "ResourceLink",
                column: "ReviewLink_MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceLink_Movies_MovieDetailsId",
                table: "ResourceLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceLink_Movies_ReviewLink_MovieDetailsId",
                table: "ResourceLink");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceLink_Movies_MovieDetailsId",
                table: "ResourceLink",
                column: "MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceLink_Movies_ReviewLink_MovieDetailsId",
                table: "ResourceLink",
                column: "ReviewLink_MovieDetailsId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
