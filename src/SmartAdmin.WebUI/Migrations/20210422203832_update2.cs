using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SideXC.WebUI.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SerialConsecutives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Folio = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialConsecutives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SerialConsecutives_ClientUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ClientUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SerialConsecutives_ClientUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ClientUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SerialConsecutives_CreatedById",
                table: "SerialConsecutives",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SerialConsecutives_ModifiedById",
                table: "SerialConsecutives",
                column: "ModifiedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerialConsecutives");
        }
    }
}
