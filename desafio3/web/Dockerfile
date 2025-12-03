FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
RUN dotnet new web -n WebApp
COPY Program.cs WebApp/Program.cs
WORKDIR /src/WebApp
RUN dotnet add package Npgsql
RUN dotnet add package StackExchange.Redis
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]

