using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recrutement_plateforme.Migrations
{
    /// <inheritdoc />
    public partial class aaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CandidateId",
                table: "Jobs",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Candidates_CandidateId",
                table: "Jobs",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Candidates_CandidateId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CandidateId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Jobs");
        }
    }
}
