using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nostrfi.Migrations
{
    /// <inheritdoc />
    public partial class NIP01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "nostrfi");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "events",
                schema: "nostrfi",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    public_key = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    kind = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    signature = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "nostrfi",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    event_id = table.Column<string>(type: "text", nullable: false),
                    tag = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    EventsId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_tags_events_EventsId",
                        column: x => x.EventsId,
                        principalSchema: "nostrfi",
                        principalTable: "events",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tags_EventsId",
                schema: "nostrfi",
                table: "tags",
                column: "EventsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tags",
                schema: "nostrfi");

            migrationBuilder.DropTable(
                name: "events",
                schema: "nostrfi");
        }
    }
}
