# # Базовый образ Node.js
# FROM node:14

# # Устанавливаем Nginx
# # RUN apt-get update && \
# #     apt-get install -y nginx

# # Копируем файлы приложения
# WORKDIR /app/frontend
# COPY package*.json ./

# # Устанавливаем зависимости
# RUN npm install

# COPY . .

# # Запускаем фронтенд, должен быть вообще билд
# RUN npm run dev

# # Копируем собранные файлы в директорию Nginx
# # RUN cp -r build /usr/share/nginx/html

# # Открываем порты
# # EXPOSE 80 # По идее этот порт нужен для nginx но я его пока вырезал
# EXPOSE 5173

# # CMD для запуска Nginx
# # CMD ["nginx", "-g", "daemon off;"

# Используем базовый образ Node.js 14
FROM node:14

# Устанавливаем рабочую директорию
WORKDIR /app/client

# Копируем package.json и package-lock.json
COPY ./client/package*.json ./

# Устанавливаем зависимости
RUN npm install

# Копируем остальные файлы проекта
COPY ./client .

# Открываем порт
EXPOSE 3000

# Запускаем приложение
CMD ["npm", "run", "dev"]