using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrAssignment.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Add_PagePermissionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PagePermissionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerType = table.Column<byte>(type: "tinyint", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetType = table.Column<byte>(type: "tinyint", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: true),
                    MenuGroupId = table.Column<short>(type: "smallint", nullable: true),
                    Action = table.Column<byte>(type: "tinyint", nullable: false),
                    OldValue = table.Column<int>(type: "int", nullable: true),
                    NewValue = table.Column<int>(type: "int", nullable: true),
                    SourcePage = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsPassived = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    RevNum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePermissionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagePermissionLogs_AppUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PagePermissionLogs_AppUser_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissionLogs_CreatedByUserId",
                table: "PagePermissionLogs",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissionLogs_CreatedDate",
                table: "PagePermissionLogs",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissionLogs_ModifiedByUserId",
                table: "PagePermissionLogs",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissionLogs_OwnerType_OwnerId",
                table: "PagePermissionLogs",
                columns: new[] { "OwnerType", "OwnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissionLogs_PageId",
                table: "PagePermissionLogs",
                column: "PageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagePermissionLogs");
        }
    }
}
