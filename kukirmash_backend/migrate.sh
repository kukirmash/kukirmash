#!/bin/bash

# Путь к папке с бэкендом (исправь, если путь отличается)
PROJECT_DIR=~/kukirmash/kukirmash_backend

# Переходим в папку проекта
cd "$PROJECT_DIR" || { echo "❌ Ошибка: Не удалось найти папку $PROJECT_DIR"; exit 1; }

# Запрашиваем название миграции
echo -n "Введите название миграции: "
read MIGRATION_NAME

# Проверяем, что название не пустое
if [ -z "$MIGRATION_NAME" ]; then
    echo "❌ Ошибка: Название миграции не может быть пустым."
    exit 1
fi

echo "🚀 Создаем миграцию '$MIGRATION_NAME'..."
dotnet ef migrations add "$MIGRATION_NAME" -s Kukirmash.API -p Kukirmash.Persistence

# Проверяем код возврата (успешно ли прошла команда)
if [ $? -ne 0 ]; then
    echo "❌ Ошибка при создании миграции."
    exit 1
fi

echo "📦 Обновляем базу данных..."
dotnet ef database update -s Kukirmash.API -p Kukirmash.Persistence

if [ $? -eq 0 ]; then
    echo "✅ Готово! База данных обновлена."
else
    echo "❌ Ошибка при обновлении базы данных."
    exit 1
fi
