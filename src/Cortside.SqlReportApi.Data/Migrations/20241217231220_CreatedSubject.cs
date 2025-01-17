using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortside.SqlReportApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatedSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_ReportGroup_ReportGroupId",
                schema: "dbo",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportArgument_Report_ReportId",
                schema: "dbo",
                table: "ReportArgument");

            migrationBuilder.AlterColumn<string>(
                name: "UserPrincipalName",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "Username (upn claim)",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "Subject primary key",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GivenName",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "Subject primary key",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FamilyName",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "Subject Surname ()",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "Subject",
                type: "datetime2",
                nullable: false,
                comment: "Date and time entity was created",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                schema: "dbo",
                table: "Subject",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Subject primary key",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ReportGroup_ReportGroupId",
                schema: "dbo",
                table: "Report",
                column: "ReportGroupId",
                principalSchema: "dbo",
                principalTable: "ReportGroup",
                principalColumn: "ReportGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportArgument_Report_ReportId",
                schema: "dbo",
                table: "ReportArgument",
                column: "ReportId",
                principalSchema: "dbo",
                principalTable: "Report",
                principalColumn: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_ReportGroup_ReportGroupId",
                schema: "dbo",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportArgument_Report_ReportId",
                schema: "dbo",
                table: "ReportArgument");

            migrationBuilder.AlterColumn<string>(
                name: "UserPrincipalName",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Username (upn claim)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Subject primary key");

            migrationBuilder.AlterColumn<string>(
                name: "GivenName",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Subject primary key");

            migrationBuilder.AlterColumn<string>(
                name: "FamilyName",
                schema: "dbo",
                table: "Subject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Subject Surname ()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "Subject",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Date and time entity was created");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                schema: "dbo",
                table: "Subject",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Subject primary key");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ReportGroup_ReportGroupId",
                schema: "dbo",
                table: "Report",
                column: "ReportGroupId",
                principalSchema: "dbo",
                principalTable: "ReportGroup",
                principalColumn: "ReportGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportArgument_Report_ReportId",
                schema: "dbo",
                table: "ReportArgument",
                column: "ReportId",
                principalSchema: "dbo",
                principalTable: "Report",
                principalColumn: "ReportId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
