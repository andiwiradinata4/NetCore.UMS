using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAppRoleandUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UMS_mstRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LogBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogByUserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogInc = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UMS_mstRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UMS_mstUserRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppRoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogByUserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogInc = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UMS_mstUserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UMS_mstUserRole_UMS_mstRole_AppRoleId",
                        column: x => x.AppRoleId,
                        principalSchema: "dbo",
                        principalTable: "UMS_mstRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UMS_mstUserRole_UMS_mstUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "UMS_mstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UMS_mstUserRole_AppRoleId",
                schema: "dbo",
                table: "UMS_mstUserRole",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UMS_mstUserRole_UserId",
                schema: "dbo",
                table: "UMS_mstUserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UMS_mstUserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UMS_mstRole",
                schema: "dbo");
        }
    }
}
