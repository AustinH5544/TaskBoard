# TaskBoard

A personal task manager built with ASP.NET Core MVC (.NET 8).

## Tech Stack

- **ASP.NET Core MVC** — controllers, Razor views, model binding, validation
- **Entity Framework Core** — code-first with SQLite
- **Bootstrap 5** — responsive layout, table badges, navbar (CDN)
- **jQuery** — AJAX for inline toggle, delete, and status filtering (CDN)

## Features

- Create, edit, and delete tasks
- Priority levels (Low / Medium / High) with color-coded badges
- Inline completion toggle via checkbox (AJAX, no page reload)
- Inline delete with confirmation dialog (AJAX, fade-out animation)
- Filter tasks by status (All / Active / Completed) without page reload
- Tasks sorted by creation date, descending

## Project Structure

```
TaskBoard/
├── Controllers/
│   └── TasksController.cs     # CRUD + AJAX JSON endpoints
├── Data/
│   └── AppDbContext.cs        # EF Core DbContext
├── Models/
│   └── TaskItem.cs            # TaskItem model + Priority enum
└── Views/
    ├── Shared/
    │   ├── _Layout.cshtml
    │   └── _ValidationScriptsPartial.cshtml
    └── Tasks/
        ├── _TaskTable.cshtml  # Partial view (used for initial load + AJAX filter)
        ├── Index.cshtml
        ├── Create.cshtml
        └── Edit.cshtml
```

## Running Locally

```bash
dotnet run
```

The database (`taskboard.db`) is created automatically on first run via `EnsureCreated()`.

To use proper EF migrations instead:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
