.PHONY: help build-api build-web build-discord build-all clean test compose stop logs docker-prune token

# Variables
COMPOSE_FILE = docker-compose.yaml

help: ## Show this help message
	@echo "Available targets:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  %-20s %s\n", $$1, $$2}'

build: ## Build the Solution in Release mode
	dotnet build --configuration Release

test: ## Run Unit and Integration Tests
	dotnet test Wizdle.Unit.Tests/Wizdle.Unit.Tests.csproj --configuration Release
	dotnet test Wizdle.Integration.Tests/Wizdle.Integration.Tests.csproj --configuration Release

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
	rm -rf **/bin **/obj
	docker system prune --all --force --volumes

trust-cert: ## Trust the dotnet HTTPS development certificate
	dotnet dev-certs https --trust

token: ## Generate a random token for API keys (.env file)
	@pwsh -Command '-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | ForEach-Object {[char]$$_})'

mutation: ## Run Stryker Mutation Testing
	dotnet stryker --config-file stryker-config.json
