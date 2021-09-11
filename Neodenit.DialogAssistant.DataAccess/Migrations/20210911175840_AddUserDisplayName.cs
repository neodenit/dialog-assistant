using Microsoft.EntityFrameworkCore.Migrations;

namespace Neodenit.DialogAssistant.DataAccess.Migrations
{
    public partial class AddUserDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql("UPDATE AppUsers SET DisplayName = Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AppUsers");
        }
    }
}
