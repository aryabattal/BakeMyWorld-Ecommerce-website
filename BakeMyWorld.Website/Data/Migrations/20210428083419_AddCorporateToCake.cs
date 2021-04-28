using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeMyWorld.Website.Migrations
{
    public partial class AddCorporateToCake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorporateGiftBox");

            migrationBuilder.DropTable(
                name: "GiftBoxes");

            migrationBuilder.CreateTable(
                name: "CakeCorporate",
                columns: table => new
                {
                    CakesId = table.Column<int>(type: "int", nullable: false),
                    CorporatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CakeCorporate", x => new { x.CakesId, x.CorporatesId });
                    table.ForeignKey(
                        name: "FK_CakeCorporate_Cakes_CakesId",
                        column: x => x.CakesId,
                        principalTable: "Cakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CakeCorporate_Corporates_CorporatesId",
                        column: x => x.CorporatesId,
                        principalTable: "Corporates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CakeCorporate_CorporatesId",
                table: "CakeCorporate",
                column: "CorporatesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CakeCorporate");

            migrationBuilder.CreateTable(
                name: "GiftBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
    }
}
