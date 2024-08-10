# Используем базовый образ Ubuntu
FROM ubuntu:20.04 AS build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

ENV DOTNET_USE_POLLING_FILE_WATCHER = true
# Обновляем список пакетов и устанавливаем необходимые зависимости
RUN apt-get update && \
    apt-get install -y \
    curl \
    wget \
    gnupg \
    apt-transport-https \
    ca-certificates \
    software-properties-common

# Добавляем Microsoft PPA
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb

# Обновляем список пакетов и устанавливаем .NET SDK 8.0
RUN apt-get update && \
    apt-get install -y dotnet-sdk-8.0

# Устанавливаем рабочую директорию
WORKDIR /app/backend/WebAPI

# Копируем файлы проекта
COPY . .

# Восстанавливаем зависимости
RUN dotnet restore Buckshout.csproj

# Собираем проект
ARG BUILD_CONFIGURATION=Release
RUN dotnet build ./Buckshout.csproj -c $BUILD_CONFIGURATION -o /app/build

RUN dotnet publish ./Buckshout.csproj -c $BUILD_CONFIGURATION -o /app/publish 
# # # Открываем порты
EXPOSE 5000
EXPOSE 5001

# # # Запускаем приложение
ENV ASPNETCORE_URLS=http://+:5000;  