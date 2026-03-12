# Step 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy both project files directly (no .sln needed)
COPY QuickTable.API/*.csproj ./QuickTable.API/
COPY QuickTable.Service/*.csproj ./QuickTable.Service/

# Restore dependencies
RUN dotnet restore ./QuickTable.API/QuickTable.API.csproj

# Copy all source code
COPY . .

# Build and publish
RUN dotnet publish ./QuickTable.API/QuickTable.API.csproj -c Release -o out

# Step 2 - Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
CMD ["dotnet", "QuickTable.API.dll"]