# Wizdle.Web

A Blazor Server web application that provides a browser-based UI for solving Wordle puzzles using the Wizdle engine.

Built with [MudBlazor](https://mudblazor.com/) and integrated with [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) for orchestration and deployment.

## Overview

Wizdle.Web communicates with the `Wizdle.Api` backend via HTTP and renders an interactive form where you can enter:

- **Correct Letters** — letters in the correct position (green tiles)
- **Misplaced Letters** — letters present but in the wrong position (yellow tiles)
- **Excluded Letters** — letters not in the word (grey tiles)

## Running Locally

Wizdle.Web is designed to run as part of the full Aspire-orchestrated stack. Use the `make` commands from the repository root:

```sh
# Build the Docker image
make build-web

# Run all services (API, Web, Discord)
make compose

# Stop all services
make stop

# View logs
make logs
```

## Functional Tests

Browser-based functional tests are in `Wizdle.Web.Functional.Tests` using Playwright.

```sh
make test-functional
```
