FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["./WebAppBackendCore.csproj", "WebAppBackendCore/"]
RUN dotnet restore "WebAppBackendCore/WebAppBackendCore.csproj"
COPY . "WebAppBackendCore/"
WORKDIR "/src/WebAppBackendCore"
RUN dotnet build "WebAppBackendCore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebAppBackendCore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebAppBackendCore.dll"]
