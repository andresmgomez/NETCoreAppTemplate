using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TemplateRESTful.Persistence.Migrations
{
    public partial class RemovePictureFieldInIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                schema: "Identity",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                schema: "Identity",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
