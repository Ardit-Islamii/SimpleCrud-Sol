using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleCrud.Migrations
{
    public partial class namechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Items_itemID",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "itemID",
                table: "Purchases",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Purchases",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_itemID",
                table: "Purchases",
                newName: "IX_Purchases_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Items_ItemId",
                table: "Purchases",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Items_ItemId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Purchases",
                newName: "itemID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Purchases",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_ItemId",
                table: "Purchases",
                newName: "IX_Purchases_itemID");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Items_itemID",
                table: "Purchases",
                column: "itemID",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
