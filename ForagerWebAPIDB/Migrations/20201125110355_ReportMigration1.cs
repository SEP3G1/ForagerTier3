using Microsoft.EntityFrameworkCore.Migrations;

namespace ForagerWebAPIDB.Migrations
{
    public partial class ReportMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_listings_ListingId",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Reports");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ListingId",
                table: "Reports",
                newName: "IX_Reports_ListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_listings_ListingId",
                table: "Reports",
                column: "ListingId",
                principalTable: "listings",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_listings_ListingId",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Report");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ListingId",
                table: "Report",
                newName: "IX_Report_ListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_listings_ListingId",
                table: "Report",
                column: "ListingId",
                principalTable: "listings",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
