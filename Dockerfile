# Import SDK to build app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set work dir
WORKDIR /app

# Copy project and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Run migrations

# Copy source code
COPY . ./

# Build app
RUN dotnet publish -c Release -o out

# Import Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl
RUN apt-get update && apt-get install -y curl

# Copy build output
COPY --from=build /app/out ./

# Expose port 80
EXPOSE 80

# Run migrations
ENTRYPOINT ["dotnet", "Chat.dll", "database update"]

# Set entry point
ENTRYPOINT ["dotnet", "Chat.dll"]