# API Functional Tests

Tests that run against the Wizdle API in a Docker container.

## Running Tests

```bash
dotnet test Wizdle.Api.Functional.Tests
```

> **Note:** The Docker image is built automatically from the Dockerfile when tests run. Tests can configure rate limits per test class via `ContainerSetup.GetWizdleApiUrl(permitLimit, windowSeconds)`.
