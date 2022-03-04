using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cortside.SqlReportApi.Data.Migrations {
    public partial class Initial : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ReportArgumentQuery",
                schema: "dbo",
                columns: table => new {
                    ReportArgumentQueryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArgQuery = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ReportArgumentQuery", x => x.ReportArgumentQueryId);
                });

            migrationBuilder.CreateTable(
                name: "ReportGroup",
                schema: "dbo",
                columns: table => new {
                    ReportGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ReportGroup", x => x.ReportGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                schema: "dbo",
                columns: table => new {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserPrincipalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                schema: "dbo",
                columns: table => new {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReportGroupId = table.Column<int>(type: "int", nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_ReportGroup_ReportGroupId",
                        column: x => x.ReportGroupId,
                        principalSchema: "dbo",
                        principalTable: "ReportGroup",
                        principalColumn: "ReportGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportArgument",
                schema: "dbo",
                columns: table => new {
                    ReportArgumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ArgName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ArgType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReportArgumentQueryId = table.Column<int>(type: "int", nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ReportArgument", x => x.ReportArgumentId);
                    table.ForeignKey(
                        name: "FK_ReportArgument_Report_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "dbo",
                        principalTable: "Report",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportArgument_ReportArgumentQuery_ReportArgumentQueryId",
                        column: x => x.ReportArgumentQueryId,
                        principalSchema: "dbo",
                        principalTable: "ReportArgumentQuery",
                        principalColumn: "ReportArgumentQueryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Report_ReportGroupId",
                schema: "dbo",
                table: "Report",
                column: "ReportGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportArgument_ReportArgumentQueryId",
                schema: "dbo",
                table: "ReportArgument",
                column: "ReportArgumentQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportArgument_ReportId",
                schema: "dbo",
                table: "ReportArgument",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "ReportArgument",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Subject",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Report",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReportArgumentQuery",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReportGroup",
                schema: "dbo");
        }
    }
}
