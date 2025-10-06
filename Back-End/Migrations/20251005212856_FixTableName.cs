using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherLens.Migrations
{
    /// <inheritdoc />
    public partial class FixTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weather_query_variables_WeatherVariables_variable_id",
                table: "weather_query_variables");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_results_WeatherVariables_variable_id",
                table: "weather_results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherVariables",
                table: "WeatherVariables");

            migrationBuilder.RenameTable(
                name: "WeatherVariables",
                newName: "weather_variables");

            migrationBuilder.AddPrimaryKey(
                name: "PK_weather_variables",
                table: "weather_variables",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_query_variables_weather_variables_variable_id",
                table: "weather_query_variables",
                column: "variable_id",
                principalTable: "weather_variables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_results_weather_variables_variable_id",
                table: "weather_results",
                column: "variable_id",
                principalTable: "weather_variables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weather_query_variables_weather_variables_variable_id",
                table: "weather_query_variables");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_results_weather_variables_variable_id",
                table: "weather_results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_weather_variables",
                table: "weather_variables");

            migrationBuilder.RenameTable(
                name: "weather_variables",
                newName: "WeatherVariables");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherVariables",
                table: "WeatherVariables",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_query_variables_WeatherVariables_variable_id",
                table: "weather_query_variables",
                column: "variable_id",
                principalTable: "WeatherVariables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_results_WeatherVariables_variable_id",
                table: "weather_results",
                column: "variable_id",
                principalTable: "WeatherVariables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
