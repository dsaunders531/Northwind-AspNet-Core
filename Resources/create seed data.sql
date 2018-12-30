-- new CategoryDbModel() { RowId = 1, CategoryName = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales", Picture = null }
select ' new CategoryDbModel() { RowId = ' + CONVERT(nvarchar(12), Categories.CategoryID) +
		+ ', CategoryName = ' + CHAR(34) + Categories.CategoryName + CHAR(34) 
		+ ', Description = ' + CHAR(34) + CONVERT(nvarchar(max), Categories.[Description]) + CHAR(34)
		+ ', Picture = null }, '
from Categories

--new CustomerDbModel() { RowId = 1, Address = "", City = "", CompanyName = "", ContactName = "", ContactTitle = "", Country = "", CustomerKey = "",  Fax = "", Phone = "", PostalCode = "", Region = "" }
select 
	'new CustomerDbModel() { RowId = ' + CONVERT(nvarchar(12), ROW_NUMBER() OVER(ORDER BY CustomerID ASC)) 
	+ ', Address = ' + ISNULL(CHAR(34) + Customers.[Address] + CHAR(34), 'null') + ', City = ' + ISNULL(CHAR(34) + City + CHAR(34), 'null') 
	+ ', CompanyName = ' + ISNULL(CHAR(34) + CompanyName + CHAR(34), 'null')  + ', ContactName = ' + ISNULL(CHAR(34) + ContactTitle + CHAR(34), 'null') 
	+ ', Country = ' +  ISNULL(CHAR(34) + Country + CHAR(34), 'null')  + ', CustomerKey = ' + ISNULL(CHAR(34) + CustomerID + CHAR(34), 'null') 
	+ ', Fax = ' + ISNULL(CHAR(34) + Fax + CHAR(34), 'null') + ', Phone = ' + ISNULL(CHAR(34) + Phone + Char(34), 'null') 
	+ ', PostalCode = ' + ISNULL(CHAR(34) + PostalCode  + CHAR(34), 'null') + ', Region = ' + ISNULL(CHAR(34) + Region + CHAR(34), 'null') 
	+ '},'
from Customers

-- new EmployeeDbModel() { RowId = 1, Address = "", BirthDate = DateTime.Now, City = "", PhotoPath = "", Region = "", Country = "", Extension = "", FirstName = "", HireDate = DateTime.Now, 
--	HomePhone = "", LastName = "", Notes = "", PostalCode = "", ReportsTo = 1, Title = "", TitleOfCourtesy = ""}
select 
	'new EmployeeDbModel() { '
	+ 'RowId = ' + CONVERT(nvarchar(12), Employees.EmployeeID) + ', Address = ' + ISNULL(CHAR(34) + Employees.[Address] + CHAR(34), 'null')
	+ ', BirthDate = new DateTime(' + FORMAT(BirthDate, 'yyyy, MM, dd') + '), City = ' + ISNULL(CHAR(34) + City + CHAR(34), 'null') 
	+ ', PhotoPath = ' + ISNULL(CHAR(34) + PhotoPath + CHAR(34), 'null') + ', Region = ' + ISNULL(CHAR(34) + Region + CHAR(34), 'null')
	+ ', Country = ' + ISNULL(CHAR(34) + Country + CHAR(34), 'null') + ', Extension = ' + ISNULL(CHAR(34) + Extension + CHAR(34), 'null')
	+ ', FirstName = ' + ISNULL(CHAR(34) + FirstName + CHAR(34), 'null') + ', HireDate = new DateTime(' + FORMAT(HireDate, 'yyyy, MM, dd')
	+ '), HomePhone = ' + ISNULL(CHAR(34) + HomePhone + CHAR(34), 'null') + ', LastName = ' + ISNULL(CHAR(34) + LastName + CHAR(34), 'null')
	+ ', Notes = ' + ISNULL(CHAR(34) + CONVERT(nvarchar(max), Notes) + CHAR(34), 'null') + ', PostalCode = ' + ISNULL(CHAR(34) + PostalCode + CHAR(34), 'null')
	+ ', ReportsTo = ' + ISNULL(CONVERT(nvarchar(12), ReportsTo), 'null') + ', Title = ' + ISNULL(CHAR(34) + Title + CHAR(34), 'null')
	+ ', TitleOfCourtesy = ' + ISNULL(CHAR(34) + TitleOfCourtesy + CHAR(34), 'null')
	+ '},' 
from Employees

-- new SupplierDbModel() { RowId = 1, Address = "", City = "", CompanyName = "", ContactName = "", ContactTitle = "", Country = "", 
-- Fax = "", HomePage = "", Phone = "", PostalCode = "", Region = "" }
select 
 'new SupplierDbModel() { ' 
 + 'RowId = ' + CONVERT(nvarchar(12), SupplierID) + ', Address = ' + ISNULL(CHAR(34) + Suppliers.[Address] + CHAR(34), 'null')
 + ', City = ' + ISNULL(CHAR(34) + City + CHAR(34), 'null') + ', CompanyName = ' + ISNULL(CHAR(34) + CompanyName + CHAR(34), 'null')
 + ', ContactName = ' + ISNULL(CHAR(34) + ContactName + CHAR(34), 'null') + ', ContactTitle = ' + ISNULL(CHAR(34) + ContactTitle + CHAR(34), 'null')
 + ', Country = ' + ISNULL(CHAR(34) + Fax + CHAR(34), 'null') + ', HomePage = ' + ISNULL(CHAR(34) + CONVERT(nvarchar(max), HomePage) + CHAR(34), 'null')
 + ', Phone = ' + ISNULL(CHAR(34) + Phone + CHAR(34), 'null') + ', PostalCode = ' + ISNULL(CHAR(34) + PostalCode + CHAR(34), 'null')
 + ', Region = ' + ISNULL(CHAR(34) + Region + CHAR(34), 'null')
 + '},'
from Suppliers

-- new ProductDbModel() { RowId = 1, CategoryId = 1, Discontinued = false, ProductName = "", 
-- QuantityPerUnit = "", ReorderLevel = 1, SupplierId = 1, UnitPrice = 50.00m, UnitsInStock = 1, UnitsOnOrder = 12 }
select
'new ProductDbModel() {' 
+ 'RowId = ' + CONVERT(nvarchar(12), ProductID) + ', CategoryId = ' + CONVERT(nvarchar(12), ISNULL(CategoryID, 1))
+ ', Discontinued = ' + CASE WHEN Discontinued = 1 THEN 'true' ELSE 'false' END + ', ProductName = ' + ISNULL(CHAR(34) + ProductName + CHAR(34), 'null')
+ ', QuantityPerUnit = ' + ISNULL(CHAR(34) + QuantityPerUnit + CHAR(34), 'null') + ', ReorderLevel = ' + CONVERT(nvarchar(12), ReorderLevel)
+ ', SupplierId = ' + CONVERT(nvarchar(12), SupplierId) + ', UnitPrice = ' + FORMAT(UnitPrice, '0.00')
+ 'm, UnitsInStock = ' + CONVERT(nvarchar(12), UnitsInStock) + ', UnitsOnOrder = ' + CONVERT(nvarchar(12), UnitsOnOrder)
+ '},'
from Products

-- new ShipperDbModel() { RowId = 1, CompanyName = "", Phone = ""}
select 
'new ShipperDbModel() {'
+ 'RowId = ' + CONVERT(nvarchar(12), ShipperId) + ', CompanyName = ' + ISNULL(CHAR(34) + CompanyName + CHAR(34), 'null')
+ ', Phone = ' + ISNULL(CHAR(34) + Phone + CHAR(34), 'null')
+ '},'
from Shippers

-- new RegionDbModel() { RowId = 1, RegionDescription = ""}
select 
'new RegionDbModel() { '
+ 'RowId = ' + CONVERT(nvarchar(12), RegionId) + ', RegionDescription = ' + ISNULL(CHAR(34) + (RegionDescription) + CHAR(34), 'null')
+ '},'
from Region

-- new TerritoryDbModel() { RowId = 1, RegionId = 1, TerritoryDescription = ""}
select
'new TerritoryDbModel () {'
+ ' RowId = ' + CONVERT(nvarchar(12), ROW_NUMBER() OVER(ORDER BY TerritoryID ASC)) + ','
+ ' RegionId = ' + CONVERT(nvarchar(12), RegionID) + ', TerritoryDescription = ' + ISNULL(CHAR(34) + TerritoryDescription + CHAR(34), 'null')
+ ', TerritoryId = ' + ISNULL(CHAR(34) + TerritoryID + CHAR(34), 'null')
+ '},'
from Territories

-- new EmployeeTerritoryDbModel() { RowId = 1, EmployeeId = 1, TerritoryId = 2}
select
'new EmployeeTerritoryDbModel() {'
--+ ' RowId = ' + CONVERT(nvarchar(12), ROW_NUMBER() OVER(ORDER BY EmployeeTerritories.TerritoryID ASC)) + ','
+ ' EmployeeId = ' + CONVERT(nvarchar(12), EmployeeTerritories.EmployeeId) + ', TerritoryId = ' + t.ROWID
+ '},'
from EmployeeTerritories 
LEFT OUTER JOIN 
(SELECT CONVERT(nvarchar(12), ROW_NUMBER() OVER(ORDER BY TerritoryID ASC)) ROWID, TerritoryId FROM Territories) t
ON EmployeeTerritories.TerritoryID = t.TerritoryID
ORDER BY EmployeeTerritories.TerritoryID

-- new OrderDbModel() { RowId = 1, CustomerId = 1, EmployeeId = 1, 
-- Freight = 0.00m, OrderDate = new DateTime(2001, 1, 1), RequiredDate = new DateTime(2001, 12, 31), 
-- ShipAddress = "", ShipCity = "", ShipCountry = "", ShipName = "", ShippedDate = new DateTime(2001, 11, 30), ShipPostalCode = "", 
-- ShipRegion = "", ShipVia = 1 }
select 
'new OrderDbModel() {'
+ 'RowId = ' + CONVERT(nvarchar(12), OrderID) + ', CustomerId = ' + c.ROWID
+ ', EmployeeId = ' + CONVERT(nvarchar(12), EmployeeID) + ', Freight = ' + FORMAT(Freight, '0.00') + 'm, OrderDate = new DateTime(' + Format(OrderDate, 'yyyy, MM, dd') + ')'
+ ', RequiredDate = new DateTime(' + Format(RequiredDate, 'yyyy, MM, dd') + '), ShipAddress = ' + ISNULL(CHAR(34) + ShipAddress + CHAR(34), 'null')
+ ', ShipCity = ' + ISNULL(CHAR(34) + ShipCity + CHAR(34), 'null') + ', ShipCountry = ' + ISNULL(CHAR(34) + ShipCountry + CHAR(34), 'null')
+ ', ShipName = ' + ISNULL(CHAR(34) + ShipName + CHAR(34), 'null') + ', ShippedDate = new DateTime(' + ISNULL(Format(ShippedDate, 'yyyy, MM, dd'), 'null') + ') '
+ ', ShipPostalCode = ' + ISNULL(CHAR(34) + ShipPostalCode + CHAR(34), 'null') + ', ShipRegion = ' + ISNULL(CHAR(34) + ShipRegion + CHAR(34), 'null') + ', ShipVia = ' + CONVERT(nvarchar(12), ShipVia)
+ '},'
from Orders LEFT OUTER JOIN (SELECT CONVERT(nvarchar(12), ROW_NUMBER() OVER(ORDER BY CustomerID ASC)) ROWID, CUSTOMERID FROM Customers) c ON Orders.CustomerID = c.CustomerID
Order By Orders.CustomerID

-- new OrderDetailDbModel() { RowId = 1, OrderId = 1, Discount = 0F, ProductId = 1, Quantity = 1, UnitPrice = 1m }
select 
'new OrderDetailDbModel() {'
--+ ' RowId = ' + CONVERT(nvarchar(12), ROW_NUMBER() OVER(ORDER BY OrderId ASC)) + ',' +
+ ' OrderId = ' + CONVERT(nvarchar(12), OrderId)
+ ', Discount = ' + FORMAT(Discount, '0.00') + 'F, ProductId = ' + CONVERT(nvarchar(12), ProductId) 
+ ', Quantity = ' + CONVERT(nvarchar(12), Quantity) + ', UnitPrice = ' + FORMAT(UnitPrice, '0.00') + 'm '
+ '},'
from [Order Details]