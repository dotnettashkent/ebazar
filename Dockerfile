# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution file and restore dependencies
COPY ["ebazar.sln", "./"]
COPY ["Server/*.csproj", "./Server/"]
COPY ["Service/*.csproj", "./Service/"]
COPY ["Shared/*.csproj", "./Shared/"]
COPY ["TestConsole/*.csproj", "./TestConsole/"]
RUN dotnet restore ebazar.sln

# Copy the entire project and build the application
COPY . .

# Publish the application
WORKDIR /app
RUN dotnet publish -c Release -o out

# Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "Server.dll", "--update-database"]
