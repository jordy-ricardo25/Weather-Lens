using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherLens.Migrations
{
    /// <inheritdoc />
    public partial class FixProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weather_queries_users_UserId",
                table: "weather_queries");

            migrationBuilder.DropIndex(
                name: "IX_weather_queries_UserId",
                table: "weather_queries");

            migrationBuilder.DropColumn(
                name: "DataUrl",
                table: "WeatherVariables");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "weather_results");

            migrationBuilder.DropColumn(
                name: "MeanValue",
                table: "weather_results");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "weather_results");

            migrationBuilder.DropColumn(
                name: "TimeRange",
                table: "weather_results");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "weather_queries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "weather_queries");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "locations");

            migrationBuilder.AlterColumn<string>(
                name: "ExtremeCondition",
                table: "weather_results",
                type: "text",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataUrl",
                table: "WeatherVariables",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<float>(
                name: "ExtremeCondition",
                table: "weather_results",
                type: "real",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<float>(
                name: "MaxValue",
                table: "weather_results",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MeanValue",
                table: "weather_results",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MinValue",
                table: "weather_results",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeRange",
                table: "weather_results",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "weather_queries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "weather_queries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "locations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "locations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_weather_queries_UserId",
                table: "weather_queries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_queries_users_UserId",
                table: "weather_queries",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
