# 🏗️ Архитектура проекта

## 📊 Диаграмма взаимодействия

```
┌─────────────────────────────────────────────────────────────────┐
│                      Веб-браузер (Frontend)                       │
│                    http://localhost:3000                          │
├─────────────────────────────────────────────────────────────────┤
│                        React Application                          │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│  │  App.js          │  │  Components/     │  │  Styles/         │
│  │  - State         │  │  - CarList       │  │  - *.css         │
│  │  - API calls     │  │  - CarCard       │  │  - Styling       │
│  │  - Handlers      │  │  - AddCarForm    │  │  - Layout        │
│  │                  │  │  - AddPartForm   │  │                  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘
└─────────────────────────────────────────────────────────────────┘
                               ↕
                            HTTP/REST
                      (JSON format requests)
                               ↕
┌─────────────────────────────────────────────────────────────────┐
│                    Backend Server (API)                           │
│                    http://localhost:5000                          │
├─────────────────────────────────────────────────────────────────┤
│                  C# ASP.NET Core Web API                          │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│  │  Controllers/    │  │  Models/         │  │  Data/           │
│  │  - CarsCtrlr    │  │  - Car.cs        │  │  - AppDbContext  │
│  │  - PartsCtrlr   │  │  - Part.cs       │  │  - Migrations    │
│  │  - Routing      │  │  - Properties    │  │  - EF Core       │
│  │  - Validation   │  │  - Relationships │  │                  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘
│                                                                    │
│              Program.cs (Configuration)                           │
│              - CORS Setup                                         │
│              - Dependency Injection                               │
│              - Database Connection                                │
│              - Middleware Setup                                   │
└─────────────────────────────────────────────────────────────────┘
                               ↕
                            SQL Queries
                      (CREATE, READ, UPDATE, DELETE)
                               ↕
┌─────────────────────────────────────────────────────────────────┐
│                     Database (SQLite)                             │
│                        cars.db                                    │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────┐      ┌─────────────────────┐            │
│  │   Cars Table        │      │   Parts Table       │            │
│  ├─────────────────────┤      ├─────────────────────┤            │
│  │ Id (PK)            │      │ Id (PK)             │            │
│  │ Name               │      │ Name                │            │
│  │ Brand              │      │ Price               │            │
│  │ Year               │      │ Type                │            │
│  │ Price              │      │ CarId (FK) ─────────┼──→         │
│  │ CreatedAt          │      │ CreatedAt           │            │
│  └─────────────────────┘      └─────────────────────┘            │
│                                                                    │
│  One Car ──→ Many Parts (One-to-Many relationship)               │
└─────────────────────────────────────────────────────────────────┘
```

## 🔄 Поток данных

### Добавление автомобиля

```
1. User fills form in React
   ↓
2. Click "Add Car" button
   ↓
3. fetch POST /api/cars with JSON {name, brand, year, price}
   ↓
4. Backend receives in CarsController
   ↓
5. Create Car object with auto-generated Id
   ↓
6. Add to DbContext and SaveChangesAsync()
   ↓
7. Database: INSERT INTO Cars (Name, Brand, Year, Price) VALUES (...)
   ↓
8. Return Car object with Id to React (201 Created)
   ↓
9. React adds to state.cars and re-renders
   ↓
10. Car appears in the list
```

### Получение машин

```
1. useEffect() on App.js mount
   ↓
2. fetch GET /api/cars
   ↓
3. Backend CarsController.GetCars()
   ↓
4. DbContext.Cars.Include(c => c.Parts)
   ↓
5. Database: SELECT * FROM Cars INNER JOIN Parts
   ↓
6. Return array of Cars with their Parts
   ↓
7. React setCars(data)
   ↓
8. Page renders with all cars and their parts
```

### Добавление запчасти

