using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalorieBurnMgt.Migrations
{
    /// <inheritdoc />
    public partial class SeededDataForFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistanceTaken",
                table: "Calories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "FoodId", "Calories", "Name" },
                values: new object[,]
                {
                    { 1, 95, "Apple (medium)" },
                    { 2, 105, "Banana (medium)" },
                    { 3, 80, "White bread (slice)" },
                    { 4, 215, "Brown rice (cooked, 1 cup)" },
                    { 5, 165, "Chicken breast (grilled, skinless, 100g)" },
                    { 6, 206, "Salmon (cooked, 100g)" },
                    { 7, 78, "Egg (large, boiled)" },
                    { 8, 164, "Almonds (raw, 28g)" },
                    { 9, 322, "Avocado (medium)" },
                    { 10, 31, "Broccoli (raw, 1 cup)" },
                    { 11, 25, "Carrot (medium)" },
                    { 12, 113, "Cheddar cheese (28g)" },
                    { 13, 149, "Whole milk (1 cup)" },
                    { 14, 73, "Greek yogurt (plain, low-fat, 100g)" },
                    { 15, 161, "Potato (baked, medium)" },
                    { 16, 271, "Beef steak (grilled, lean, 100g)" },
                    { 17, 221, "Pasta (cooked spaghetti, 1 cup)" },
                    { 18, 166, "Oatmeal (cooked, 1 cup)" },
                    { 19, 62, "Orange (medium)" },
                    { 20, 46, "Strawberries (1 cup)" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Foods",
                keyColumn: "FoodId",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "DistanceTaken",
                table: "Calories");
        }
    }
}
