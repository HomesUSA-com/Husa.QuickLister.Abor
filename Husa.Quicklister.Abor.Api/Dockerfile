# Use the official .NET 8 SDK image as the base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG FEED_ACCESSTOKEN

# Install artifacts cred provider for Azure Devops Feed
RUN wget -qO- https://aka.ms/install-artifacts-credprovider.sh | bash

WORKDIR /app
EXPOSE 443
EXPOSE 8080

# Set environment variable for authentication
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesusacom/HomesUSA/_packaging/homesusa-dev/nuget/v3/index.json\", \"username\":\"build\", \"password\":\"${FEED_ACCESSTOKEN}\"}]}"

# Copy files and restore dependencies
COPY . .

RUN dotnet restore "Husa.Quicklister.Abor.Api/Husa.Quicklister.Abor.Api.csproj"

# Build the project
RUN dotnet build "Husa.Quicklister.Abor.Api/Husa.Quicklister.Abor.Api.csproj" -c Release --no-restore

# Publish the project
RUN dotnet publish "Husa.Quicklister.Abor.Api/Husa.Quicklister.Abor.Api.csproj" -c Release -o /app/publish --no-restore

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 7016
# Specify the entry point for the container
ENTRYPOINT ["dotnet", "Husa.Quicklister.Abor.Api.dll"]
