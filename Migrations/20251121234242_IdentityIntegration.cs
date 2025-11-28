using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalorieBurnMgt.Migrations
{
    /// <inheritdoc />
    public partial class IdentityIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calories_AspNetUsers_UserId1",
                table: "Calories");

            migrationBuilder.DropIndex(
                name: "IX_Calories_UserId1",
                table: "Calories");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Calories");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Calories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Calories_UserId",
                table: "Calories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_AspNetUsers_UserId",
                table: "Calories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calories_AspNetUsers_UserId",
                table: "Calories");

            migrationBuilder.DropIndex(
                name: "IX_Calories_UserId",
                table: "Calories");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Calories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Calories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calories_UserId1",
                table: "Calories",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_AspNetUsers_UserId1",
                table: "Calories",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
