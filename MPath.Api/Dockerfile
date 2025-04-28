# Use the official .NET 8 runtime as the base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET 8 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies for all projects
COPY ["MPath.sln", "./"]
COPY ["MPath.Api/MPath.Api.csproj", "MPath.Api/"]
COPY ["MPath.Application/MPath.Application.csproj", "MPath.Application/"]
COPY ["MPath.Domain/MPath.Domain.csproj", "MPath.Domain/"]
COPY ["MPath.Infrastructure/MPath.Infrastructure.csproj", "MPath.Infrastructure/"]
COPY ["MPath.SharedKernel/MPath.SharedKernel.csproj", "MPath.SharedKernel/"]
RUN dotnet restore "MPath.Api/MPath.Api.csproj"

# Copy the entire source code and build the application
COPY . .
WORKDIR "/src/MPath.Api"
RUN dotnet build "MPath.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MPath.Api.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MPath.Api.dll"]