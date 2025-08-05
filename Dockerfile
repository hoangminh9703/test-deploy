# ----------- Stage 1: Build (với SDK .NET 8) -------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . ./

RUN dotnet restore "GenScriptTool.sln"
RUN dotnet publish "GenScriptTool/GenScriptTool.csproj" -c Release -o /app/publish

# ----------- Stage 2: Run (với Runtime .NET 8) -------------
FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GenScriptTool.dll"]
