## 1. Project Structure

Controllers: Located in the Controllers folder. Each controller should focus on handling requests for a specific resource type (e.g., ProjectController for projects).
Models: Stored in the Models folder. Represents data entities and view models.
Views: Stored in the Views folder, with subfolders corresponding to each controller (e.g., Views/Project for ProjectController views).
Data Context: The database context is defined in the dbContext folder (PortofolioDbContext).
Static Files: Static assets (e.g., CSS, JS, images) are stored in the wwwroot folder.

## 2. Naming Conventions
# 2.1 Classes

Use PascalCase for class names.
Example: Project, Tag, AccountController.

# 2.2 Methods

Use PascalCase for method names.
Example: Create, Edit, Delete.

# 2.3 Variables

Use camelCase for local variables and method parameters.
Example: projectName, tagList.

# 2.4 Files and Folders

Use singular names for folders unless referring to a collection conceptually.
Example: Model, Controller.

# 2.5 View Files

Use the action name as the view name.
Example: Create.cshtml for the Create action in the ProjectController.

# 3. Authentication and Authorization

Authentication: Implemented using cookie-based authentication.
        Login path: /Account/Login
        Logout path: /Account/Logout
Authorization:
        Use User.Identity.IsAuthenticated to restrict access to certain actions or UI elements.
        Ensure links to sensitive actions (e.g., "Create Project") are displayed only to authenticated users.

## 4. Database and Models
# 4.1 Database Context

Use Entity Framework Core for database access.
Define the database context (PortofolioDbContext) with DbSet properties for each entity.
Example:

  public DbSet<Project> Projects { get; set; }
  public DbSet<Tag> Tags { get; set; }

# 4.2 Relationships

Configure relationships between entities using navigation properties and HasMany/HasOne Fluent API methods in the OnModelCreating method.

# 4.3 Migrations

Use EF Core migrations to manage database schema changes.
Commands:
Add a migration: dotnet ef migrations add MigrationName
Apply migrations: dotnet ef database update

## 5. Views
# 5.1 Layout

Use a consistent layout for all views. The _Layout.cshtml file in the Views/Shared folder defines the common structure.

# 5.2 View Conventions

Use Razor syntax for dynamic content rendering.
Use Html.ActionLink or TagHelpers for navigation links.
Display validation messages using Html.ValidationMessageFor.

## 6. Controllers
# 6.1 Responsibilities

Controllers handle HTTP requests and responses.
Use dependency injection for accessing the database context (PortofolioDbContext).

# 6.2 Actions

Use appropriate HTTP verbs for actions:
HttpGet for data retrieval and page rendering.
HttpPost for data submission.
Return RedirectToAction after successful data modifications.

# 6.3 Error Handling

Use try-catch blocks for operations prone to errors.
Return appropriate error views or messages for exceptions.

## 7. Routing

Use attribute routing or conventional routing as needed.
Ensure routes are descriptive and align with RESTful standards.
Example: /Project/Create, /Tag/Edit.

## 8. Session Management

Use sessions to store temporary user data (e.g., authentication state).
Enable session management in Program.cs:

app.UseSession();

## 9. Validation

Use DataAnnotations for model validation.
Example:

    [Required]
    public string Name { get; set; }

Validate data in controllers before performing database operations:

    if (!ModelState.IsValid)
    {
        return View(model);
    }


## 12. Coding Standards

Write clean and readable code.
Add comments where necessary, especially for complex logic.
Use proper indentation and formatting.
