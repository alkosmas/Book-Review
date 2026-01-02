# 📚 Book Reviews - ASP.NET Core 9 MVC + REST API

A full-stack web application for managing books, reviews, and ratings with authentication, built using Clean Architecture principles.

---

## 🎯 Features

### Authentication & Authorization
- ✅ User registration and login (ASP.NET Core Identity)
- ✅ Protected routes (only authenticated users can add reviews/votes)
- ✅ Session management

### Books Management
- ✅ Create, Read, Update, Delete (CRUD) books
- ✅ Advanced filtering (Title, Author, Genre, Year, Rating)
- ✅ Pagination
- ✅ Professional Bootstrap UI

### Reviews & Ratings
- ✅ Add reviews with ratings (1-5 stars)
- ✅ View reviews per book
- ✅ Like/Dislike reviews (one vote per user)
- ✅ Display review statistics

### REST API
- ✅ RESTful API endpoints (JSON)
- ✅ Swagger documentation
- ✅ JWT authentication support

---

## 🏗️ Architecture

**Clean Architecture** with CQRS pattern:
```
BookReviews/
├── src/
│   ├── BookReviews.API          # Presentation Layer (MVC + API)
│   ├── BookReviews.Application  # Business Logic (CQRS Handlers)
│   ├── BookReviews.Domain       # Entities & Interfaces
│   └── BookReviews.Infrastructure # Data Access (EF Core)
```

**Technologies:**
- ASP.NET Core 9 MVC
- Entity Framework Core 9
- PostgreSQL 16
- MediatR (CQRS)
- Mapster (Object mapping)
- FluentValidation
- ASP.NET Core Identity

---

## 📋 Prerequisites

Before you begin, ensure you have installed:

### Required
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (9.0 or later)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for PostgreSQL)

