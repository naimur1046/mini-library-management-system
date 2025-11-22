using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniLibrary.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailLogEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "email_logs",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipient_email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    sent_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_success = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    error_message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    email_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    related_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_logs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_email_logs_email_type",
                schema: "public",
                table: "email_logs",
                column: "email_type");

            migrationBuilder.CreateIndex(
                name: "ix_email_logs_recipient_email",
                schema: "public",
                table: "email_logs",
                column: "recipient_email");

            migrationBuilder.CreateIndex(
                name: "ix_email_logs_sent_date",
                schema: "public",
                table: "email_logs",
                column: "sent_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "email_logs",
                schema: "public");
        }
    }
}
