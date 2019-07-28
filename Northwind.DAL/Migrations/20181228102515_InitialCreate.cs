using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CategoryName = table.Column<string>(maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Picture = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDemographics",
                columns: table => new
                {
                    RowId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CustomerDesc = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDemographics", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CustomerID = table.Column<string>(maxLength: 5, nullable: false),
                    CompanyName = table.Column<string>(maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(maxLength: 30, nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    Phone = table.Column<string>(maxLength: 24, nullable: true),
                    Fax = table.Column<string>(maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "CustomersHistory",
                columns: table => new
                {
                    RowId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Action = table.Column<int>(nullable: false),
                    CustomerID = table.Column<string>(maxLength: 5, nullable: false),
                    CompanyName = table.Column<string>(maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(maxLength: 30, nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    Phone = table.Column<string>(maxLength: 24, nullable: true),
                    Fax = table.Column<string>(maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersHistory", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(maxLength: 10, nullable: false),
                    Title = table.Column<string>(maxLength: 30, nullable: true),
                    TitleOfCourtesy = table.Column<string>(maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    HomePhone = table.Column<string>(maxLength: 24, nullable: true),
                    Extension = table.Column<string>(maxLength: 4, nullable: true),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    ReportsTo = table.Column<int>(nullable: true),
                    PhotoPath = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_Employees_Employees",
                        column: x => x.ReportsTo,
                        principalTable: "Employees",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesHistory",
                columns: table => new
                {
                    RowId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Action = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(maxLength: 10, nullable: false),
                    Title = table.Column<string>(maxLength: 30, nullable: true),
                    TitleOfCourtesy = table.Column<string>(maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    HomePhone = table.Column<string>(maxLength: 24, nullable: true),
                    Extension = table.Column<string>(maxLength: 4, nullable: true),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    ReportsTo = table.Column<int>(nullable: true),
                    PhotoPath = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesHistory", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "ProductsHistory",
                columns: table => new
                {
                    RowId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Action = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 40, nullable: false),
                    SupplierID = table.Column<int>(nullable: true),
                    CategoryID = table.Column<int>(nullable: true),
                    QuantityPerUnit = table.Column<string>(maxLength: 20, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
                    UnitsInStock = table.Column<short>(nullable: true, defaultValueSql: "((0))"),
                    UnitsOnOrder = table.Column<short>(nullable: true, defaultValueSql: "((0))"),
                    ReorderLevel = table.Column<short>(nullable: true, defaultValueSql: "((0))"),
                    Discontinued = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsHistory", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    RegionDescription = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "Shippers",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 40, nullable: false),
                    Phone = table.Column<string>(maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippers", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(maxLength: 30, nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    Phone = table.Column<string>(maxLength: 24, nullable: true),
                    Fax = table.Column<string>(maxLength: 24, nullable: true),
                    HomePage = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCustomerDemo",
                columns: table => new
                {
                    RowId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    CustomerTypeID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCustomerDemo", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_CustomerCustomerDemo_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CustomerCustomerDemo",
                        column: x => x.CustomerTypeID,
                        principalTable: "CustomerDemographics",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Territories",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    TerritoryId = table.Column<string>(maxLength: 10, nullable: false),
                    TerritoryDescription = table.Column<string>(maxLength: 50, nullable: false),
                    RegionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territories", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_Territories_Region",
                        column: x => x.RegionID,
                        principalTable: "Region",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    CustomerID = table.Column<int>(maxLength: 5, nullable: false),
                    EmployeeID = table.Column<int>(nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShipVia = table.Column<int>(nullable: true),
                    Freight = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
                    ShipName = table.Column<string>(maxLength: 40, nullable: true),
                    ShipAddress = table.Column<string>(maxLength: 60, nullable: true),
                    ShipCity = table.Column<string>(maxLength: 15, nullable: true),
                    ShipRegion = table.Column<string>(maxLength: 15, nullable: true),
                    ShipPostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    ShipCountry = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_Employees",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_Shippers",
                        column: x => x.ShipVia,
                        principalTable: "Shippers",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 40, nullable: false),
                    SupplierID = table.Column<int>(nullable: true),
                    CategoryID = table.Column<int>(nullable: true),
                    QuantityPerUnit = table.Column<string>(maxLength: 20, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: true, defaultValueSql: "((0))"),
                    UnitsInStock = table.Column<short>(nullable: true, defaultValueSql: "((0))"),
                    UnitsOnOrder = table.Column<short>(nullable: true, defaultValueSql: "((0))"),
                    ReorderLevel = table.Column<short>(nullable: true, defaultValueSql: "((0))"),
                    Discontinued = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_Products_Categories",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTerritories",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: false),
                    TerritoryID = table.Column<int>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTerritories", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_EmployeeTerritories_Employees",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmployeeTerritories_Territories",
                        column: x => x.TerritoryID,
                        principalTable: "Territories",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Order Details",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyToken = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RowVersion = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    OrderID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    Quantity = table.Column<short>(nullable: false, defaultValueSql: "((1))"),
                    Discount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order Details", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_Order_Details_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_Details_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "CategoryName",
                table: "Categories",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomerDemo_CustomerID",
                table: "CustomerCustomerDemo",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomerDemo_CustomerTypeID",
                table: "CustomerCustomerDemo",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "City",
                table: "Customers",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "CompanyName",
                table: "Customers",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "PostalCode",
                table: "Customers",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "Region",
                table: "Customers",
                column: "Region");

            migrationBuilder.CreateIndex(
                name: "City",
                table: "CustomersHistory",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "CompanyName",
                table: "CustomersHistory",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "PostalCode",
                table: "CustomersHistory",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "Region",
                table: "CustomersHistory",
                column: "Region");

            migrationBuilder.CreateIndex(
                name: "LastName",
                table: "Employees",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "PostalCode",
                table: "Employees",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReportsTo",
                table: "Employees",
                column: "ReportsTo");

            migrationBuilder.CreateIndex(
                name: "LastName",
                table: "EmployeesHistory",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "PostalCode",
                table: "EmployeesHistory",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTerritories_EmployeeID",
                table: "EmployeeTerritories",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTerritories_TerritoryID",
                table: "EmployeeTerritories",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Order Details_OrderID",
                table: "Order Details",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Order Details_ProductID",
                table: "Order Details",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "CustomersOrders",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "EmployeesOrders",
                table: "Orders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "ShipPostalCode",
                table: "Orders",
                column: "ShipPostalCode");

            migrationBuilder.CreateIndex(
                name: "ShippersOrders",
                table: "Orders",
                column: "ShipVia");

            migrationBuilder.CreateIndex(
                name: "ShippedDate",
                table: "Orders",
                column: "ShippedDate");

            migrationBuilder.CreateIndex(
                name: "CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "ProductName",
                table: "Products",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "SuppliersProducts",
                table: "Products",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "CategoryID",
                table: "ProductsHistory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "ProductName",
                table: "ProductsHistory",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "SuppliersProducts",
                table: "ProductsHistory",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "CompanyName",
                table: "Suppliers",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "PostalCode",
                table: "Suppliers",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Territories_RegionID",
                table: "Territories",
                column: "RegionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerCustomerDemo");

            migrationBuilder.DropTable(
                name: "CustomersHistory");

            migrationBuilder.DropTable(
                name: "EmployeesHistory");

            migrationBuilder.DropTable(
                name: "EmployeeTerritories");

            migrationBuilder.DropTable(
                name: "Order Details");

            migrationBuilder.DropTable(
                name: "ProductsHistory");

            migrationBuilder.DropTable(
                name: "CustomerDemographics");

            migrationBuilder.DropTable(
                name: "Territories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Shippers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
