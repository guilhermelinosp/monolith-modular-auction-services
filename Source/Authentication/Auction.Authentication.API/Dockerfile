FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 443
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/Authentication/Auction.Authentication.API/Auction.Authentication.API.csproj", "Source/Authentication/Auction.Authentication.API/"]
COPY ["Source/Authentication/Auction.Authentication.Application/Auction.Authentication.Application.csproj", "Source/Authentication/Auction.Authentication.Application/"]
COPY ["Source/Authentication/Auction.Authentication.Domain/Auction.Authentication.Domain.csproj", "Source/Authentication/Auction.Authentication.Domain/"]
COPY ["Source/Authentication/Auction.Authentication.Infrastructure/Auction.Authentication.Infrastructure.csproj", "Source/Authentication/Auction.Authentication.Infrastructure/"]
RUN dotnet restore "Source/Authentication/Auction.Authentication.API/Auction.Authentication.API.csproj"
COPY . .
WORKDIR "/src/Source/Authentication/Auction.Authentication.API"
RUN dotnet build "Auction.Authentication.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Auction.Authentication.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auction.Authentication.API.dll"]
