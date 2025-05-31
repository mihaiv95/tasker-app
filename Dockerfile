# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
RUN mkdir -p ./Resources
EXPOSE 5070
EXPOSE 7221

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy csproj and restore dependencies (for better layer caching)
COPY ["tasker-app.csproj", "./"]
RUN dotnet restore "tasker-app.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "tasker-app.csproj" -c $configuration -o /app/build

# Development stage for debugging
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS development
WORKDIR /src

# Install dotnet tools and debugger
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Install the Visual Studio debugger
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

# Copy csproj and restore
COPY ["tasker-app.csproj", "./"]
RUN dotnet restore "tasker-app.csproj"

# Copy source code
COPY . .

EXPOSE 5070
EXPOSE 7221

# Build the application in Debug mode and keep it ready for debugging
RUN dotnet build "tasker-app.csproj" -c Debug -o /app/build

# Start the application automatically in debug mode
CMD ["dotnet", "/app/build/tasker-app.dll", "--urls", "http://0.0.0.0:5070"]

# Publish stage
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "tasker-app.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final production stage
FROM base AS final
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "tasker-app.dll"]
