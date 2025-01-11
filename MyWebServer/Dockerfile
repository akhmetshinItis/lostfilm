FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
EXPOSE 8888

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyWebServer/MyWebServer.csproj", "MyWebServer/"]
COPY ["HttpServerLibrary/HttpServerLibrary.csproj", "HttpServerLibrary/"]
COPY ["MyORMLibrary/MyORMLibrary.csproj", "MyORMLibrary/"]
COPY ["TemlateEngine/TemlateEngine.csproj", "TemlateEngine/"]
RUN dotnet restore "MyWebServer/MyWebServer.csproj"
COPY . .
WORKDIR "/src/MyWebServer"
RUN dotnet build "MyWebServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyWebServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebServer.dll"]
