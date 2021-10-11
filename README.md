
# Alinta.CustomersApi

Alinta Energy's Customers API contains 5 endpoints, to create, update, delete and read individual Customer entities as well as search by name.

Customers API project is using an in-memory database which could be changed to use SQL Server or Azure SQL if needed.

The API architecture is broken down into a service and a repository layer to maximize testability and troubleshooting.

Unit/Integration tests for the Service and Controller layer could be found in the following project:
Alinta.CustomersApi.Tests

**Swagger** - Please use the following endpoint to view examples and schemas in dev version:
https://localhost:44366/swagger/index.html

**Create Customer** - Body: { FirstName, LastName, DateOfBirth }
[POST] https://localhost:44366/api/v1/customers

**Update Customer** - Body: { Id, FirstName, LastName, DateOfBirth }
[PATCH] https://localhost:44366/api/v1/customers/1

**Delete Customer**
[DELETE] https://localhost:44366/api/v1/customers/1

**Get Customer By Id**
[GET] https://localhost:44366/api/v1/customers/1

**Search Customers By Name**
[GET] https://localhost:44366/api/v1/customers/searchByName/jane

**Build**

    dotnet build

**Run**

    dotnet run --project Alinta.CustomersApi

**Run tests**

    dotnet test