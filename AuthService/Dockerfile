# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy project files and restore dependencies first for better caching
COPY ["AuthService/AuthService.csproj", "AuthService/"]
COPY ["QuantityMeasurementAppModels/QuantityMeasurementAppModels.csproj", "QuantityMeasurementAppModels/"]
COPY ["QuantityMeasurementAppServices/QuantityMeasurementAppServices.csproj", "QuantityMeasurementAppServices/"]
COPY ["QuantityMeasurementAppRepositories/QuantityMeasurementAppRepositories.csproj", "QuantityMeasurementAppRepositories/"]
COPY ["QuantityMeasurementAppBusiness/QuantityMeasurementAppBusiness.csproj", "QuantityMeasurementAppBusiness/"]

RUN dotnet restore "AuthService/AuthService.csproj"

# Copy the remaining source and build
COPY . .
WORKDIR "/src/AuthService"
RUN dotnet build "AuthService.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Install globalization libraries (required for SQL Server on Alpine)
RUN apk add --no-cache icu-libs krb5-libs

# Set environment variables for globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]
