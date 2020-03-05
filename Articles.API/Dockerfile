#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Articles.API/Articles.API.csproj", "Articles.API/"]
COPY ["Articles.Services/Articles.Services.csproj", "Articles.Services/"]
COPY ["Articles.Data/Articles.Data.csproj", "Articles.Data/"]
RUN dotnet restore "Articles.API/Articles.API.csproj"
COPY . .
WORKDIR "/src/Articles.API"
RUN dotnet build "Articles.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Articles.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Articles.API.dll"]