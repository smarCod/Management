using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.Infrastructure.Department.Migrations
{
    /// <inheritdoc />
    public partial class InitialDepartmentCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostPort",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPort", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostSectionTyp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSectionTyp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DepartmentModId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostSection_PostDepartment_DepartmentModId",
                        column: x => x.DepartmentModId,
                        principalTable: "PostDepartment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostSectionSingle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SectionTypId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<decimal>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SectionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSectionSingle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostSectionSingle_PostSectionTyp_SectionTypId",
                        column: x => x.SectionTypId,
                        principalTable: "PostSectionTyp",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostSectionSingle_PostSection_SectionId",
                        column: x => x.SectionId,
                        principalTable: "PostSection",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SectionSingleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostDevice_PostSectionSingle_SectionSingleId",
                        column: x => x.SectionSingleId,
                        principalTable: "PostSectionSingle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostSectionSinglePort",
                columns: table => new
                {
                    SectionSingleId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSectionSinglePort", x => new { x.SectionSingleId, x.PortId });
                    table.ForeignKey(
                        name: "FK_PostSectionSinglePort_PostPort_PortId",
                        column: x => x.PortId,
                        principalTable: "PostPort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostSectionSinglePort_PostSectionSingle_SectionSingleId",
                        column: x => x.SectionSingleId,
                        principalTable: "PostSectionSingle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostDevice_SectionSingleId",
                table: "PostDevice",
                column: "SectionSingleId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSection_DepartmentModId",
                table: "PostSection",
                column: "DepartmentModId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSectionSingle_SectionId",
                table: "PostSectionSingle",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSectionSingle_SectionTypId",
                table: "PostSectionSingle",
                column: "SectionTypId");

            migrationBuilder.CreateIndex(
                name: "IX_PostSectionSinglePort_PortId",
                table: "PostSectionSinglePort",
                column: "PortId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostDevice");

            migrationBuilder.DropTable(
                name: "PostSectionSinglePort");

            migrationBuilder.DropTable(
                name: "PostPort");

            migrationBuilder.DropTable(
                name: "PostSectionSingle");

            migrationBuilder.DropTable(
                name: "PostSectionTyp");

            migrationBuilder.DropTable(
                name: "PostSection");

            migrationBuilder.DropTable(
                name: "PostDepartment");
        }
    }
}
