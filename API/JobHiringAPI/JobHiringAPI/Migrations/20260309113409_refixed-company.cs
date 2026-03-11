using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHiringAPI.Migrations
{
    /// <inheritdoc />
    public partial class refixedcompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Areas_AreaID",
                table: "Companies");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "Companies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyName",
                table: "Companies",
                column: "CompanyName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Areas_AreaID",
                table: "Companies",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Areas_AreaID",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CompanyName",
                table: "Companies");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Areas_AreaID",
                table: "Companies",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
