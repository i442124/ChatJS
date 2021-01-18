# pull official base image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base

# set working directory
WORKDIR /app

# Expose port 80 to your local machine so you can access the app.
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS restore

## fetch and install Node
RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs

# copy config files
COPY common.props .
COPY stylecop.json .
COPY stylecop.props .
COPY stylecop.ruleset .

# copy source files
COPY src/ChatJS.Data/. ./src/ChatJS.Data
COPY src/ChatJS.Domain/. ./src/ChatJS.Domain
COPY src/ChatJS.Models/. ./src/ChatJS.Models
COPY src/ChatJS.WebApp/. ./src/ChatJS.WebApp
COPY src/ChatJS.WebServer/. ./src/ChatJS.WebServer

# restore project
RUN dotnet restore "src/ChatJS.WebServer/ChatJS.WebServer.csproj"

# build project
FROM restore AS build
RUN dotnet build "src/ChatJS.WebServer/ChatJS.WebServer.csproj" -c Release -o app/build

# publish project
FROM build AS publish
RUN dotnet publish "src/ChatJS.WebServer/ChatJS.WebServer.csproj" -c Release -o app/publish

# start project
FROM base as final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatJS.WebServer.dll"]