using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalorieBurnMgt.Migrations
{
    /// <inheritdoc />
    public partial class PutTotalCalorieBalanceInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Calories_UserId",
                table: "Calories");

            migrationBuilder.CreateIndex(
                name: "IX_Calories_UserId",
                table: "Calories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Calories_UserId",
                table: "Calories");

            migrationBuilder.CreateIndex(
                name: "IX_Calories_UserId",
                table: "Calories",
                column: "UserId",
                unique: true);
        }
    }
}
