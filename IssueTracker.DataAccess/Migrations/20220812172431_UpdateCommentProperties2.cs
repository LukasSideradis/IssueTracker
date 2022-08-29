using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.DataAccess.Migrations
{
    public partial class UpdateCommentProperties2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EditedDate",
                table: "Comments",
                newName: "LastUpdated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "Comments",
                newName: "EditedDate");
        }
    }
}
