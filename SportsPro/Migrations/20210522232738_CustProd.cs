using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsPro.Migrations
{
    public partial class CustProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustProds",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustProds", x => new { x.CustomerID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_CustProds_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustProds_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CustProds",
                columns: new[] { "CustomerID", "ProductID" },
                values: new object[,]
                {
                    { 1002, 1 },
                    { 1004, 2 },
                    { 1006, 1 },
                    { 1008, 3 },
                    { 1010, 4 },
                    { 1012, 5 },
                    { 1015, 5 },
                    { 1002, 2 },
                    { 1002, 3 },
                    { 1004, 3 },
                    { 1006, 3 },
                    { 1008, 4 },
                    { 1010, 5 },
                    { 1012, 3 },
                    { 1015, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustProds_ProductID",
                table: "CustProds",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustProds");
        }
    }
}
