using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Core.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "UMS_mstUser",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_UMS_mstUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UMS_mstUserAccessGroup",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppSystemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppSystemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppSystemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppProjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppProjectCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyInitial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_UMS_mstUserAccessGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UMS_sysToken",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_UMS_sysToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UMS_mstUserAccess",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_UMS_mstUserAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UMS_mstUserAccess_UMS_mstUserAccessGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalSchema: "dbo",
                        principalTable: "UMS_mstUserAccessGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UMS_mstUserAccess_UMS_mstUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "UMS_mstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UMS_mstUserAccessGroupModule",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppModuleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppModuleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppModuleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserAccessGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_UMS_mstUserAccessGroupModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UMS_mstUserAccessGroupModule_UMS_mstUserAccessGroup_AppUserAccessGroupId",
                        column: x => x.AppUserAccessGroupId,
                        principalSchema: "dbo",
                        principalTable: "UMS_mstUserAccessGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UMS_mstUserAccessGroupModuleAccess",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppAccessId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppAccessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppAccessDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserAccessGroupModuleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_UMS_mstUserAccessGroupModuleAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UMS_mstUserAccessGroupModuleAccess_UMS_mstUserAccessGroupModule_AppUserAccessGroupModuleId",
                        column: x => x.AppUserAccessGroupModuleId,
                        principalSchema: "dbo",
                        principalTable: "UMS_mstUserAccessGroupModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UMS_mstUserAccess_UserGroupId",
                schema: "dbo",
                table: "UMS_mstUserAccess",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UMS_mstUserAccess_UserId",
                schema: "dbo",
                table: "UMS_mstUserAccess",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UMS_mstUserAccessGroupModule_AppUserAccessGroupId",
                schema: "dbo",
                table: "UMS_mstUserAccessGroupModule",
                column: "AppUserAccessGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UMS_mstUserAccessGroupModuleAccess_AppUserAccessGroupModuleId",
                schema: "dbo",
                table: "UMS_mstUserAccessGroupModuleAccess",
                column: "AppUserAccessGroupModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UMS_mstUserAccess",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UMS_mstUserAccessGroupModuleAccess",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UMS_sysToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UMS_mstUser",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UMS_mstUserAccessGroupModule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UMS_mstUserAccessGroup",
                schema: "dbo");
        }
    }
}
