FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CeCursos/CeCursos.csproj", "CeCursos/"]
RUN dotnet restore "CeCursos/CeCursos.csproj"

COPY . .
WORKDIR /src/CeCursos
RUN dotnet publish "CeCursos.csproj" --configuration Release --output /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["sh", "-c", "dotnet CeCursos.dll --urls http://0.0.0.0:${PORT:-10000}"]
