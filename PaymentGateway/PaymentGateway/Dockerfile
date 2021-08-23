FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS base-build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build 
COPY . ./
RUN dotnet publish -c Release -o outputDirectory

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=base-build-env /app/outputDirectory .
ENTRYPOINT ["dotnet", "PaymentGateway.dll"]