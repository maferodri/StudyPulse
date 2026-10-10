using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudyStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDetails_StudyStatus_StudyStatusId",
                table: "StudyDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyStatus",
                table: "StudyStatus");

            migrationBuilder.RenameTable(
                name: "StudyStatus",
                newName: "StudyStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyStatuses",
                table: "StudyStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDetails_StudyStatuses_StudyStatusId",
                table: "StudyDetails",
                column: "StudyStatusId",
                principalTable: "StudyStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDetails_StudyStatuses_StudyStatusId",
                table: "StudyDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyStatuses",
                table: "StudyStatuses");

            migrationBuilder.RenameTable(
                name: "StudyStatuses",
                newName: "StudyStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyStatus",
                table: "StudyStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDetails_StudyStatus_StudyStatusId",
                table: "StudyDetails",
                column: "StudyStatusId",
                principalTable: "StudyStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
