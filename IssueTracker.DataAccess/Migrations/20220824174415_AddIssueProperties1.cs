using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.DataAccess.Migrations
{
    public partial class AddIssueProperties1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueAssignment_AspNetUsers_UserId",
                table: "IssueAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_IssueAssignment_Issues_IssueId",
                table: "IssueAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueAssignment",
                table: "IssueAssignment");

            migrationBuilder.RenameTable(
                name: "IssueAssignment",
                newName: "IssueAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_IssueAssignment_UserId",
                table: "IssueAssignments",
                newName: "IX_IssueAssignments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IssueAssignment_IssueId",
                table: "IssueAssignments",
                newName: "IX_IssueAssignments_IssueId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueAssignments",
                table: "IssueAssignments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueAssignments_AspNetUsers_UserId",
                table: "IssueAssignments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueAssignments_Issues_IssueId",
                table: "IssueAssignments",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueAssignments_AspNetUsers_UserId",
                table: "IssueAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_IssueAssignments_Issues_IssueId",
                table: "IssueAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueAssignments",
                table: "IssueAssignments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Issues");

            migrationBuilder.RenameTable(
                name: "IssueAssignments",
                newName: "IssueAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_IssueAssignments_UserId",
                table: "IssueAssignment",
                newName: "IX_IssueAssignment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IssueAssignments_IssueId",
                table: "IssueAssignment",
                newName: "IX_IssueAssignment_IssueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueAssignment",
                table: "IssueAssignment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueAssignment_AspNetUsers_UserId",
                table: "IssueAssignment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueAssignment_Issues_IssueId",
                table: "IssueAssignment",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
