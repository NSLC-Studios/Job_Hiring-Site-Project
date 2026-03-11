using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHiringAPI.Migrations
{
    /// <inheritdoc />
    public partial class finalfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Areas_AreaID",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "Jobs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Areas_AreaID",
                table: "Jobs",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Areas_AreaID",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Areas_AreaID",
                table: "Jobs",
                column: "AreaID",
                principalTable: "Areas",
                principalColumn: "AreaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
