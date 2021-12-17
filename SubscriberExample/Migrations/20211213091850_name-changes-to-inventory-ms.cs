using Microsoft.EntityFrameworkCore.Migrations;

namespace SubscriberExample.Migrations
{
    public partial class namechangestoinventoryms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "itemID",
                table: "Inventory",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Inventory",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Inventory",
                newName: "itemID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Inventory",
                newName: "ID");
        }
    }
}
