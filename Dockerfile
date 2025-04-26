# filepath: /Users/seanghorn/Documents/Project/mpath-assessment/MPath.Api/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MPath.Api/MPath.Api.csproj", "MPath.Api/"]
RUN dotnet restore "MPath.Api/MPath.Api.csproj"
COPY . .
WORKDIR "/src/MPath.Api"
RUN dotnet build "MPath.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MPath.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MPath.Api.dll"]