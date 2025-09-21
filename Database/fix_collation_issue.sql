-- Скрипт для исправления проблемы с collation в PostgreSQL
-- Выполните этот скрипт от имени пользователя postgres

-- 1. Обновляем версии collation для всех баз-шаблонов
ALTER DATABASE template1 REFRESH COLLATION VERSION;
ALTER DATABASE template0 REFRESH COLLATION VERSION;

-- 2. Если есть база postgres, обновляем и её
ALTER DATABASE postgres REFRESH COLLATION VERSION;

-- 3. Проверяем статус
SELECT datname, datcollversion FROM pg_database WHERE datname IN ('template0', 'template1', 'postgres');

-- 4. Если проблема не решается, можно пересоздать базы-шаблоны
-- (ВНИМАНИЕ: Это более радикальный подход)
/*
-- Останавливаем подключения к template1
UPDATE pg_database SET datallowconn = false WHERE datname = 'template1';

-- Завершаем все подключения к template1
SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'template1';

-- Пересоздаем template1
DROP DATABASE template1;
CREATE DATABASE template1 WITH TEMPLATE template0;

-- Восстанавливаем права
UPDATE pg_database SET datallowconn = true WHERE datname = 'template1';
*/
