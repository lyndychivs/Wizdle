.PHONY: help build-api build-web build-discord build-all clean test compose stop logs docker-prune token playwright mutate trust-cert restart solve test-functional test-all tools perf

# Variables
COMPOSE_FILE = docker-compose.yaml

help: ## Show this help message
	@echo "Available targets:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  %-20s %s\n", $$1, $$2}'

build: ## Build the Solution in Release mode
	dotnet build --configuration Release

test: ## Run Unit and Integration Tests
	dotnet test --project Wizdle.Unit.Tests/Wizdle.Unit.Tests.csproj --configuration Release --no-build
	dotnet test --project Wizdle.Api.Unit.Tests/Wizdle.Api.Unit.Tests.csproj --configuration Release --no-build
	dotnet test --project Wizdle.Web.Unit.Tests/Wizdle.Web.Unit.Tests.csproj --configuration Release --no-build
	dotnet test --project Wizdle.Discord.Unit.Tests/Wizdle.Discord.Unit.Tests.csproj --configuration Release --no-build
ifeq ($(OS),Windows_NT)
	dotnet test --project Wizdle.Wpf.Unit.Tests/Wizdle.Wpf.Unit.Tests.csproj --configuration Release --no-build
endif
	dotnet test --project Wizdle.Integration.Tests/Wizdle.Integration.Tests.csproj --configuration Release --no-build

test-functional: ## Run Functional Tests
	dotnet test --project Wizdle.Api.Functional.Tests/Wizdle.Api.Functional.Tests.csproj --configuration Release --no-build
	dotnet test --project Wizdle.Web.Functional.Tests/Wizdle.Web.Functional.Tests.csproj --configuration Release --no-build

test-all: test test-functional solve ## Run all tests

solve: ## Attempts to solve Wordle using Wizdle
	dotnet test --project Wizdle.Functional.Tests/Wizdle.Functional.Tests.csproj --configuration Release --no-build

build-api: ## Build Wizdle.Api Docker image (wizdle-api:latest)
	dotnet publish Wizdle.Api/Wizdle.Api.csproj --configuration Release -t:PublishContainer --os linux --arch x64 -p:ContainerImageTag=latest -p:ContainerRepository=wizdle-api

build-web: ## Build Wizdle.Web Docker image (wizdle-web:latest)
	dotnet publish Wizdle.Web/Wizdle.Web.csproj --configuration Release -t:PublishContainer --os linux --arch x64 -p:ContainerImageTag=latest -p:ContainerRepository=wizdle-web

build-discord: ## Build Wizdle.Discord Docker image (wizdle-discord:latest)
	dotnet publish Wizdle.Discord/Wizdle.Discord.csproj --configuration Release -t:PublishContainer --os linux --arch x64 -p:ContainerImageTag=latest -p:ContainerRepository=wizdle-discord

build-all: build-api build-web build-discord ## Builds all Docker images

compose: ## Composes Wizdle Docker images
	docker compose --file $(COMPOSE_FILE) up --detach

stop: ## Stops Wizdle Docker images
	docker compose --file $(COMPOSE_FILE) down

stop-volumes: ## Stops Wizdle Docker images and removes volumes
	docker compose --file $(COMPOSE_FILE) down --volumes

logs: ## Shows logs for Wizdle Docker images
	@docker compose --file $(COMPOSE_FILE) logs --follow

docker-prune: ## Prune unused Docker resources
	docker system prune --all --force --volumes

restart: stop-volumes build-all compose ## Rebuild Wizdle Docker images and restart Containers

clean: ## Clean Wizdle build artifacts and Docker resources
	dotnet clean
	rm -rf **/bin **/obj TestResults BenchmarkDotNet.Artifacts
	docker system prune --all --force --volumes

trust-cert: ## Trust the dotnet HTTPS development certificate
	dotnet dev-certs https --trust

token: ## Generate a random token for API keys (.env file)
	@pwsh -Command '-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | ForEach-Object {[char]$$_})'

tools: ## Restore dotnet tools
	dotnet tool restore

perf: ## Run Performance Tests
	dotnet run --project Wizdle.Performance.Tests/Wizdle.Performance.Tests.csproj --configuration Release

mutate: ## Run Stryker Mutation Testing
	dotnet stryker --config-file Wizdle.Unit.Tests/stryker-config.json

playwright: ## Install Playwright browsers
	pwsh -Command "& (Get-ChildItem -Path Wizdle.Web.Functional.Tests -Filter playwright.ps1 -Recurse | Select-Object -First 1).FullName install"
