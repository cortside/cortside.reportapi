using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cortside.SqlReportApi.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "dbo",
                columns: table => new
                {
                    PermissionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "ReportArgumentQuery",
                schema: "dbo",
                columns: table => new
                {
                    ReportArgumentQueryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArgQuery = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportArgumentQuery", x => x.ReportArgumentQueryId);
                });

            migrationBuilder.CreateTable(
                name: "ReportGroup",
                schema: "dbo",
                columns: table => new
                {
                    ReportGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportGroup", x => x.ReportGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                schema: "dbo",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    GivenName = table.Column<string>(maxLength: 100, nullable: true),
                    FamilyName = table.Column<string>(maxLength: 100, nullable: true),
                    UserPrincipalName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                schema: "dbo",
                columns: table => new
                {
                    ReportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReportGroupId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "dbo",
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_ReportGroup_ReportGroupId",
                        column: x => x.ReportGroupId,
                        principalSchema: "dbo",
                        principalTable: "ReportGroup",
                        principalColumn: "ReportGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SqlReportApi",
                schema: "dbo",
                columns: table => new
                {
                    SqlReportApiId = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreateSubjectId = table.Column<Guid>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedSubjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlReportApi", x => x.SqlReportApiId);
                    table.ForeignKey(
                        name: "FK_SqlReportApi_Subject_CreateSubjectId",
                        column: x => x.CreateSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlReportApi_Subject_LastModifiedSubjectId",
                        column: x => x.LastModifiedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportArgument",
                schema: "dbo",
                columns: table => new
                {
                    ReportArgumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ArgName = table.Column<string>(nullable: true),
                    ArgType = table.Column<string>(nullable: true),
                    ReportArgumentQueryId = table.Column<int>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportArgument", x => x.ReportArgumentId);
                    table.ForeignKey(
                        name: "FK_ReportArgument_ReportArgumentQuery_ReportArgumentQueryId",
                        column: x => x.ReportArgumentQueryId,
                        principalSchema: "dbo",
                        principalTable: "ReportArgumentQuery",
                        principalColumn: "ReportArgumentQueryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportArgument_Report_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "dbo",
                        principalTable: "Report",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Report_PermissionId",
                schema: "dbo",
                table: "Report",
                column: "PermissionId");

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

            migrationBuilder.CreateIndex(
                name: "IX_SqlReportApi_CreateSubjectId",
                schema: "dbo",
                table: "SqlReportApi",
                column: "CreateSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SqlReportApi_LastModifiedSubjectId",
                schema: "dbo",
                table: "SqlReportApi",
                column: "LastModifiedSubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportArgument",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SqlReportApi",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReportArgumentQuery",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Report",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Subject",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReportGroup",
                schema: "dbo");
        }
    }
}
