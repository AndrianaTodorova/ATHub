using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ATHub.Data.Migrations
{
    public partial class DeletedOnForVideoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Videos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Videos");
        }
    }
}
