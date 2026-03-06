using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHiringAPI.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVs_Areas_AreaID",
                table: "CVs");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "CVs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_CVs_Areas_AreaID",
                table: "CVs",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVs_Areas_AreaID",
                table: "CVs");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "CVs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CVs_Areas_AreaID",
                table: "CVs",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
