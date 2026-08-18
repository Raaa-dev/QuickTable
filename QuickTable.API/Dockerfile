# Step 1 - Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Cache dependency restoration
COPY QuickTable.API/*.csproj ./QuickTable.API/
COPY QuickTable.Service/*.csproj ./QuickTable.Service/
RUN dotnet restore ./QuickTable.API/QuickTable.API.csproj

# Copy source code and build
COPY . .
RUN dotnet publish ./QuickTable.API/QuickTable.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Step 2 - Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Ensure uploads folder exists
RUN mkdir -p /app/uploads

# Copy published files
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "QuickTable.API.dll"]