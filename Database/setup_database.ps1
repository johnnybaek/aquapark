# Скрипт для автоматической настройки PostgreSQL базы данных аквапарка
# Запустите этот скрипт от имени администратора

param(
    [string]$PostgresPassword = "postgres",
    [string]$DatabaseName = "aquapark_db",
    [string]$Username = "postgres"
)

Write-Host "=== Настройка базы данных аквапарка ===" -ForegroundColor Green

# Проверяем наличие PostgreSQL
try {
    $psqlVersion = & psql --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "PostgreSQL найден: $psqlVersion" -ForegroundColor Green
    } else {
        throw "PostgreSQL не найден"
    }
} catch {
    Write-Host "Ошибка: PostgreSQL не установлен или не добавлен в PATH" -ForegroundColor Red
    Write-Host "Пожалуйста, установите PostgreSQL и добавьте его в PATH" -ForegroundColor Yellow
    Write-Host "Скачать можно с: https://www.postgresql.org/download/windows/" -ForegroundColor Cyan
    exit 1
}

# Проверяем подключение к PostgreSQL
Write-Host "Проверка подключения к PostgreSQL..." -ForegroundColor Yellow
$connectionTest = & psql -U $Username -h localhost -c "SELECT version();" 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "Ошибка подключения к PostgreSQL" -ForegroundColor Red
    Write-Host "Убедитесь, что PostgreSQL запущен и пароль правильный" -ForegroundColor Yellow
    exit 1
}

Write-Host "Подключение к PostgreSQL успешно!" -ForegroundColor Green

# Создаем базу данных если не существует
Write-Host "Создание базы данных '$DatabaseName'..." -ForegroundColor Yellow
$createDbQuery = "SELECT 1 FROM pg_database WHERE datname = '$DatabaseName';"
$dbExists = & psql -U $Username -h localhost -d postgres -t -c $createDbQuery 2>$null

if ($dbExists -match "1") {
    Write-Host "База данных '$DatabaseName' уже существует" -ForegroundColor Yellow
} else {
    & psql -U $Username -h localhost -d postgres -c "CREATE DATABASE $DatabaseName;" 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "База данных '$DatabaseName' создана успешно!" -ForegroundColor Green
    } else {
        Write-Host "Ошибка создания базы данных" -ForegroundColor Red
        exit 1
    }
}

# Выполняем скрипт создания таблиц
Write-Host "Создание таблиц..." -ForegroundColor Yellow
$setupScript = Join-Path $PSScriptRoot "setup_aquapark_database.sql"
if (Test-Path $setupScript) {
    & psql -U $Username -h localhost -d $DatabaseName -f $setupScript 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Таблицы созданы успешно!" -ForegroundColor Green
    } else {
        Write-Host "Ошибка создания таблиц" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "Файл setup_aquapark_database.sql не найден" -ForegroundColor Red
    exit 1
}

Write-Host "`n=== Настройка завершена! ===" -ForegroundColor Green
Write-Host "База данных '$DatabaseName' готова к использованию" -ForegroundColor Green
Write-Host "Строка подключения: Host=localhost;Database=$DatabaseName;Username=$Username;Password=$PostgresPassword;Port=5432;" -ForegroundColor Cyan
