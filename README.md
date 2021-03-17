# BillingApi

Configuration:
Database configuration: 
Billing\Billing.Api\appsettings.json
Currently
 - database name = OrderDB
 - using MS SQL LocalDb as database server

This section must be changed if something are different
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrdersDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}

Migrations:
Migrations has been genereted, bust must be applied to DB after DB connection
has been configured to appsettings.json file
To update database either use Visual Studio "Package manager console" and run Update-Database
fot project Billing.Api or, if dotnet tools for ef has been installed you can run command from
command promt inside Billing.Api project folder "dotnet ef database update"

Project:
Project done using VS 2019 Community Edition with .Net 5 so it must be used to run project or if Visual Studio 
is not installed then .Net 5 SDK should be installed to run it.

Using Visual Studio open solution file Billing/Billing.sln, compile it or just run project Billing.Api 
(set as default project for debugging).

Using dotnet sdk go to folder Billing/Billing.Api and run command dotnet run.

If using Visual Studio API address will be http://localhost:32538/api/order/process if using
dotnet sdk it will be http://localhost:5000/api/order/process

API address accepts POST request as follows (just example):
{
    "orderNumber": "a3f7dd33-5e3e-450c-b421-64db3487503f",
    "userId": 1,
    "payableAmount": 100,
    "paymentGateway": 1,
    "description": ""
}

Parameters: 
 - orderNumber - order number. When running app first time app will create some Order intries. Correct order number
can be found at table Orders (select * from Orders;).
 - userId - user id. Also app will insert one, but for fresh DB it will be 1. Can check at Users table
 - payableAmount - amount to pay. Must be equal to Order amount. Can check at Orders table.
 - description - optional description  
 
Return:
 - If wrong request format, will return HTTP status code 400 Bad request
 - If there are error in input parameters will return HTTP status code 422 and error description about parameter
 - If error occured while processing order billing will return HTTP status code 500
 - If is ok will return HTTP status code 200
Also will return JSON data:
{
	"receipt": "JVBERi0....",
    "hasErrors": false,
    "errorMessage": ""
}
 - receipt - if all is ok will contain byte array of receipt in PDF format
 - hasErrors - true, if there are errors, false otherwise
 - errorMessage - if hasErrors then error message empty string otherwise
 
Sample receipt:
Simple sample receipt in file SampleReceipt.pdf