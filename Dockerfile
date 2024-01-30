FROM mcr.microsoft.com/dotnet/aspnet:6.0.26-bookworm-slim-arm64v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.418-bookworm-slim-arm64v8 AS build
WORKDIR /src
COPY ["Server/BlazorApp.Server.csproj", "."]
RUN dotnet restore "BlazorApp.Server.csproj"
COPY . .
RUN dotnet build "Server/BlazorApp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server/BlazorApp.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorApp.Server.dll"]