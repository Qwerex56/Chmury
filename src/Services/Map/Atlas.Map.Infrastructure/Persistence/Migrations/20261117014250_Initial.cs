using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Atlas.Map.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "map");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "species",
                schema: "map",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PolishName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    LatinName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Edibility = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SeasonFrom = table.Column<int>(type: "integer", nullable: false),
                    SeasonTo = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "findings",
                schema: "map",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesId = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Location = table.Column<Point>(type: "geometry(Point,4326)", nullable: false),
                    FoundAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_findings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_findings_species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalSchema: "map",
                        principalTable: "species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_findings_FoundAt",
                schema: "map",
                table: "findings",
                column: "FoundAt");

            migrationBuilder.CreateIndex(
                name: "IX_findings_Location",
                schema: "map",
                table: "findings",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_findings_SpeciesId",
                schema: "map",
                table: "findings",
                column: "SpeciesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "findings",
                schema: "map");

            migrationBuilder.DropTable(
                name: "species",
                schema: "map");
        }
    }
}
