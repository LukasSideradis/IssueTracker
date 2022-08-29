using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.DataAccess.Migrations
{
    public partial class AddIssueAssignmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueUser");

            migrationBuilder.CreateTable(
                name: "IssueAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueAssignments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueAssignments_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueAssignments_IssueId",
                table: "IssueAssignments",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueAssignments_UserId",
                table: "IssueAssignments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueAssignments");

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
    }
}
