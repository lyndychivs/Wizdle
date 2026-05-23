# Web Functional Tests

Tests that run against the Wizdle Web application in Docker containers using Playwright.

## Running Tests

```bash
dotnet test Wizdle.Web.Functional.Tests
```

> **Note:** The Docker images for both the API and Web containers are built automatically from the Dockerfiles when tests run. Tests use Playwright for browser automation to test the Blazor web application.
