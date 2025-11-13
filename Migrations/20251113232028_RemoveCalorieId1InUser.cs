using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalorieBurnMgt.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCalorieId1InUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Calories_CalorieId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CalorieId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CalorieId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CalorieId1",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Calories_UserId",
                table: "Calories",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_Users_UserId",
                table: "Calories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calories_Users_UserId",
                table: "Calories");

            migrationBuilder.DropIndex(
                name: "IX_Calories_UserId",
                table: "Calories");

            migrationBuilder.AddColumn<int>(
                name: "CalorieId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CalorieId1",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CalorieId1",
                table: "Users",
                column: "CalorieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Calories_CalorieId1",
                table: "Users",
                column: "CalorieId1",
                principalTable: "Calories",
                principalColumn: "CalorieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
