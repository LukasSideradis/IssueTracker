using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.DataAccess.Migrations
{
    public partial class AddUserCollectionToIssue2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Issues_UserId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUsers",
                newName: "IssueId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Issues_IssueId",
                table: "AspNetUsers",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Issues_IssueId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IssueId",
                table: "AspNetUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_IssueId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Issues_UserId",
                table: "AspNetUsers",
                column: "UserId",
                principalTable: "Issues",
                principalColumn: "Id");
        }
    }
}
