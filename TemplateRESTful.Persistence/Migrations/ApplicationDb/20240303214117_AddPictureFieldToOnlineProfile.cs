using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TemplateRESTful.Persistence.Migrations.ApplicationDb
{
    public partial class AddPictureFieldToOnlineProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "OnlineProfiles",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "OnlineProfiles");
        }
    }
}
