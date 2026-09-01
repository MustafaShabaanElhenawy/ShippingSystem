Training Shipping Management System

Shipping Management application built using ASP.NET Web API, C#, SQL Server, ADO.NET, HTML, Bootstrap, JavaScript, jQuery, and AJAX.

Technologies

Backend

* C#
* ASP.NET Web API
* SQL Server
* ADO.NET
* BLL / DAL
* Stored Procedures
* SQL Views

Frontend

* HTML
* CSS
* Bootstrap
* JavaScript
* jQuery
* AJAX

Features

* Client management (CRUD)
* Voyage management (CRUD)
* Bill management (CRUD)
* Container management (CRUD)
* Search clients
* Search voyages
* Search bills
* Search containers
* Client and Voyage dropdowns in Bills
* Bill dropdown in Containers
* Shipping Summary report
* SQL View for reporting
* Form validation
* API error handling
* Success and error messages
* Delete confirmation

API Endpoints

Method	Endpoint	Description
GET	/api/Clients	Get all clients
GET	/api/Clients/{id}	Get client by ID
POST	/api/Clients	Create client
PUT	/api/Clients/{id}	Update client
DELETE	/api/Clients/{id}	Delete client
GET	/api/Voyages	Get all voyages
GET	/api/Voyages/{id}	Get voyage by ID
POST	/api/Voyages	Create voyage
PUT	/api/Voyages/{id}	Update voyage
DELETE	/api/Voyages/{id}	Delete voyage
GET	/api/Bills	Get all bills
GET	/api/Bills/{id}	Get bill by ID
POST	/api/Bills	Create bill
PUT	/api/Bills/{id}	Update bill
DELETE	/api/Bills/{id}	Delete bill
GET	/api/Containers	Get all containers
GET	/api/Containers/{id}	Get container by ID
POST	/api/Containers	Create container
PUT	/api/Containers/{id}	Update container
DELETE	/api/Containers/{id}	Delete container

Database

Database: TrainingShippingDB

Main tables:

* Clients
* Voyages
* Bills
* Containers

Relationships:

Client
   ↓
Bills
   ↓
Containers
Voyage
   ↓
Bills

The database uses Primary Keys, Foreign Keys, Identity columns, constraints, Stored Procedures, and a SQL View for the Shipping Summary report.

Application Flow

HTML / Bootstrap
      ↓
JavaScript / jQuery
      ↓
AJAX
      ↓
ASP.NET Web API
      ↓
BLL
      ↓
DAL
      ↓
SQL Server

How to Run

1. Configure the SQL Server connection in the backend.
2. Create the TrainingShippingDB database and run the SQL scripts.
3. Run the ASP.NET Web API using Visual Studio.
4. Open the frontend pages.
5. Make sure the backend API is running before using the frontend.

Testing

The API can be tested using Postman, and the frontend can be used to test all CRUD and search operations.

Author

Mostafa Shaban Elhenawy
