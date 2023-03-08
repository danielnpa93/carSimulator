FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY src/DriverService.API/*.csproj ./src/DriverService.API/
WORKDIR /app/src/DriverService.API
RUN dotnet restore

COPY src/DriverService.API ./
COPY src/Dlls ../Dlls/
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/src/DriverService.API/out .
# COPY src/Dlls/*.dll .

ENTRYPOINT ["dotnet","DriverService.API.dll"]

