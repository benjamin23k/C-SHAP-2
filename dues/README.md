# Residential Dues Management API (SQL Server)

ASP.NET Core 8 Web API using Entity Framework Core with SQL Server.

## Setup

1. Update the connection string in `appsettings.json` (`ConnectionStrings:DefaultConnection`) to point to your SQL Server instance.

2. Restore packages:
```bash
dotnet restore
```

3. Install EF Core CLI tool (if not already installed):
```bash
dotnet tool install --global dotnet-ef
```

4. Create the initial migration:
```bash
dotnet ef migrations add InitialCreate
```

5. Run the API (migrations apply automatically on startup):
```bash
dotnet run
```

Swagger UI available at `/swagger`.

## Models
- **Resident**: Id, Name, ApartmentId
- **Apartment**: Id, Number, MonthlyFee
- **Due**: Id, ApartmentId, Month, Year, Amount, AmountPaid, DueDate, Status (Pending/Partial/Paid/Overdue), Balance (computed)
- **Payment**: Id, DueId, Amount, Date, Method, ReceiptNumber

## Endpoints

### Residents
- `GET/POST /api/residents`
- `GET/PUT/DELETE /api/residents/{id}`

### Apartments
- `GET/POST /api/apartments`
- `GET/PUT/DELETE /api/apartments/{id}`

### Dues
- `GET/POST /api/dues`
- `GET/DELETE /api/dues/{id}`
- `GET /api/dues/apartment/{apartmentId}`
- `POST /api/dues/generate-monthly?month=6&year=2026` — creates dues for all apartments for that period
- `POST /api/dues/update-overdue` — flags past-due unpaid dues as Overdue
- `GET /api/dues/reports/debts` — total debt per apartment

### Payments
- `GET/POST /api/payments`
- `GET/DELETE /api/payments/{id}`
- `GET /api/payments/due/{dueId}`
- `GET /api/payments/{id}/receipt` — receipt for a payment

## Seed data
Apartments 101, 102 (MonthlyFee 2500), 201 (MonthlyFee 3000). Residents: Wilson Rosario (apt 101), Maria Perez (apt 102).
