using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherLens.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weather_queries_locations_LocationId",
                table: "weather_queries");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_query_variables_WeatherVariables_VariableId",
                table: "weather_query_variables");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_query_variables_weather_queries_QueryId",
                table: "weather_query_variables");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_results_WeatherVariables_VariableId",
                table: "weather_results");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_results_weather_queries_QueryId",
                table: "weather_results");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "WeatherVariables",
                newName: "unit");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "WeatherVariables",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WeatherVariables",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WeatherVariables",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "WeatherVariables",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "weather_results",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VariableId",
                table: "weather_results",
                newName: "variable_id");

            migrationBuilder.RenameColumn(
                name: "QueryId",
                table: "weather_results",
                newName: "query_id");

            migrationBuilder.RenameColumn(
                name: "ProbabilityExtreme",
                table: "weather_results",
                newName: "probability_extreme");

            migrationBuilder.RenameColumn(
                name: "ExtremeCondition",
                table: "weather_results",
                newName: "extreme_condition");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "weather_results",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_weather_results_VariableId",
                table: "weather_results",
                newName: "IX_weather_results_variable_id");

            migrationBuilder.RenameIndex(
                name: "IX_weather_results_QueryId",
                table: "weather_results",
                newName: "IX_weather_results_query_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "weather_query_variables",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VariableId",
                table: "weather_query_variables",
                newName: "variable_id");

            migrationBuilder.RenameColumn(
                name: "QueryId",
                table: "weather_query_variables",
                newName: "query_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "weather_query_variables",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_weather_query_variables_VariableId",
                table: "weather_query_variables",
                newName: "IX_weather_query_variables_variable_id");

            migrationBuilder.RenameIndex(
                name: "IX_weather_query_variables_QueryId",
                table: "weather_query_variables",
                newName: "IX_weather_query_variables_query_id");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "weather_queries",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "weather_queries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "weather_queries",
                newName: "location_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "weather_queries",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_weather_queries_LocationId",
                table: "weather_queries",
                newName: "IX_weather_queries_location_id");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "locations",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "locations",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "locations",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "locations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "locations",
                newName: "created_at");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_queries_locations_location_id",
                table: "weather_queries",
                column: "location_id",
                principalTable: "locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_query_variables_WeatherVariables_variable_id",
                table: "weather_query_variables",
                column: "variable_id",
                principalTable: "WeatherVariables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_query_variables_weather_queries_query_id",
                table: "weather_query_variables",
                column: "query_id",
                principalTable: "weather_queries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_results_WeatherVariables_variable_id",
                table: "weather_results",
                column: "variable_id",
                principalTable: "WeatherVariables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_results_weather_queries_query_id",
                table: "weather_results",
                column: "query_id",
                principalTable: "weather_queries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weather_queries_locations_location_id",
                table: "weather_queries");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_query_variables_WeatherVariables_variable_id",
                table: "weather_query_variables");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_query_variables_weather_queries_query_id",
                table: "weather_query_variables");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_results_WeatherVariables_variable_id",
                table: "weather_results");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_results_weather_queries_query_id",
                table: "weather_results");

            migrationBuilder.RenameColumn(
                name: "unit",
                table: "WeatherVariables",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "WeatherVariables",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "WeatherVariables",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "WeatherVariables",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "WeatherVariables",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "weather_results",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "variable_id",
                table: "weather_results",
                newName: "VariableId");

            migrationBuilder.RenameColumn(
                name: "query_id",
                table: "weather_results",
                newName: "QueryId");

            migrationBuilder.RenameColumn(
                name: "probability_extreme",
                table: "weather_results",
                newName: "ProbabilityExtreme");

            migrationBuilder.RenameColumn(
                name: "extreme_condition",
                table: "weather_results",
                newName: "ExtremeCondition");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "weather_results",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_weather_results_variable_id",
                table: "weather_results",
                newName: "IX_weather_results_VariableId");

            migrationBuilder.RenameIndex(
                name: "IX_weather_results_query_id",
                table: "weather_results",
                newName: "IX_weather_results_QueryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "weather_query_variables",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "variable_id",
                table: "weather_query_variables",
                newName: "VariableId");

            migrationBuilder.RenameColumn(
                name: "query_id",
                table: "weather_query_variables",
                newName: "QueryId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "weather_query_variables",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_weather_query_variables_variable_id",
                table: "weather_query_variables",
                newName: "IX_weather_query_variables_VariableId");

            migrationBuilder.RenameIndex(
                name: "IX_weather_query_variables_query_id",
                table: "weather_query_variables",
                newName: "IX_weather_query_variables_QueryId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "weather_queries",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "weather_queries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "location_id",
                table: "weather_queries",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "weather_queries",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_weather_queries_location_id",
                table: "weather_queries",
                newName: "IX_weather_queries_LocationId");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "locations",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "locations",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "locations",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "locations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "locations",
                newName: "CreatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_queries_locations_LocationId",
                table: "weather_queries",
                column: "LocationId",
                principalTable: "locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_query_variables_WeatherVariables_VariableId",
                table: "weather_query_variables",
                column: "VariableId",
                principalTable: "WeatherVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_query_variables_weather_queries_QueryId",
                table: "weather_query_variables",
                column: "QueryId",
                principalTable: "weather_queries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_results_WeatherVariables_VariableId",
                table: "weather_results",
                column: "VariableId",
                principalTable: "WeatherVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weather_results_weather_queries_QueryId",
                table: "weather_results",
                column: "QueryId",
                principalTable: "weather_queries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
