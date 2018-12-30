using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.DAL.Migrations
{
    public partial class SetRowDefaults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Territories",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Territories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Suppliers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Suppliers",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Shippers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Shippers",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Region",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Region",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "ProductsHistory",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "ProductsHistory",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Products",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Products",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Order Details",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Order Details",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "EmployeeTerritories",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "EmployeeTerritories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "EmployeesHistory",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "EmployeesHistory",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "CustomersHistory",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "CustomersHistory",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "CustomerDemographics",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "CustomerDemographics",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "CustomerCustomerDemo",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "CustomerCustomerDemo",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Territories",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Territories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Suppliers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Suppliers",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Shippers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Shippers",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Region",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Region",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "ProductsHistory",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "ProductsHistory",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Products",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Products",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Order Details",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Order Details",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "EmployeeTerritories",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "EmployeeTerritories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "EmployeesHistory",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "EmployeesHistory",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "CustomersHistory",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "CustomersHistory",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "CustomerDemographics",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "CustomerDemographics",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "CustomerCustomerDemo",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "CustomerCustomerDemo",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
