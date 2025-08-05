# ----------- Stage 1: Build -------------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# Copy toàn bộ solution
COPY . ./

# Restore dependencies
RUN dotnet restore "GenScriptTool.sln"

# Build project (Release)
RUN dotnet publish "GenScriptTool/GenScriptTool.csproj" -c Release -o /app/publish

# ----------- Stage 2: Run -------------
FROM mcr.microsoft.com/dotnet/runtime:7.0

WORKDIR /app

COPY --from=build /app/publish .

# Entry point
ENTRYPOINT ["dotnet", "GenScriptTool.dll"]
