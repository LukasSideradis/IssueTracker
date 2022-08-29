using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.DataAccess.Migrations
{
    public partial class AddAssignedIssuesAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Issues_IssueId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IssueId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "IssueUser",
                columns: table => new
                {
                    AssignedIssuesId = table.Column<int>(type: "int", nullable: false),
                    AssignedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueUser", x => new { x.AssignedIssuesId, x.AssignedUsersId });
                    table.ForeignKey(
                        name: "FK_IssueUser_AspNetUsers_AssignedUsersId",
                        column: x => x.AssignedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueUser_Issues_AssignedIssuesId",
                        column: x => x.AssignedIssuesId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueUser_AssignedUsersId",
                table: "IssueUser",
                column: "AssignedUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueUser");

            migrationBuilder.AddColumn<int>(
                name: "IssueId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IssueId",
                table: "AspNetUsers",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Issues_IssueId",
                table: "AspNetUsers",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id");
        }
    }
}
