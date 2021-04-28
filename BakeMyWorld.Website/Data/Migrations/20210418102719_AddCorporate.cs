using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeMyWorld.Website.Migrations
{
    public partial class AddCorporate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Corporates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlSlug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiftBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UrlSlug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftBoxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CorporateGiftBox",
                columns: table => new
                {
                    CorporatesId = table.Column<int>(type: "int", nullable: false),
                    GiftBoxesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateGiftBox", x => new { x.CorporatesId, x.GiftBoxesId });
                    table.ForeignKey(
                        name: "FK_CorporateGiftBox_Corporates_CorporatesId",
                        column: x => x.CorporatesId,
                        principalTable: "Corporates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporateGiftBox_GiftBoxes_GiftBoxesId",
                        column: x => x.GiftBoxesId,
                        principalTable: "GiftBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorporateGiftBox_GiftBoxesId",
                table: "CorporateGiftBox",
                column: "GiftBoxesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorporateGiftBox");

            migrationBuilder.DropTable(
                name: "Corporates");

            migrationBuilder.DropTable(
                name: "GiftBoxes");
        }
    }
}
