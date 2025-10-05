using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherLens.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DataUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherVariables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weather_queries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_queries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weather_queries_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_weather_queries_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "weather_query_variables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    QueryId = table.Column<Guid>(type: "uuid", nullable: false),
                    VariableId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_query_variables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weather_query_variables_WeatherVariables_VariableId",
                        column: x => x.VariableId,
                        principalTable: "WeatherVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_weather_query_variables_weather_queries_QueryId",
                        column: x => x.QueryId,
                        principalTable: "weather_queries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "weather_results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    MeanValue = table.Column<float>(type: "real", nullable: true),
                    MaxValue = table.Column<float>(type: "real", nullable: true),
                    MinValue = table.Column<float>(type: "real", nullable: true),
                    ProbabilityExtreme = table.Column<float>(type: "real", nullable: false),
                    ExtremeCondition = table.Column<float>(type: "real", nullable: false),
                    TimeRange = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    QueryId = table.Column<Guid>(type: "uuid", nullable: false),
                    VariableId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weather_results_WeatherVariables_VariableId",
                        column: x => x.VariableId,
                        principalTable: "WeatherVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_weather_results_weather_queries_QueryId",
                        column: x => x.QueryId,
                        principalTable: "weather_queries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_weather_queries_LocationId",
                table: "weather_queries",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_weather_queries_UserId",
                table: "weather_queries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_weather_query_variables_QueryId",
                table: "weather_query_variables",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_weather_query_variables_VariableId",
                table: "weather_query_variables",
                column: "VariableId");

            migrationBuilder.CreateIndex(
                name: "IX_weather_results_QueryId",
                table: "weather_results",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_weather_results_VariableId",
                table: "weather_results",
                column: "VariableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weather_query_variables");

            migrationBuilder.DropTable(
                name: "weather_results");

            migrationBuilder.DropTable(
                name: "WeatherVariables");

            migrationBuilder.DropTable(
                name: "weather_queries");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
