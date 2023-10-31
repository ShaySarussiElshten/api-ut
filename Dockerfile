# Use Microsoft's official lightweight .NET images.
# https://hub.docker.com/r/microsoft/dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ParkingGarageManagement.dll"]