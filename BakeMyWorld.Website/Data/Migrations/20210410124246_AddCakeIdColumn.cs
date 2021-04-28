using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeMyWorld.Website.Migrations
{
    public partial class AddCakeIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLine_Cakes_CakeId",
                table: "OrderLine");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Customers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "CakeId",
                table: "OrderLine",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLine_Cakes_CakeId",
                table: "OrderLine",
                column: "CakeId",
                principalTable: "Cakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLine_Cakes_CakeId",
                table: "OrderLine");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Customers",
                newName: "CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "CakeId",
                table: "OrderLine",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLine_Cakes_CakeId",
                table: "OrderLine",
                column: "CakeId",
                principalTable: "Cakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
