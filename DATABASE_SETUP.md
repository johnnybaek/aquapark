# Настройка базы данных PostgreSQL для AquaparkApp

## Требования
- PostgreSQL 12 или выше
- Пользователь `postgres` с паролем `postgres`

## Установка PostgreSQL

### Windows
1. Скачайте PostgreSQL с официального сайта: https://www.postgresql.org/download/windows/
2. Установите PostgreSQL с настройками по умолчанию
3. Запомните пароль для пользователя `postgres` (по умолчанию `postgres`)

### Linux (Ubuntu/Debian)
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

## Настройка базы данных

1. **Подключитесь к PostgreSQL как пользователь postgres:**
   ```bash
   sudo -u postgres psql
   ```

2. **Создайте базу данных и таблицы:**
   ```bash
   # Выполните скрипты в следующем порядке:
   \i Database/create_database.sql
   \i Database/create_tables.sql
   \i Database/insert_sample_data.sql
   ```

3. **Или выполните все команды вручную:**
   ```sql
   -- Создание базы данных
   CREATE DATABASE aquapark_db
       WITH 
       OWNER = postgres
       ENCODING = 'UTF8'
       LC_COLLATE = 'ru_RU.UTF-8'
       LC_CTYPE = 'ru_RU.UTF-8'
       TABLESPACE = pg_default
       CONNECTION LIMIT = -1;

   -- Подключение к базе данных
   \c aquapark_db;

   -- Создание расширений
   CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
   ```

4. **Создайте таблицы, выполнив содержимое файла `Database/create_tables.sql`**

5. **Добавьте тестовые данные, выполнив содержимое файла `Database/insert_sample_data.sql`**

## Проверка подключения

После настройки базы данных запустите приложение. Если база данных настроена правильно, приложение должно запуститься без ошибок подключения.

## Настройка строки подключения

Если вы используете другие настройки PostgreSQL, измените строку подключения в файле `AquaparkApp.DAL/DatabaseConnection.cs`:

```csharp
private static readonly string ConnectionString = "Host=localhost;Database=aquapark_db;Username=postgres;Password=postgres;Port=5432;";
```

Замените параметры на ваши:
- `Host` - адрес сервера PostgreSQL
- `Database` - имя базы данных
- `Username` - имя пользователя
- `Password` - пароль пользователя
- `Port` - порт PostgreSQL (по умолчанию 5432)

## Устранение неполадок

### Ошибка "connection refused"
- Убедитесь, что PostgreSQL запущен
- Проверьте, что порт 5432 открыт
- Проверьте настройки файрвола

### Ошибка "authentication failed"
- Проверьте имя пользователя и пароль
- Убедитесь, что пользователь имеет права на базу данных

### Ошибка "database does not exist"
- Создайте базу данных `aquapark_db`
- Выполните скрипты создания таблиц

## Альтернативный способ (без PostgreSQL)

Если вы не хотите устанавливать PostgreSQL, можно временно изменить код для работы без базы данных или использовать SQLite. Для этого нужно:

1. Изменить строку подключения на SQLite
2. Обновить код репозиториев для работы с SQLite
3. Создать соответствующие таблицы SQLite

Однако рекомендуется использовать PostgreSQL для полной функциональности приложения.
