using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recrutement_plateforme.Migrations
{
    /// <inheritdoc />
    public partial class initital12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "Candidates",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Candidates",
                newName: "password");
        }
    }
}
