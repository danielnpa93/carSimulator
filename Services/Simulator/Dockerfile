FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln ./
COPY src/Simulator.Console/*.csproj ./src/Simulator.Console/
COPY src/Simulator.Schema/*.csproj ./src/Simulator.Schema/
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Simulator.Console.dll"]