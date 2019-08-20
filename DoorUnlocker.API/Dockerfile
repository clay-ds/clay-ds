FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
EXPOSE 80
WORKDIR /app

COPY . ./
RUN dotnet publish DoorUnlocker.API -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/DoorUnlocker.API/out .
ENTRYPOINT ["dotnet", "DoorUnlocker.API.dll"]