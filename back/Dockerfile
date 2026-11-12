# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json Directory.Build.props Directory.Packages.props ./
COPY src/Petyei.Domain/Petyei.Domain.csproj src/Petyei.Domain/
COPY src/Petyei.Application/Petyei.Application.csproj src/Petyei.Application/
COPY src/Petyei.Infrastructure/Petyei.Infrastructure.csproj src/Petyei.Infrastructure/
COPY src/Petyei.Api/Petyei.Api.csproj src/Petyei.Api/
RUN dotnet restore src/Petyei.Api/Petyei.Api.csproj

COPY src/ src/
RUN dotnet publish src/Petyei.Api/Petyei.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "Petyei.Api.dll"]
