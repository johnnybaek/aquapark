-- Создание базы данных для аквапарка
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