```
1. User fills part form and clicks "Add"
   ↓
2. fetch POST /api/parts with {name, price, type, carId}
   ↓
3. Backend PartsController.PostPart()
   ↓
4. Verify Car with carId exists
   ↓
5. Create Part object
   ↓
6. Add to DbContext and SaveChangesAsync()
   ↓
7. Database: INSERT INTO Parts (Name, Price, Type, CarId)
   ↓
8. Return Part object to React
   ↓
9. React adds part to car.parts array
   ↓
10. Part appears under the car
```

## 📁 Структура файлов

### Backend структура

```
backend/
├── Models/
│   ├── Car.cs              # Car entity
│   └── Part.cs             # Part entity
│
├── Data/
│   ├── AppDbContext.cs     # DbContext configuration
│   └── [Migrations]/       # Auto-generated migrations (если нужны)
│
├── Controllers/
│   ├── CarsController.cs   # GET, POST, PUT, DELETE for Cars
│   └── PartsController.cs  # GET, POST, PUT, DELETE for Parts
│
├── Program.cs              # App configuration & startup
├── backend.csproj          # Project file with NuGet packages
│
├── appsettings.json        # App settings
├── appsettings.Development.json
│
├── bin/                    # Compiled output
│   └── Debug/
│       └── net10.0/
│           ├── backend.dll
│           ├── cars.db     # SQLite database ← ЭТО ВАЖНО!
│           └── [other files]
│
└── obj/                    # Intermediate compilation files
```

### Frontend структура

```
frontend/
├── src/
│   ├── components/
│   │   ├── AddCarForm.js     # Form for adding cars
│   │   ├── AddPartForm.js    # Form for adding parts
│   │   ├── CarCard.js        # Single car card with parts
│   │   └── CarList.js        # Grid of car cards
│   │
│   ├── styles/
│   │   ├── AddCarForm.css    # Styles for car form
│   │   ├── AddPartForm.css   # Styles for part form
│   │   ├── CarCard.css       # Styles for car card
│   │   └── CarList.css       # Styles for car list
│   │
│   ├── App.js               # Main component (state, API calls)
│   ├── App.css              # Main styles
│   ├── index.js             # React entry point
│   └── index.css            # Global styles
│
├── public/
│   ├── index.html           # HTML template
│   └── manifest.json
│
├── package.json             # Dependencies & scripts
├── .env                     # Environment variables
└── node_modules/            # Installed packages
```

## 🔗 Relationships (Модели)

### Car Model
```csharp
public class Car
{
    public int Id { get; set; }                      // Primary Key
    public string Name { get; set; }                 // e.g. "BMW X5"
    public string Brand { get; set; }                // e.g. "BMW"
    public int Year { get; set; }                    // e.g. 2023
    public decimal Price { get; set; }               // e.g. 5000000
    public DateTime CreatedAt { get; set; }          // Auto-timestamp
    
    // One-to-Many: One Car has Many Parts
    public ICollection<Part> Parts { get; set; }     // Related parts
}
```

### Part Model
```csharp
public class Part
{
    public int Id { get; set; }                      // Primary Key
    public string Name { get; set; }                 // e.g. "Oil Filter"
    public decimal Price { get; set; }               // e.g. 5000
    public string Type { get; set; }                 // e.g. "engine"
    public DateTime CreatedAt { get; set; }          // Auto-timestamp
    
    // Many-to-One: Many Parts belong to One Car
    public int CarId { get; set; }                   // Foreign Key
    public Car Car { get; set; }                     // Navigation property
}
```

## 🌊 Data Flow Lifecycle

### Initial Load (Page Refresh)
```
1. React Component mounted
2. useEffect hook triggers
3. Fetch all cars with parts: GET /api/cars
4. Receive JSON array with cars and nested parts
5. setState(cars)
6. Component re-renders with data
7. Page displays all cars and parts
```

### Add Car
```
1. Form submit event
2. Validation check
3. POST /api/cars with car data
4. Backend saves to database
5. Receive created car object with Id
6. Add to local state
7. Form clears
8. List updates with new car
```

