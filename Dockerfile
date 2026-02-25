# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Sneakers.Shop.Backend.Api/Sneakers.Shop.Backend.Api.csproj", "src/Sneakers.Shop.Backend.Api/"]
COPY ["src/Sneakers.Shop.Backend.Application/Sneakers.Shop.Backend.Application.csproj", "src/Sneakers.Shop.Backend.Application/"]
COPY ["src/Sneakers.Shop.Backend.Domain/Sneakers.Shop.Backend.Domain.csproj", "src/Sneakers.Shop.Backend.Domain/"]
COPY ["src/Sneakers.Shop.Backend.Infrastructure/Sneakers.Shop.Backend.Infrastructure.csproj", "src/Sneakers.Shop.Backend.Infrastructure/"]

RUN dotnet restore "src/Sneakers.Shop.Backend.Api/Sneakers.Shop.Backend.Api.csproj"

COPY src/ .

WORKDIR "/src/Sneakers.Shop.Backend.Api"
RUN dotnet build "Sneakers.Shop.Backend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sneakers.Shop.Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sneakers.Shop.Backend.Api.dll"]