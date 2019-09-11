using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityRepositoryPoC.Data.Migrations
{
    public partial class AddedIsAdminPropertyToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "ApplicationUser",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "ApplicationUser");

           
        }
    }
}