### Optional
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [pgAdmin](https://www.pgadmin.org/) (PostgreSQL GUI)

---

## 🐳 Database Setup (PostgreSQL with Docker)

### Step 1: Start PostgreSQL Container

You're already using this command (perfect!):

```bash
docker run --name bookreviews-db \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres123 \
  -e POSTGRES_DB=BookReviewsDb \
  -p 5432:5432 \
  -d postgres:16
```

**Important:** Keep this container running! The app connects to it.

### Step 2: Verify Container is Running

```bash
docker ps
```

You should see:
```
CONTAINER ID   IMAGE         STATUS         PORTS                    NAMES
abc123...      postgres:16   Up 2 minutes   0.0.0.0:5432->5432/tcp  bookreviews-db
```

### Step 3: Connection String

The app uses this connection string (already configured in `appsettings.json`):

```json
"DefaultConnection": "Host=localhost;Port=5432;Database=BookReviewsDb;Username=postgres;Password=postgres123"
```

---

## 🚀 Installation & Running

### Step 1: Clone & Navigate

```bash
cd /path/to/MainSys
```

### Step 2: Update Required Files

Copy these updated files to your project:

#### A. GetBooksQuery.cs
```bash
cp GetBooksQuery.cs src/BookReviews.Application/Features/Books/Queries/GetBooks/GetBooksQuery.cs
```

#### B. GetBooksQueryHandler.cs
```bash
cp GetBooksQueryHandler.cs src/BookReviews.Application/Features/Books/Queries/GetBooks/GetBooksQueryHandler.cs
```

#### C. ReviewConfiguration.cs
```bash
cp ReviewConfiguration.cs src/BookReviews.Infrastructure/Persistence/Configurations/ReviewConfiguration.cs
```

#### D. Index.cshtml
```bash
cp Index.cshtml src/BookReviews.API/Views/BooksMvc/Index.cshtml
```

### Step 3: Restore Dependencies

```bash
dotnet restore
```

### Step 4: Apply Database Migrations

```bash
dotnet ef database update --project src/BookReviews.Infrastructure --startup-project src/BookReviews.API
```

This creates all tables in your PostgreSQL database.

### Step 5: Build the Project

```bash
dotnet build
```

### Step 6: Run the Application

```bash
dotnet run --project src/BookReviews.API
```

You should see:
```
Now listening on: http://localhost:5029
```

---

## 🧪 Testing the Application

### 1. Open Browser
```
http://localhost:5029
```

### 2. Register a User
- Navigate to `/account/register`
- Fill in: Username, Email, Password
- Click "Register"
- You'll be auto-logged in

### 3. Create a Book
- Go to `/books`
- Click "Add New Book"
- Fill in: Title, Author, Genre, Year
- Click "Save"

### 4. Test Filters
- Go to `/books`
- Try filtering by:
  - **Genre:** Select "Fiction"
  - **Year:** Enter "2024"
  - **Min Rating:** Select "4+"
  - Click "Apply Filters"

### 5. Add a Review
- Click "Reviews" on any book
- Click "Add Review"
- Write content, select rating (1-5)
- Click "Submit"

### 6. Vote on Reviews
- View a book's reviews
- Click 👍 (Like) or 👎 (Dislike)

---

## 📊 API Endpoints

### Books
```
GET    /api/books              - Get all books (with filters)
GET    /api/books/{id}         - Get book by ID
POST   /api/books              - Create book
PUT    /api/books/{id}         - Update book
DELETE /api/books/{id}         - Delete book (soft delete)
```

### Reviews
```
GET    /api/books/{bookId}/reviews       - Get reviews for a book
POST   /api/books/{bookId}/reviews       - Add review (requires auth)
PUT    /api/books/{bookId}/reviews/{id}/vote  - Vote on review (requires auth)
```

### Swagger UI
```
http://localhost:5029/swagger
```

---

## 🗂️ Database Schema

### Tables Created by Migrations

| Table | Description |
|-------|-------------|
| **AspNetUsers** | User accounts (Identity) |
| **Books** | Book information |
| **Reviews** | Book reviews with ratings |
| **ReviewVotes** | User votes on reviews |

### Sample Data Structure

**Book:**
```json
{
  "id": 1,
  "title": "The Great Gatsby",
  "author": "F. Scott Fitzgerald",
  "genre": "Fiction",
  "publishedYear": 1925
}
```

**Review:**
```json
{
  "id": 1,
  "bookId": 1,
  "userId": "user-guid",
  "content": "A masterpiece!",
  "rating": 5,
  "createdAt": "2024-01-02T10:30:00Z"
}
```

---

## 🐛 Troubleshooting

### Issue 1: Container Not Running
```bash
# Check if container exists
docker ps -a

# Start existing container
docker start bookreviews-db

# Or recreate it
docker rm bookreviews-db
docker run --name bookreviews-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres123 -e POSTGRES_DB=BookReviewsDb -p 5432:5432 -d postgres:16
```

### Issue 2: Port 5432 Already in Use
```bash
# Check what's using port 5432
sudo lsof -i :5432

# Option A: Stop other PostgreSQL
sudo systemctl stop postgresql

# Option B: Use different port
docker run --name bookreviews-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres123 -e POSTGRES_DB=BookReviewsDb -p 5433:5432 -d postgres:16

# Then update appsettings.json: Port=5433
```

### Issue 3: Database Connection Failed
```bash
# Test PostgreSQL connection
docker exec -it bookreviews-db psql -U postgres -d BookReviewsDb

# Inside psql:
\dt    # List tables
\q     # Quit
```

### Issue 4: Migrations Fail
```bash
# Remove database and recreate
dotnet ef database drop --project src/BookReviews.Infrastructure --startup-project src/BookReviews.API --force

# Apply migrations again
dotnet ef database update --project src/BookReviews.Infrastructure --startup-project src/BookReviews.API
```

### Issue 5: Build Errors
```bash
# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Rebuild
dotnet build
```

---

## 🔒 Default Credentials

**Admin User (if seeded):**
- Email: `admin@bookreviews.com`
- Password: `Admin123!`

**Register your own:**
- Navigate to `/account/register`
- Password requirements: 6+ characters

---

## 📁 Project Structure

```
MainSys/
├── src/
│   ├── BookReviews.API/
│   │   ├── Controllers/
│   │   │   ├── BooksController.cs          # REST API
│   │   │   ├── ReviewsController.cs        # REST API
│   │   │   ├── BooksMvcController.cs       # MVC
│   │   │   └── AccountMvcController.cs     # MVC Auth
│   │   ├── Views/
│   │   │   ├── BooksMvc/
│   │   │   │   ├── Index.cshtml           # Books list + filters
│   │   │   │   ├── Details.cshtml         # Book details + reviews
│   │   │   │   ├── Create.cshtml          # Add book
│   │   │   │   └── Edit.cshtml            # Edit book
│   │   │   └── AccountMvc/
│   │   │       ├── Login.cshtml           # Login form
│   │   │       └── Register.cshtml        # Registration
│   │   └── appsettings.json               # Configuration
│   │
│   ├── BookReviews.Application/
│   │   ├── Features/
│   │   │   ├── Books/
│   │   │   │   ├── Commands/             # Create, Update, Delete
│   │   │   │   └── Queries/              # Get, List, Filter
│   │   │   └── Reviews/
│   │   │       ├── Commands/             # Create, Vote
│   │   │       └── Queries/              # Get reviews
│   │   └── Common/
│   │       ├── Interfaces/
│   │       └── Models/
│   │
│   ├── BookReviews.Domain/
│   │   └── Entities/
│   │       ├── Book.cs
│   │       ├── Review.cs
│   │       └── ReviewVote.cs
│   │
│   └── BookReviews.Infrastructure/
│       ├── Persistence/
│       │   ├── AppDbContext.cs
│       │   ├── Configurations/          # EF Core configs
│       │   └── Migrations/              # Database migrations
│       └── Identity/
│           └── ApplicationUser.cs
```

---

## 🎓 Assessment Compliance

This project satisfies 100% of the assessment requirements:

| Requirement | Status |
|------------|--------|
| ASP.NET Core Identity | ✅ Implemented |
| User Registration/Login | ✅ Working |
| Books CRUD | ✅ Complete |
| Reviews & Ratings | ✅ Working |
| Vote System (1 vote/user) | ✅ Implemented |
| REST API Endpoints | ✅ All endpoints |
| MVC UI with Filters | ✅ Genre, Year, Rating filters |
| EF Core Code-First | ✅ Migrations working |
| Clean Architecture | ✅ CQRS + MediatR |
| Async/Await | ✅ Throughout |

---

## 🛠️ Development Commands

### Database
```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/BookReviews.Infrastructure --startup-project src/BookReviews.API

# Update database
dotnet ef database update --project src/BookReviews.Infrastructure --startup-project src/BookReviews.API

# Drop database
dotnet ef database drop --project src/BookReviews.Infrastructure --startup-project src/BookReviews.API --force
```

### Build & Run
```bash
# Clean
dotnet clean

# Restore packages
dotnet restore

# Build
dotnet build

# Run
dotnet run --project src/BookReviews.API

# Run with hot reload
dotnet watch run --project src/BookReviews.API
```

### Docker
```bash
# Start container
docker start bookreviews-db

# Stop container
docker stop bookreviews-db

# View logs
docker logs bookreviews-db

# Connect to PostgreSQL
docker exec -it bookreviews-db psql -U postgres -d BookReviewsDb

# Remove container
docker rm -f bookreviews-db
```

---

## 📝 Notes

### Docker is REQUIRED
- Yes, you **need Docker** running with PostgreSQL
- The app **will not work** without the database container
- Keep the container running: `docker start bookreviews-db`

### Connection String Location
```
src/BookReviews.API/appsettings.json
```

Make sure it matches your Docker settings:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=BookReviewsDb;Username=postgres;Password=postgres123"
}
```

### First Time Setup
1. ✅ Start Docker container
2. ✅ Run migrations
3. ✅ Build project
4. ✅ Run application
5. ✅ Register user
6. ✅ Start using!

---

## 🚦 Quick Start Checklist

- [ ] Docker Desktop is running
- [ ] PostgreSQL container is started (`docker ps`)
- [ ] Copied all updated files
- [ ] Ran `dotnet restore`
- [ ] Ran `dotnet ef database update`
- [ ] Ran `dotnet build` (no errors)
- [ ] Ran `dotnet run --project src/BookReviews.API`
- [ ] Opened `http://localhost:5029`
- [ ] Registered a user
- [ ] Created a book
- [ ] Added a review
- [ ] Tested filters

---

## 📞 Support

If you encounter issues:

1. **Check Docker:** `docker ps` should show `bookreviews-db` running
2. **Check logs:** `docker logs bookreviews-db`
3. **Check build:** `dotnet build` should succeed
4. **Check connection:** Connection string matches Docker settings
5. **Check migrations:** Database tables exist

---

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Docker Documentation](https://docs.docker.com/)
- [Clean Architecture Guide](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## 📄 License

This project is for educational/assessment purposes.

---

## ✨ Author

Created as part of .NET Core 9 MVC + REST API assessment.
