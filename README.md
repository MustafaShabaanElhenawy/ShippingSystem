# Shipping Management System

A shipping management web application built with **ASP.NET Web API (.NET Framework 4.7.2)**, **ADO.NET**, **SQL Server** and a **jQuery / Bootstrap** frontend. It manages clients, voyages, bills and containers, and includes **token-based authentication** and a complete **Audit Trail** module that records who did what, and when.

---

## Table of Contents

1. [Features](#features)
2. [Technologies](#technologies)
3. [Architecture](#architecture)
4. [Project Structure](#project-structure)
5. [Getting Started](#getting-started)
6. [Default Accounts](#default-accounts)
7. [Authentication and Authorization](#authentication-and-authorization)
8. [Audit Trail Module](#audit-trail-module)
9. [API Endpoints](#api-endpoints)
10. [Database Design](#database-design)
11. [Testing with Postman](#testing-with-postman)
12. [Design Decisions and Assumptions](#design-decisions-and-assumptions)
13. [Problems Faced and Solutions](#problems-faced-and-solutions)
14. [Scalability and Production Improvements](#scalability-and-production-improvements)
15. [Bonus Features](#bonus-features)
16. [Author](#author)

---

## Features

### Shipping Management
- Manage clients, voyages, bills and containers (create, read, update, delete)
- Search clients, voyages, bills and containers
- Advanced bill search with filtering, pagination and sorting
- Client, voyage and bill dropdowns
- Shipping Summary report
- Form validation, API error handling, success and error messages
- Delete confirmation

### Authentication
- Sign up and sign in pages with validation, password strength meter and show / hide password
- Secure password storage (salted PBKDF2)
- Token-based sessions stored in the database
- Signed-in username shown in the navbar, with Logout
- Role-based access (`Admin` and `User`)

### Audit Trail
- Records Login, Create, Update and Delete actions
- Stores the user, date and time, module, action type, record ID and description
- Field-level history for updates (old value and new value)
- Snapshots of the values for created and deleted records
- Audit Log screen with search, filters, pagination and a details view
- Audit Logs are available to administrators only
- A failure in audit logging never breaks the main operation

---

## Technologies

| Layer | Technologies |
|---|---|
| Backend | C#, ASP.NET Web API 2 (.NET Framework 4.7.2), ADO.NET |
| Database | SQL Server, Stored Procedures, SQL Views |
| Frontend | HTML, CSS, Bootstrap 5, JavaScript, jQuery, AJAX |
| Tools | Visual Studio, SSMS, Postman, Git / GitHub |

---

## Architecture

```text
HTML / Bootstrap
      ↓
JavaScript / jQuery (AJAX, Bearer token)
      ↓
ASP.NET Web API
   ├─ AuthTokenHandler  (reads the token, sets the user and role)
   ├─ Authorize filter  (every endpoint requires sign in)
   └─ Controllers
      ↓
BLL  (validation and business rules)
      ↓
DAL  (ADO.NET, stored procedures)
      ↓
SQL Server
```

How an audited action flows:

```text
Controller (Create / Update / Delete)
      ↓
AuditLoggingService  ──►  AuditLogChangeTracker  (compares old and new values)
      ↓
AuditLogBLL  (validation)
      ↓
AuditLogDAL  ──►  AuditLogs + AuditLogDetails
```

---

## Project Structure

```text
TrainingShippingSystem/
├── App_Start/          WebApiConfig (routes, handler, global Authorize)
├── Controllers/        Clients, Voyages, Bills, Containers, Reports, Auth, AuditLogs
├── BLL/                Business logic, AuthBLL, PasswordHasher,
│                       AuditLoggingService, AuditLogChangeTracker, AuditLogBLL
├── DAL/                ADO.NET data access, UserDAL, UserSessionDAL, AuditLogDAL
├── Models/             Entities and request / result models
├── Handlers/           AuthTokenHandler
├── Filters/            AdminOnlyAttribute
└── Frontend/
    ├── *.html          Pages (login, register, dashboard, modules, audit-logs)
    ├── css/            style.css, auth.css
    └── js/             auth.js and one script per page
```

---

## Getting Started

### Prerequisites
- Visual Studio 2019 or later with the **ASP.NET and web development** workload
- .NET Framework 4.7.2 developer pack
- SQL Server (Express is enough) and SQL Server Management Studio

### 1. Clone the repository

```bash
git clone https://github.com/MustafaShabaanElhenawy/ShippingSystem.git
```

### 2. Create the database

Run the scripts in **this order** in SSMS. Each script is safe to run once on top of the previous ones.

| Order | Script | Purpose |
|---|---|---|
| 1 | Shipping scripts | Create `TrainingShippingDB`, the Clients / Voyages / Bills / Containers tables, stored procedures, views and sample data |
| 2 | Audit Trail script | Create `AuditLogs`, `AuditLogDetails` and their stored procedures |
| 3 | `Users.sql` | Create `AppUsers` and the seed accounts |
| 4 | `Auth_Upgrade.sql` | Add `PasswordSalt`, `CreatedAt`, and the sign up / sign in stored procedures |
| 5 | `UserSessions.sql` | Create `UserSessions` (token storage) and its stored procedures |

### 3. Configure the connection string

Open `TrainingShippingSystem/Web.config` and set the connection string named `TrainingShippingDBConnection`:

```xml
<connectionStrings>
  <add name="TrainingShippingDBConnection"
       connectionString="Server=YOUR_SERVER;Database=TrainingShippingDB;Integrated Security=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

> Do not commit real passwords. Prefer Windows authentication for local development.

### 4. Run the application

1. Open `TrainingShippingSystem.sln` in Visual Studio.
2. Press **F5**. The API runs at `https://localhost:44320`.
3. Open the application at:

```text
https://localhost:44320/Frontend/login.html
```

> The frontend calls the API at `https://localhost:44320`. If your port is different, update the base URL in the `js` files.

---

## Default Accounts

| Username | Password | Role |
|---|---|---|
| `admin` | `Admin@123` | Admin |
| `user1` | `User@123` | User |

These are training accounts only. Change or remove them in a real environment. New users who register through **Sign up** always get the `User` role.

To promote a user to administrator:

```sql
UPDATE AppUsers SET Role = 'Admin' WHERE Username = 'the_username';
```

The change takes effect immediately, because the role is read from the database on every request.

---

## Authentication and Authorization

**Sign up** (`POST /api/Auth/Register`)
- Username: 3 to 30 characters (letters, numbers, dot, underscore)
- Password: at least 8 characters, with a letter and a number, and it must match the confirmation
- The role is never accepted from the client; the server always assigns `User`

**Sign in** (`POST /api/Auth/Login`)
- Returns a random 256-bit token, the username, the full name and the role
- Unknown username and wrong password return the same message, so accounts cannot be discovered

**Sessions**
- The token is sent as `Authorization: Bearer <token>`
- Sessions are stored in the `UserSessions` table and expire after 8 hours
- Only the **SHA-256 hash** of the token is stored, never the token itself
- Sessions survive an application restart
- `AuthTokenHandler` validates the token on every request and sets the user and role
- Logout deletes the session

**Passwords**
- PBKDF2-SHA256, 100,000 iterations, a unique 16-byte salt per user, constant-time comparison
- Older unsalted accounts are verified and upgraded automatically at their next sign in

**Authorization**
- A global `[Authorize]` filter protects every endpoint; only Login and Register are `[AllowAnonymous]`
- `[AdminOnly]` protects the Audit Logs API. It returns **401** when not signed in and **403** when signed in without the Admin role

---

## Audit Trail Module

### What is recorded

| Action | Module examples | Stored details |
|---|---|---|
| LOGIN | Auth | User, time, description |
| CREATE | Clients, Voyages, Bills, Containers, Users | Snapshot of the created values |
| UPDATE | Clients, Voyages, Bills, Containers | Only the fields that changed, with old and new values |
| DELETE | Clients, Voyages, Bills, Containers | Snapshot of the deleted values |

Each audit record contains the user, date and time, module, action type, record ID and a description.

### How it works
1. A controller performs its normal operation (for example, update a client).
2. It calls `AuditLoggingService` with the user from the token.
3. For updates, `AuditLogChangeTracker.Compare` compares the record before and after the update and returns only the changed fields.
4. For creates and deletes, `AuditLogChangeTracker.ForCreate` and `ForDelete` build a snapshot of the record.
5. `AuditLogBLL` validates the data and `AuditLogDAL` saves one `AuditLogs` row plus its `AuditLogDetails` rows.

### Preventing duplicate or useless logs
An update is logged **only if at least one field really changed**. Saving a form without changes creates no audit record.

### Audit Log screen (`Frontend/audit-logs.html`, Admin only)
- Table of activity with colored badges for Create, Update, Delete and Login
- Search by description or record ID
- Filters: user, module, action type and date range
- Server-side pagination with a selectable page size
- Details window: changed fields (old and new values) for updates, and created or deleted values for the other actions
- Times are shown in the local time zone

### Failure handling
Audit logging is **fail-open**: if writing the audit record fails, the error is written to the trace output and the business operation still succeeds. This favors availability. In a regulated system, you would instead log inside the same transaction as the operation.

---

## API Endpoints

All endpoints except Register and Login require `Authorization: Bearer <token>`.

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Auth/Register` | Create an account (role `User`) |
| POST | `/api/Auth/Login` | Sign in and get a token |
| POST | `/api/Auth/Logout` | End the current session |

### Clients, Voyages, Bills, Containers

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Clients` | Get all clients |
| GET | `/api/Clients/{id}` | Get client by ID |
| POST | `/api/Clients` | Create client |
| PUT | `/api/Clients/{id}` | Update client |
| DELETE | `/api/Clients/{id}` | Delete client |
| GET | `/api/Voyages` | Get all voyages |
| GET | `/api/Voyages/{id}` | Get voyage by ID |
| POST | `/api/Voyages` | Create voyage |
| PUT | `/api/Voyages/{id}` | Update voyage |
| DELETE | `/api/Voyages/{id}` | Delete voyage |
| GET | `/api/Bills` | Get all bills |
| GET | `/api/Bills/{id}` | Get bill by ID |
| POST | `/api/Bills` | Create bill |
| PUT | `/api/Bills/{id}` | Update bill |
| DELETE | `/api/Bills/{id}` | Delete bill |
| GET | `/api/Bills/AdvancedSearch` | Search by bill number, client and voyage |
| GET | `/api/Bills/Filter` | Filter with pagination and sorting |
| GET | `/api/Containers` | Get all containers |
| GET | `/api/Containers/{id}` | Get container by ID |
| POST | `/api/Containers` | Create container |
| PUT | `/api/Containers/{id}` | Update container |
| DELETE | `/api/Containers/{id}` | Delete container |

Clients, Voyages and Containers also have a `search` endpoint.

### Audit Logs (Admin only)

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/AuditLogs` | Paged list with filters |
| GET | `/api/AuditLogs/{id}` | One audit record with its details |
| GET | `/api/AuditLogs/Users` | Users for the filter dropdown |
| GET | `/api/AuditLogs/Modules` | Modules for the filter dropdown |

Query parameters for `GET /api/AuditLogs`: `Search`, `UserName`, `ModuleName`, `ActionType`, `FromDate`, `ToDate`, `PageNumber`, `PageSize`.

---

## Database Design

Database: `TrainingShippingDB`

```text
Client ──< Bill >── Voyage
             │
             └──< Container
```

| Group | Tables |
|---|---|
| Shipping | `Clients`, `Voyages`, `Bills`, `Containers` |
| Authentication | `AppUsers`, `UserSessions` |
| Audit Trail | `AuditLogs`, `AuditLogDetails` |

The database uses primary keys, foreign keys, identity columns, constraints, stored procedures and SQL views.

### Audit tables

```text
AuditLogs (1) ────< (many) AuditLogDetails
```

- `AuditLogs`: one row per action (user, module, action type, record ID, description, created time).
- `AuditLogDetails`: one row per changed field (field name, old value, new value).

**Why two tables?** One update can change several fields. Keeping the header and the field changes separate keeps the main table small and fast to filter and page, avoids empty columns, and makes it easy to show "which fields changed". A single table would repeat the header data for every field.

### Authentication tables
- `AppUsers`: username (unique), salted password hash, role, active flag.
- `UserSessions`: token hash (primary key), user, creation time and expiry. It is cascade-deleted with the user.

---

## Testing with Postman

1. Send `POST https://localhost:44320/api/Auth/Login` with a JSON body:
   ```json
   { "Username": "admin", "Password": "Admin@123" }
   ```
2. Copy the `token` from the response.
3. In the collection, open **Authorization**, choose **Bearer Token** and paste the token. Requests inherit it.
4. Call any endpoint, for example `GET /api/AuditLogs?ModuleName=Clients`.

Expected results for the Audit Logs API:

| Request | Result |
|---|---|
| No token, or an expired token | `401 Unauthorized` |
| Token of a `User` account | `403 Forbidden` |
| Token of an `Admin` account | `200 OK` with data |

---

## Design Decisions and Assumptions

### Decisions
- **Layered architecture** (Controller → BLL → DAL → stored procedures) keeps validation, business rules and data access separate and easy to test.
- **Reusable audit service**: any module can be audited with one call, without repeating audit code.
- **Change tracking by comparison**: the record is read before and after the update, so only real changes are stored.
- **Stored procedures and parameters** for all database access, which prevents SQL injection.
- **Token authentication stored in the database** instead of cookies or in-memory sessions, so it is stateless on the web server and survives restarts.
- **Server-side role decisions**: the role is never trusted from the client and is read from the database for every request.

### Assumptions
- The application is used by internal staff, so sign up creates only normal users and administrators are created from the database.
- Audit records are never edited or deleted from the application.
- Time is stored in UTC and shown in the user's local time.
- A single web server is used (no load balancing).
- The audit trail is more valuable than strict transactional coupling, so audit failures do not fail the business operation.

---

## Problems Faced and Solutions

| Problem | Solution |
|---|---|
| Audit records showed the user as **Anonymous** | Added sign in with tokens. `AuthTokenHandler` reads the token and sets the user, so controllers record the real username |
| A `Users` table already existed, so creating it failed | Created a dedicated `AppUsers` table and left the old table untouched |
| A non-admin user was logged out when opening an admin-only API | Web API's built-in `[Authorize]` returns 401 for signed-in users. A custom `AdminOnly` filter returns **403**, so the frontend can tell "not signed in" from "not allowed" |
| Sessions were lost whenever the application restarted | Moved sessions from memory to the `UserSessions` table and store only the token hash |
| Create and Delete audit records had no details | Added snapshots (`ForCreate`, `ForDelete`) stored as detail rows |
| An audit failure could break a valid operation | Audit logging is wrapped in error handling inside the service; failures are written to the trace output only |
| Old accounts used plain SHA-256 hashes | They are verified once and upgraded to salted PBKDF2 at the next sign in |

---

## Scalability and Production Improvements

**If the audit table contained millions of records**
- Add indexes on the filtered columns (created date, user, module, action type, record ID) and keep pagination in the database, so only one page is read.
- Prefer keyset pagination over deep offsets for very large pages.
- Replace `LIKE '%text%'` searches with full-text search.
- Archive or partition old records (for example by month) and keep recent records in the hot table.
- Write audit records asynchronously through a queue, so requests do not wait for the audit write.

**Other improvements for a production system**
- Make the audit table append-only (no update or delete permissions) and add integrity protection.
- Audit failed sign in attempts and add account lockout and rate limiting.
- Log inside the same transaction as the operation when strict compliance is required.
- Add a short in-memory cache for session lookups.
- Use HTTPS only, secret management for connection strings, and refresh tokens.
- Add a user management screen for administrators.
- Add password reset by email and optional two-factor authentication.
- Add automated unit and integration tests.

---

## Bonus Features

Implemented:
- Reusable audit logging that works with multiple modules
- User-friendly display of changed fields, created values and deleted values
- Search and filtering with pagination on the Audit Log screen

Not implemented in this version:
- Export of audit history to CSV or Excel
- Sorting on the Audit Log table
- Dashboard statistics for audit data

---

## Author

**Mostafa Shaban Elhenawy**
Software Engineering Trainee at IST
