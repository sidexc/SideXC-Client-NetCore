using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SideXC.WebUI.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "MaterialTypes");

            migrationBuilder.DropColumn(
                name: "MaximunRange",
                table: "MaterialTypes");

            migrationBuilder.DropColumn(
                name: "MinimunRange",
                table: "MaterialTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdjustment",
                table: "TransactionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaterialTypeCostId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MaterialTypeCosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MinimunRange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximunRange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypeCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialTypeCosts_ClientUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ClientUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialTypeCosts_ClientUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ClientUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeCostId",
                table: "Materials",
                column: "MaterialTypeCostId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypeCosts_CreatedById",
                table: "MaterialTypeCosts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypeCosts_ModifiedById",
                table: "MaterialTypeCosts",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterialTypeCosts_MaterialTypeCostId",
                table: "Materials",
                column: "MaterialTypeCostId",
                principalTable: "MaterialTypeCosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterialTypeCosts_MaterialTypeCostId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "MaterialTypeCosts");

            migrationBuilder.DropIndex(
                name: "IX_Materials_MaterialTypeCostId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "IsAdjustment",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "MaterialTypeCostId",
                table: "Materials");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "MaterialTypes",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MaximunRange",
                table: "MaterialTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimunRange",
                table: "MaterialTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
