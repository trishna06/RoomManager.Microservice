using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomManager.Infrastructure.Migrations
{
    public partial class INITIAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RM_ROOM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CREATED_DATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CREATED_BY_USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MODIFIED_DATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_BY = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MODIFIED_BY_USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RM_ROOM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RM_ROOM_AVAILABILITY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROOM_LINK_ID = table.Column<int>(type: "int", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CREATED_DATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CREATED_BY_USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MODIFIED_DATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_BY = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MODIFIED_BY_USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RM_ROOM_AVAILABILITY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RM_ROOM_AVAILABILITY_RM_ROOM_ROOM_LINK_ID",
                        column: x => x.ROOM_LINK_ID,
                        principalTable: "RM_ROOM",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RM_ROOM_AVAILABILITY_ROOM_LINK_ID",
                table: "RM_ROOM_AVAILABILITY",
                column: "ROOM_LINK_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RM_ROOM_AVAILABILITY");

            migrationBuilder.DropTable(
                name: "RM_ROOM");
        }
    }
}
