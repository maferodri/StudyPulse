using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudyPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudyStatusEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyStatusId",
                table: "StudyDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StudyStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyDetails_StudyStatusId",
                table: "StudyDetails",
                column: "StudyStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDetails_StudyStatus_StudyStatusId",
                table: "StudyDetails",
                column: "StudyStatusId",
                principalTable: "StudyStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDetails_StudyStatus_StudyStatusId",
                table: "StudyDetails");

            migrationBuilder.DropTable(
                name: "StudyStatus");

            migrationBuilder.DropIndex(
                name: "IX_StudyDetails_StudyStatusId",
                table: "StudyDetails");

            migrationBuilder.DropColumn(
                name: "StudyStatusId",
                table: "StudyDetails");
        }
    }
}
