using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recrutement_plateforme.Migrations
{
    /// <inheritdoc />
    public partial class aaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CandidateId1",
                table: "CandidateJob",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateJob_CandidateId1",
                table: "CandidateJob",
                column: "CandidateId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateJob_Candidates_CandidateId1",
                table: "CandidateJob",
                column: "CandidateId1",
                principalTable: "Candidates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateJob_Candidates_CandidateId1",
                table: "CandidateJob");

            migrationBuilder.DropIndex(
                name: "IX_CandidateJob_CandidateId1",
                table: "CandidateJob");

            migrationBuilder.DropColumn(
                name: "CandidateId1",
                table: "CandidateJob");
        }
    }
}
