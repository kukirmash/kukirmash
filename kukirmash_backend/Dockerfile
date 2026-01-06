FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
# Создаем структуру папок для загружаемых файлов, чтобы не было ошибок доступа
RUN mkdir -p /app/wwwroot/media/server-icons

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем csproj (пути верные, исходя из твоей структуры)
COPY Kukirmash.API/Kukirmash.API.csproj Kukirmash.API/
COPY Kukirmash.Application/Kukirmash.Application.csproj Kukirmash.Application/
COPY Kukirmash.Core/Kukirmash.Core.csproj Kukirmash.Core/
COPY Kukirmash.Infrastructure/Kukirmash.Infrastructure.csproj Kukirmash.Infrastructure/
COPY Kukirmash.Persistence/Kukirmash.Persistence.csproj Kukirmash.Persistence/

RUN dotnet restore Kukirmash.API/Kukirmash.API.csproj

COPY . .

RUN dotnet publish Kukirmash.API/Kukirmash.API.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Kukirmash.API.dll"]