using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniLibrary.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "books",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    author = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    isbn = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    copies_available = table.Column<int>(type: "integer", nullable: false),
                    published_year = table.Column<int>(type: "integer", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "members",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    join_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_members", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "borrows",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    borrow_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_borrows", x => x.id);
                    table.ForeignKey(
                        name: "fk_borrows_members_member_id",
                        column: x => x.member_id,
                        principalSchema: "public",
                        principalTable: "members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "borrow_items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    borrow_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    return_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_borrow_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_borrow_items_books_book_id",
                        column: x => x.book_id,
                        principalSchema: "public",
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_borrow_items_borrows_borrow_id",
                        column: x => x.borrow_id,
                        principalSchema: "public",
                        principalTable: "borrows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_books_isbn",
                schema: "public",
                table: "books",
                column: "isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_borrow_items_book_id",
                schema: "public",
                table: "borrow_items",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "ix_borrow_items_borrow_id",
                schema: "public",
                table: "borrow_items",
                column: "borrow_id");

            migrationBuilder.CreateIndex(
                name: "ix_borrows_member_id",
                schema: "public",
                table: "borrows",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "ix_members_email",
                schema: "public",
                table: "members",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "borrow_items",
                schema: "public");

            migrationBuilder.DropTable(
                name: "books",
                schema: "public");

            migrationBuilder.DropTable(
                name: "borrows",
                schema: "public");

            migrationBuilder.DropTable(
                name: "members",
                schema: "public");
        }
    }
}
