# Скрипт быстрого запуска системы управления аквапарком
# Запустите этот скрипт для автоматической настройки и запуска приложения

Write-Host "🌊 Система управления аквапарком 'Водный мир'" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan

# Проверяем наличие .NET
Write-Host "`n🔍 Проверка .NET 8.0..." -ForegroundColor Yellow
try {
    $dotnetVersion = & dotnet --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ .NET найден: $dotnetVersion" -ForegroundColor Green
    } else {
        throw ".NET не найден"
    }
} catch {
    Write-Host "❌ Ошибка: .NET 8.0 не установлен" -ForegroundColor Red
    Write-Host "Скачайте и установите .NET 8.0 SDK с https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

# Проверяем наличие PostgreSQL
Write-Host "`n🔍 Проверка PostgreSQL..." -ForegroundColor Yellow
try {
    $psqlVersion = & psql --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ PostgreSQL найден: $psqlVersion" -ForegroundColor Green
    } else {
        throw "PostgreSQL не найден"
    }
} catch {
    Write-Host "❌ Ошибка: PostgreSQL не установлен или не добавлен в PATH" -ForegroundColor Red
    Write-Host "Установите PostgreSQL и добавьте его в PATH" -ForegroundColor Yellow
    Write-Host "Скачать можно с: https://www.postgresql.org/download/windows/" -ForegroundColor Cyan
    exit 1
}

# Проверяем подключение к PostgreSQL
Write-Host "`n🔍 Проверка подключения к PostgreSQL..." -ForegroundColor Yellow
$connectionTest = & psql -U postgres -h localhost -c "SELECT 1;" 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Ошибка подключения к PostgreSQL" -ForegroundColor Red
    Write-Host "Убедитесь, что PostgreSQL запущен и пароль правильный" -ForegroundColor Yellow
    Write-Host "Попробуйте запустить: net start postgresql-x64-14" -ForegroundColor Cyan
    exit 1
}
Write-Host "✅ Подключение к PostgreSQL успешно!" -ForegroundColor Green

# Проверяем наличие базы данных
Write-Host "`n🔍 Проверка базы данных..." -ForegroundColor Yellow
$dbExists = & psql -U postgres -h localhost -d postgres -t -c "SELECT 1 FROM pg_database WHERE datname = 'aquapark_db';" 2>$null
if ($dbExists -match "1") {
    Write-Host "✅ База данных 'aquapark_db' найдена" -ForegroundColor Green
} else {
    Write-Host "⚠️ База данных 'aquapark_db' не найдена" -ForegroundColor Yellow
    Write-Host "Создаем базу данных..." -ForegroundColor Yellow
    
    & psql -U postgres -h localhost -d postgres -c "CREATE DATABASE aquapark_db;" 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ База данных создана успешно!" -ForegroundColor Green
        
        # Выполняем скрипты создания таблиц
        if (Test-Path "Database\setup_aquapark_database.sql") {
            Write-Host "📋 Создаем таблицы..." -ForegroundColor Yellow
            & psql -U postgres -h localhost -d aquapark_db -f "Database\setup_aquapark_database.sql" 2>$null
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✅ Таблицы созданы успешно!" -ForegroundColor Green
            } else {
                Write-Host "⚠️ Ошибка создания таблиц" -ForegroundColor Yellow
            }
        }
    } else {
        Write-Host "❌ Ошибка создания базы данных" -ForegroundColor Red
        exit 1
    }
}

# Восстанавливаем пакеты NuGet
Write-Host "`n📦 Восстановление пакетов NuGet..." -ForegroundColor Yellow
& dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Пакеты восстановлены успешно!" -ForegroundColor Green
} else {
    Write-Host "❌ Ошибка восстановления пакетов" -ForegroundColor Red
    exit 1
}

# Собираем проект
Write-Host "`n🔨 Сборка проекта..." -ForegroundColor Yellow
& dotnet build --configuration Release
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Проект собран успешно!" -ForegroundColor Green
} else {
    Write-Host "❌ Ошибка сборки проекта" -ForegroundColor Red
    exit 1
}

# Запускаем приложение
Write-Host "`n🚀 Запуск приложения..." -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "🌊 Добро пожаловать в систему управления аквапарком!" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan

& dotnet run --project AquaparkApp --configuration Release

Write-Host "`n👋 Спасибо за использование системы управления аквапарком!" -ForegroundColor Green
