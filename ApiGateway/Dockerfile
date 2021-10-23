# # For more info see: http://aka.ms/VSContainerToolingDockerfiles
# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
# # FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
# WORKDIR /src
# EXPOSE 5100

# # FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /src
# COPY *.sln ./
# COPY ApiGateway/DeveloperSummit.ApiGateway.csproj ApiGateway/
# RUN dotnet restore
# COPY . .
# WORKDIR /src/ApiGateway
# RUN dotnet build -c Release -o /src

# FROM builder AS publish
# RUN dotnet publish -c Release -o /src

# FROM base AS production
# WORKDIR /src
# COPY --from=publish /src .
# ENTRYPOINT ["dotnet", "DeveloperSummit.ApiGateway.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY DeveloperSummit.ApiGateway.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "DeveloperSummit.ApiGateway.dll"]