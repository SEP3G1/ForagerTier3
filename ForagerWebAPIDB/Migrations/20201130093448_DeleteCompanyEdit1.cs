using Microsoft.EntityFrameworkCore.Migrations;

namespace ForagerWebAPIDB.Migrations
{
    public partial class DeleteCompanyEdit1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WishDeletion",
                table: "Companies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WishDeletion",
                table: "Companies");
        }
    }
}
