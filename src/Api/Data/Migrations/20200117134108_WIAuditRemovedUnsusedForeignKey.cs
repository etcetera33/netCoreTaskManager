using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class WIAuditRemovedUnsusedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemAudits_WorkItems_WorkItemId",
                table: "WorkItemAudits");

            migrationBuilder.DropIndex(
                name: "IX_WorkItemAudits_WorkItemId",
                table: "WorkItemAudits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkItemAudits_WorkItemId",
                table: "WorkItemAudits",
                column: "WorkItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemAudits_WorkItems_WorkItemId",
                table: "WorkItemAudits",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "WorkItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
