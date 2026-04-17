FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY ["ApiGateway/ApiGateway.csproj", "ApiGateway/"]
COPY ["QuantityMeasurementAppModels/QuantityMeasurementAppModels.csproj", "QuantityMeasurementAppModels/"]
COPY ["QuantityMeasurementAppServices/QuantityMeasurementAppServices.csproj", "QuantityMeasurementAppServices/"]
COPY ["QuantityMeasurementAppRepositories/QuantityMeasurementAppRepositories.csproj", "QuantityMeasurementAppRepositories/"]
COPY ["QuantityMeasurementAppBusiness/QuantityMeasurementAppBusiness.csproj", "QuantityMeasurementAppBusiness/"]
RUN dotnet restore "ApiGateway/ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateway"
RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Install globalization libraries (required for SQL Server on Alpine)
RUN apk add --no-cache icu-libs krb5-libs

# Set environment variables for globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
