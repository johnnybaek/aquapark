# 🚀 Скрипт для создания дистрибутива AquaparkApp
# Уменьшает вероятность блокировки антивирусами

param(
    [string]$OutputPath = ".\Distribution",
    [string]$Version = "1.0.0"
)

Write-Host "🌊 Создание дистрибутива AquaparkApp v$Version" -ForegroundColor Cyan

# Создать папку для дистрибутива
if (Test-Path $OutputPath) {
    Remove-Item $OutputPath -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $OutputPath

Write-Host "📁 Создана папка: $OutputPath" -ForegroundColor Green

# Собрать приложение
Write-Host "🔨 Сборка приложения..." -ForegroundColor Yellow
Set-Location ".\AquaparkApp"

# Очистить предыдущие сборки
if (Test-Path ".\bin\Release") {
    Remove-Item ".\bin\Release" -Recurse -Force
}

# Собрать с оптимизированными настройками
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=false

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Ошибка сборки!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Сборка завершена успешно!" -ForegroundColor Green

# Вернуться в корневую папку
Set-Location ".."

# Скопировать файлы
$sourcePath = ".\AquaparkApp\bin\Release\net8.0-windows\win-x64\publish"
if (Test-Path $sourcePath) {
    Write-Host "📋 Копирование файлов..." -ForegroundColor Yellow
    Copy-Item "$sourcePath\*" -Destination $OutputPath -Recurse -Force
    Write-Host "✅ Файлы скопированы!" -ForegroundColor Green
} else {
    Write-Host "❌ Папка сборки не найдена: $sourcePath" -ForegroundColor Red
    exit 1
}

# Создать README файл
$readme = @"
# 🌊 Аквапарк Водный мир - Система управления

## 📋 Описание
Современное приложение для управления аквапарком с красивым интерфейсом в стиле macOS Sequoia.

## 🚀 Установка и запуск

### ⚠️ ВАЖНО! Антивирус может заблокировать файл

Если Windows Defender или другой антивирус блокирует приложение:

1. **Нажмите "Подробнее"** в предупреждении
2. **Выберите "Выполнить в любом случае"**
3. **Или добавьте в исключения:**
   - Откройте Windows Security
   - Перейдите в "Защита от вирусов и угроз"
   - Нажмите "Управление настройками"
   - Добавьте папку с приложением в исключения

### Альтернативные способы:
- Запустите от имени администратора
- Временно отключите антивирус
- Добавьте файл в исключения антивируса

## 💻 Системные требования
- Windows 10/11 (64-bit)
- .NET 8.0 Runtime (включен в дистрибутив)
- Минимум 4 GB RAM
- 500 MB свободного места на диске

## 🎯 Функции
- ✅ Управление аттракционами
- ✅ Бронирование билетов
- ✅ Система пользователей
- ✅ Админ-панель
- ✅ Отчеты и аналитика
- ✅ Красивый дизайн в стиле macOS

## 📞 Поддержка
- Email: support@aquapark.com
- Telegram: @aquapark_support
- Телефон: +7 (XXX) XXX-XX-XX

## 📄 Лицензия
Copyright © 2024 Aquapark Management Systems

---
Версия: $Version
Дата сборки: $(Get-Date -Format "dd.MM.yyyy HH:mm")
"@

$readme | Out-File -FilePath "$OutputPath\README.txt" -Encoding UTF8
Write-Host "📄 Создан README.txt" -ForegroundColor Green

# Создать файл с инструкциями по антивирусу
$antivirusInstructions = @"
# 🛡️ Решение проблем с антивирусами

## Windows Defender
1. Откройте Windows Security
2. Перейдите в "Защита от вирусов и угроз"
3. Нажмите "Управление настройками"
4. Добавьте папку с приложением в исключения

## Другие антивирусы
- Kaspersky: Настройки → Угрозы и исключения → Исключения
- Avast: Настройки → Исключения → Добавить исключение
- Norton: Настройки → Антивирус → Исключения
- McAfee: Настройки → Исключения → Добавить файл/папку

## Если ничего не помогает:
1. Запустите от имени администратора
2. Временно отключите антивирус
3. Скачайте файл повторно
4. Обратитесь в поддержку

## Почему антивирус блокирует?
- Файл не подписан цифровой подписью
- Антивирус не знает о файле
- Подозрительные библиотеки (QRCoder, EPPlus)
- Отсутствует информация о производителе

Это нормально для неопубликованных приложений!
"@

$antivirusInstructions | Out-File -FilePath "$OutputPath\ANTIVIRUS_HELP.txt" -Encoding UTF8
Write-Host "🛡️ Создан ANTIVIRUS_HELP.txt" -ForegroundColor Green

# Создать ZIP архив
$zipName = "AquaparkApp_v$Version.zip"
if (Test-Path $zipName) {
    Remove-Item $zipName -Force
}

Write-Host "📦 Создание ZIP архива..." -ForegroundColor Yellow
Compress-Archive -Path "$OutputPath\*" -DestinationPath $zipName -Force

if (Test-Path $zipName) {
    $zipSize = (Get-Item $zipName).Length / 1MB
    Write-Host "✅ ZIP архив создан: $zipName ($([math]::Round($zipSize, 2)) MB)" -ForegroundColor Green
} else {
    Write-Host "❌ Ошибка создания ZIP архива!" -ForegroundColor Red
}

# Создать батник для запуска
$batContent = @"
@echo off
title Аквапарк Водный мир
echo.
echo 🌊 Запуск Аквапарк Водный мир...
echo.
echo ⚠️  Если антивирус блокирует приложение:
echo    1. Нажмите "Подробнее" в предупреждении
echo    2. Выберите "Выполнить в любом случае"
echo    3. Или добавьте папку в исключения антивируса
echo.
echo 📞 Поддержка: support@aquapark.com
echo.
pause
start "" "AquaparkApp.exe"
"@

$batContent | Out-File -FilePath "$OutputPath\Запуск.bat" -Encoding ASCII
Write-Host "🚀 Создан батник для запуска" -ForegroundColor Green

# Создать файл с информацией о сборке
$buildInfo = @"
# Информация о сборке

Версия: $Version
Дата сборки: $(Get-Date -Format "dd.MM.yyyy HH:mm")
Платформа: Windows x64
.NET Runtime: 8.0 (включен)
Режим сборки: Release
Оптимизации: ReadyToRun, SelfContained

## Файлы в дистрибутиве:
- AquaparkApp.exe - Основное приложение
- README.txt - Инструкции по установке
- ANTIVIRUS_HELP.txt - Помощь с антивирусами
- Запуск.bat - Батник для запуска
- *.dll - Библиотеки .NET
- *.pdb - Отладочная информация

## Размеры файлов:
"@

# Добавить информацию о размерах файлов
Get-ChildItem $OutputPath | ForEach-Object {
    $size = $_.Length / 1MB
    $buildInfo += "`n- $($_.Name): $([math]::Round($size, 2)) MB"
}

$buildInfo | Out-File -FilePath "$OutputPath\BUILD_INFO.txt" -Encoding UTF8
Write-Host "📊 Создан BUILD_INFO.txt" -ForegroundColor Green

# Показать итоговую информацию
Write-Host "`n🎉 Дистрибутив создан успешно!" -ForegroundColor Green
Write-Host "📁 Папка: $OutputPath" -ForegroundColor Cyan
Write-Host "📦 ZIP архив: $zipName" -ForegroundColor Cyan
Write-Host "📄 Файлы:" -ForegroundColor Yellow
Get-ChildItem $OutputPath | ForEach-Object {
    $size = $_.Length / 1MB
    Write-Host "   - $($_.Name) ($([math]::Round($size, 2)) MB)" -ForegroundColor White
}

Write-Host "`n💡 Рекомендации:" -ForegroundColor Yellow
Write-Host "   1. Протестируйте на разных компьютерах" -ForegroundColor White
Write-Host "   2. Предоставьте инструкции пользователям" -ForegroundColor White
Write-Host "   3. Рассмотрите получение цифрового сертификата" -ForegroundColor White
Write-Host "   4. Создайте установщик для профессионального вида" -ForegroundColor White

Write-Host "`n🛡️ Для решения проблем с антивирусами:" -ForegroundColor Yellow
Write-Host "   - Используйте ZIP архив вместо .exe" -ForegroundColor White
Write-Host "   - Предоставьте четкие инструкции" -ForegroundColor White
Write-Host "   - Рассмотрите подпись кода" -ForegroundColor White

Write-Host "`n✨ Готово! Дистрибутив готов к распространению." -ForegroundColor Green
