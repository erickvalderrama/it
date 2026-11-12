using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petyei.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCatalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "catalog_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    entity_key = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    field = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    locale = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_catalog_translations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    code = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    country_code = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.id);
                    table.ForeignKey(
                        name: "fk_departments_countries_country_code",
                        column: x => x.country_code,
                        principalTable: "countries",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_breeds_species_species_id",
                        column: x => x.species_id,
                        principalTable: "species",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "localities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country_code = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    locality_type = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    is_selectable = table.Column<bool>(type: "boolean", nullable: false),
                    source_version = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localities", x => x.id);
                    table.ForeignKey(
                        name: "fk_localities_countries_country_code",
                        column: x => x.country_code,
                        principalTable: "countries",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_localities_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_id_code",
                table: "breeds",
                columns: new[] { "species_id", "code" },
                unique: true,
                filter: "state <> 2");

            migrationBuilder.CreateIndex(
                name: "ix_breeds_state",
                table: "breeds",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_catalog_translations_entity_type_entity_key_field_locale",
                table: "catalog_translations",
                columns: new[] { "entity_type", "entity_key", "field", "locale" },
                unique: true,
                filter: "state <> 2");

            migrationBuilder.CreateIndex(
                name: "ix_catalog_translations_entity_type_locale",
                table: "catalog_translations",
                columns: new[] { "entity_type", "locale" });

            migrationBuilder.CreateIndex(
                name: "ix_catalog_translations_state",
                table: "catalog_translations",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_countries_is_enabled",
                table: "countries",
                column: "is_enabled");

            migrationBuilder.CreateIndex(
                name: "ix_countries_state",
                table: "countries",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_departments_country_code_code",
                table: "departments",
                columns: new[] { "country_code", "code" },
                unique: true,
                filter: "state <> 2");

            migrationBuilder.CreateIndex(
                name: "ix_departments_state",
                table: "departments",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_localities_country_code_code",
                table: "localities",
                columns: new[] { "country_code", "code" },
                unique: true,
                filter: "state <> 2");

            migrationBuilder.CreateIndex(
                name: "ix_localities_country_code_is_selectable_department_id_name",
                table: "localities",
                columns: new[] { "country_code", "is_selectable", "department_id", "name" });

            migrationBuilder.CreateIndex(
                name: "ix_localities_department_id",
                table: "localities",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_localities_state",
                table: "localities",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_species_code",
                table: "species",
                column: "code",
                unique: true,
                filter: "state <> 2");

            migrationBuilder.CreateIndex(
                name: "ix_species_sort_order",
                table: "species",
                column: "sort_order");

            migrationBuilder.CreateIndex(
                name: "ix_species_state",
                table: "species",
                column: "state");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breeds");

            migrationBuilder.DropTable(
                name: "catalog_translations");

            migrationBuilder.DropTable(
                name: "localities");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
