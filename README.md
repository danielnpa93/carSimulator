# Car Simulator

Small project of a car driver, using GoogleMaps Directions API. Simulating a realtime position of a drive.

API with websocket(.NET6 & SignalR) + Console Simulator (.NET6) + FrontEnd (React) + MessageBroker (Kafka) + DB NoSql(MongoDB) 

![image](https://user-images.githubusercontent.com/45390048/232230928-10fc7963-6eb9-4863-931d-b7a6a3c4d085.png)


# Usage

Get your Google KEY and put on .env(REACT) and .appsettings.json(API)

Gen you local SSL certificate:

    $dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p <yourPass>
    $dotnet dev-certs https --trust
    
set Secret on csProj (UserSecretsId  | AppData/Roaming/Microsoft/UserSecrets):

    $dotnet user-secrets set "Kestrel:Certificates:Development:Password" <yourPass> 
    
Finally:

    $docker-compose up -d
    &docker-compose down --volume


