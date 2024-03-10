using Microsoft.EntityFrameworkCore.Migrations;

namespace TemplateRESTful.Persistence.Migrations.ApplicationDb
{
    public partial class AddIdentityFieldToOnlineProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "OnlineProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "OnlineProfiles");
        }
    }
}
