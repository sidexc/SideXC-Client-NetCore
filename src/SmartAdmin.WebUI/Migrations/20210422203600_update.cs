using Microsoft.EntityFrameworkCore.Migrations;

namespace SideXC.WebUI.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Materials_materialId",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "materialId",
                table: "Inventories",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_materialId",
                table: "Inventories",
                newName: "IX_Inventories_MaterialId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Materials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Materials",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "Materials",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Materials_MaterialId",
                table: "Inventories",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Materials_MaterialId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "Inventories",
                newName: "materialId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_MaterialId",
                table: "Inventories",
                newName: "IX_Inventories_materialId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Materials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Materials",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Materials_materialId",
                table: "Inventories",
                column: "materialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
