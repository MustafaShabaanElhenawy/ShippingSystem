 Shipping Management System

A mini Shipping Management System built as the final training project using ASP.NET Web API, C#, SQL Server, ADO.NET, jQuery, AJAX, and Bootstrap.

 Features

* Client Management (CRUD + Search)
* Voyage Management (CRUD + Search)
* Bill Management (CRUD + Search)
* Container Management (CRUD + Search)
* Shipping Summary Report
* SQL View for reporting
* Stored Procedures for database operations
* BLL / DAL layered architecture
* Frontend & Backend validation
* AJAX API integration
* Error handling & delete confirmation
* Dynamic dropdowns for Clients, Voyages, and Bills

 Technologies

Backend

* C#
* .NET Framework 4.x
* ASP.NET Web API
* ADO.NET
* SQL Server
* Stored Procedures & Views

Frontend

* HTML
* CSS
* Bootstrap
* JavaScript
* jQuery
* AJAX

Tools

* Visual Studio
* SQL Server Management Studio
* Postman
* Git & GitHub

 Architecture

Frontend
   ↓
jQuery AJAX
   ↓
Web API
   ↓
BLL
   ↓
DAL
   ↓
SQL Server

🗄️ Database

Database: TrainingShippingDB

Main tables:

Clients
Voyages
Bills
Containers

Relationships:

Client ──→ Bill ──→ Container
Voyage ──→ Bill

 API Endpoints

/api/Clients
/api/Voyages
/api/Bills
/api/Containers

Each module supports:

GET
GET by ID
POST
PUT
DELETE
Search

 Setup

1. Create TrainingShippingDB in SQL Server.
2. Execute the SQL scripts in the SQL folder.
3. Configure the connection string in Web.config.
4. Open the solution in Visual Studio.
5. Build and run the Web API.
6. Run the frontend and make sure the API URL matches the local Visual Studio URL.
7. Use Postman to test the API endpoints.

 Bonus

Optional bonus features such as dashboard statistics, advanced search, pagination, sorting, and loading indicators are not implemented in the current version.

 Author

Mostafa Shaban Elhenawy

