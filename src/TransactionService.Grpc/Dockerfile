# Base image with .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ./src/TransactionService.sln
RUN dotnet build ./src/TransactionService.Grpc/TransactionService.Grpc.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish ./src/TransactionService.Grpc/TransactionService.Grpc.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionService.Grpc.dll"]
