using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recrutement_plateforme.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Recruteurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Recruteurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Recruteurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "Recruteurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Recruteurs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Recruteurs");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Recruteurs");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "Recruteurs");
        }
    }
}
