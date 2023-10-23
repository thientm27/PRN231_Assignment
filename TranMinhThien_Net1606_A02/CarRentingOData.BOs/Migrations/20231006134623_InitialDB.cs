using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentingOData.BOs.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarProducers",
                columns: table => new
                {
                    ProducerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProducerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarProducers", x => x.ProducerID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdentityCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarModelYear = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProducerID = table.Column<int>(type: "int", nullable: false),
                    RentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarID);
                    table.ForeignKey(
                        name: "FK_Cars_CarProducers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "CarProducers",
                        principalColumn: "ProducerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarRentals",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRentals", x => new { x.CustomerID, x.CarID, x.PickupDate });
                    table.ForeignKey(
                        name: "FK_CarRentals_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "CarID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarRentals_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    ReviewStar = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => new { x.CustomerID, x.CarID });
                    table.ForeignKey(
                        name: "FK_Reviews_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "CarID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CarProducers",
                columns: new[] { "ProducerID", "Address", "Country", "ProducerName" },
                values: new object[,]
                {
                    { 1, "Address1", "Country1", "Producer1" },
                    { 2, "Address2", "Country2", "Producer2" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "Birthday", "CustomerName", "Email", "IdentityCard", "LicenceDate", "LicenceNumber", "Mobile", "Password" },
                values: new object[,]
                {
                    { 1, new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "John Doe", "john.doe@example.com", "123456789", new DateTime(2005, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABC123", "1234567890", "1" },
                    { 2, new DateTime(1990, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane Smith", "jane.smith@example.com", "987654321", new DateTime(2008, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "XYZ789", "9876543210", "1" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarID", "Capacity", "CarModelYear", "CarName", "Color", "Description", "ImportDate", "ProducerID", "RentPrice", "Status" },
                values: new object[] { 1, 4, 2022, "Car1", "Red", "Description for Car1", new DateTime(2023, 10, 6, 20, 46, 23, 612, DateTimeKind.Local).AddTicks(2006), 1, 50m, "Available" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarID", "Capacity", "CarModelYear", "CarName", "Color", "Description", "ImportDate", "ProducerID", "RentPrice", "Status" },
                values: new object[] { 2, 5, 2023, "Car2", "Blue", "Description for Car2", new DateTime(2023, 10, 6, 20, 46, 23, 612, DateTimeKind.Local).AddTicks(2019), 2, 60m, "Available" });

            migrationBuilder.InsertData(
                table: "CarRentals",
                columns: new[] { "CarID", "CustomerID", "PickupDate", "RentPrice", "ReturnDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 10, 6, 20, 46, 23, 612, DateTimeKind.Local).AddTicks(2060), 100m, new DateTime(2023, 10, 9, 20, 46, 23, 612, DateTimeKind.Local).AddTicks(2061), "Completed" },
                    { 2, 2, new DateTime(2023, 10, 11, 20, 46, 23, 612, DateTimeKind.Local).AddTicks(2066), 120m, new DateTime(2023, 10, 16, 20, 46, 23, 612, DateTimeKind.Local).AddTicks(2067), "Pending" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "CarID", "CustomerID", "Comment", "ReviewStar" },
                values: new object[,]
                {
                    { 1, 1, "Good experience with this car.", 4 },
                    { 2, 2, "Excellent car! Highly recommended.", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarRentals_CarID",
                table: "CarRentals",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ProducerID",
                table: "Cars",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CarID",
                table: "Reviews",
                column: "CarID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarRentals");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CarProducers");
        }
    }
}
