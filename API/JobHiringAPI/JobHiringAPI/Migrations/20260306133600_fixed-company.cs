using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHiringAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixedcompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Companies_CompanyID",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_CompanyID",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Areas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "Areas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CompanyID",
                table: "Areas",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Companies_CompanyID",
                table: "Areas",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "CompanyID");
        }
    }
}