### Add Part to Car
```
1. Form submit event
2. POST /api/parts with part data + carId
3. Backend validates car exists
4. Saves to database
5. Receive part object with Id
6. Find car in state by carId
7. Add part to car.parts array
8. Component re-renders
9. Part visible under car
```

### Delete
```
1. Click delete button
2. Confirmation dialog
3. DELETE /api/cars/{id} or /api/parts/{id}
4. Backend removes from database
5. Receive 204 No Content
6. Remove from local state
7. Component re-renders
8. Item disappears from list
```

## 🔐 Entity Framework Relationships

```csharp
// In AppDbContext.OnModelCreating():

modelBuilder.Entity<Car>()
    .HasMany(c => c.Parts)           // One Car
    .WithOne(p => p.Car)             // Many Parts
    .HasForeignKey(p => p.CarId)     // Foreign Key
    .OnDelete(DeleteBehavior.Cascade); // If car deleted → parts deleted
```

**Это означает:**
- Когда удаляется Car → все его Parts автоматически удаляются
- Каждый Part может принадлежать только одному Car
- Каждый Car может иметь много Parts

## 💾 Database Schema

### Cars Table
```sql
CREATE TABLE [Cars] (
    [Id]        INTEGER PRIMARY KEY AUTOINCREMENT,
    [Name]      TEXT NOT NULL,
    [Brand]     TEXT NOT NULL,
    [Year]      INTEGER NOT NULL,
    [Price]     DECIMAL(18, 2) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL
);
```

### Parts Table
```sql
CREATE TABLE [Parts] (
    [Id]        INTEGER PRIMARY KEY AUTOINCREMENT,
    [Name]      TEXT NOT NULL,
    [Price]     DECIMAL(18, 2) NOT NULL,
    [Type]      TEXT NOT NULL,
    [CarId]     INTEGER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    FOREIGN KEY ([CarId]) REFERENCES [Cars]([Id]) ON DELETE CASCADE
);
```

## 🔌 API Request/Response Flow

### Request Example: Add Car
```
POST /api/cars HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Content-Length: 67

{"name":"BMW X5","brand":"BMW","year":2023,"price":5000000}
```

### Response Example
```
HTTP/1.1 201 Created
Content-Type: application/json
Content-Length: 142

{
  "id":1,
  "name":"BMW X5",
  "brand":"BMW",
  "year":2023,
  "price":5000000,
  "createdAt":"2024-12-29T10:30:00Z",
  "parts":[]
}
```

## 🎯 Component Hierarchy

```
App (Main component with state)
├── Header (Title & description)
├── Error message (if any)
├── AddCarForm (Form to add cars)
└── CarList (Grid of cars)
    └── CarCard (For each car)
        ├── Car header (Name, Brand, Year)
        ├── Car price
        ├── Parts section
        │   ├── Part list
        │   │   └── PartItem (For each part)
        │   │       ├── Part name
        │   │       ├── Part type
        │   │       └── Delete button
        │   ├── Total parts price
        │   └── "Add part" button
        └── AddPartForm (Form to add parts - conditional)
```

## 🔄 State Management

All state is in `App.js`:
- `cars` - Array of car objects with their parts
- `loading` - Boolean for loading state
- `error` - String for error message

State is updated only through:
- `setCars()` - Update cars array
- `setLoading()` - Update loading state
- `setError()` - Update error message

No Redux or Context needed - simple and effective!

## 🚀 Performance Considerations

1. **Lazy loading** - All cars loaded once on mount
2. **Optimistic updates** - UI updates immediately, server validates
3. **Error handling** - User sees error messages
4. **Database indexing** - FK on CarId automatically indexed
5. **Cascade delete** - Efficient cleanup of related data

---

**This architecture ensures**:
- ✅ Clean separation of concerns
- ✅ Scalable design
- ✅ Easy to add features
- ✅ Type-safe (C#)
- ✅ Responsive UI (React)
- ✅ Persistent data (SQLite)
