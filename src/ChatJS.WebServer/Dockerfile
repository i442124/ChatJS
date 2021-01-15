FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
ENV ASPNETCORE_URLS=https://+:3002;http://+:3001
WORKDIR /app
EXPOSE 3002
EXPOSE 3001

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS restore

COPY src/ChatJS.Data/*.csproj ./src/ChatJS.Data/
COPY src/ChatJS.Domain/*.csproj ./src/ChatJS.Domain/
COPY src/ChatJS.Models/*.csproj ./src/ChatJS.Models/
COPY src/ChatJS.WebServer/*.csproj ./src/ChatJS.WebServer/
RUN dotnet restore "src/ChatJS.WebServer/ChatJS.WebServer.csproj"


FROM restore AS build
COPY . .
RUN dotnet build "ChatJS.Deployment.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatJS.Deployment.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatJS.WebServer.dll"]