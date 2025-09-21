# Скрипт для исправления проблемы с collation в PostgreSQL
# Запустите этот скрипт от имени администратора

Write-Host "🔧 Исправление проблемы с collation в PostgreSQL" -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan

# Путь к psql
$psqlPath = "C:\Program Files\PostgreSQL\17\bin\psql.exe"

# Проверяем наличие psql
if (-not (Test-Path $psqlPath)) {
    Write-Host "❌ PostgreSQL не найден по пути: $psqlPath" -ForegroundColor Red
    Write-Host "Убедитесь, что PostgreSQL установлен правильно" -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ PostgreSQL найден: $psqlPath" -ForegroundColor Green

# Функция для выполнения SQL команд
function Execute-SQL {
    param(
        [string]$Command,
        [string]$Password = "postgres"
    )
    
    $env:PGPASSWORD = $Password
    try {
        $result = & $psqlPath -U postgres -c $Command 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Команда выполнена успешно" -ForegroundColor Green
            return $true
        } else {
            Write-Host "❌ Ошибка выполнения команды: $result" -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Host "❌ Исключение при выполнении команды: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Попробуем разные пароли
$passwords = @("postgres", "123", "admin", "password", "")

Write-Host "`n🔍 Попытка подключения к PostgreSQL..." -ForegroundColor Yellow

$connected = $false
foreach ($password in $passwords) {
    Write-Host "Пробуем пароль: $password" -ForegroundColor Yellow
    
    $env:PGPASSWORD = $password
    $testResult = & $psqlPath -U postgres -c "SELECT version();" 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Подключение успешно с паролем: $password" -ForegroundColor Green
        $connected = $true
        break
    } else {
        Write-Host "❌ Не удалось подключиться с паролем: $password" -ForegroundColor Red
    }
}

if (-not $connected) {
    Write-Host "`n❌ Не удалось подключиться к PostgreSQL" -ForegroundColor Red
    Write-Host "Возможные решения:" -ForegroundColor Yellow
    Write-Host "1. Проверьте, что PostgreSQL запущен" -ForegroundColor Yellow
    Write-Host "2. Убедитесь в правильности пароля пользователя postgres" -ForegroundColor Yellow
    Write-Host "3. Проверьте настройки pg_hba.conf" -ForegroundColor Yellow
    Write-Host "4. Попробуйте сбросить пароль postgres" -ForegroundColor Yellow
    exit 1
}

Write-Host "`n🔧 Исправление проблемы с collation..." -ForegroundColor Yellow

# Команды для исправления collation
$commands = @(
    "ALTER DATABASE template1 REFRESH COLLATION VERSION;",
    "ALTER DATABASE template0 REFRESH COLLATION VERSION;",
    "ALTER DATABASE postgres REFRESH COLLATION VERSION;"
)

foreach ($command in $commands) {
    Write-Host "Выполняем: $command" -ForegroundColor Yellow
    if (Execute-SQL -Command $command) {
        Write-Host "✅ Команда выполнена успешно" -ForegroundColor Green
    } else {
        Write-Host "⚠️ Команда не выполнена, но продолжаем..." -ForegroundColor Yellow
    }
}

# Проверяем результат
Write-Host "`n🔍 Проверка результата..." -ForegroundColor Yellow
$checkCommand = "SELECT datname, datcollversion FROM pg_database WHERE datname IN ('template0', 'template1', 'postgres');"
Execute-SQL -Command $checkCommand

Write-Host "`n🎯 Попытка создания тестовой базы данных..." -ForegroundColor Yellow
$testDbCommand = "CREATE DATABASE test_collation_db;"
if (Execute-SQL -Command $testDbCommand) {
    Write-Host "✅ Тестовая база создана успешно!" -ForegroundColor Green
    
    # Удаляем тестовую базу
    Execute-SQL -Command "DROP DATABASE test_collation_db;"
    Write-Host "✅ Тестовая база удалена" -ForegroundColor Green
    
    Write-Host "`n🎉 Проблема с collation исправлена!" -ForegroundColor Green
    Write-Host "Теперь можно создавать базу данных для аквапарка" -ForegroundColor Green
} else {
    Write-Host "`n⚠️ Проблема с collation может остаться" -ForegroundColor Yellow
    Write-Host "Попробуйте перезапустить службу PostgreSQL" -ForegroundColor Yellow
    Write-Host "Или обратитесь к администратору базы данных" -ForegroundColor Yellow
}

Write-Host "`n📋 Следующие шаги:" -ForegroundColor Cyan
Write-Host "1. Запустите скрипт создания базы данных аквапарка" -ForegroundColor White
Write-Host "2. Или выполните: .\Запуск_приложения.ps1" -ForegroundColor White
