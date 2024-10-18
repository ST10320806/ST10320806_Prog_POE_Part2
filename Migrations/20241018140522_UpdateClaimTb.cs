using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog_POE_Part2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaimTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Claims",
                newName: "HoursWorked");

            migrationBuilder.AddColumn<string>(
                name: "ClaimNotes",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "HourlyRate",
                table: "Claims",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimNotes",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "HoursWorked",
                table: "Claims",
                newName: "Amount");
        }
    }
}
