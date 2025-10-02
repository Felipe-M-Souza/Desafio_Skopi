# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Use the official .NET 8 SDK as build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskManagementAPI.csproj", "."]
RUN dotnet restore "TaskManagementAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "TaskManagementAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagementAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementAPI.dll"]
