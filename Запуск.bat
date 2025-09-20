@echo off
chcp 65001 >nul
title 🌊 Аквапарк Водный мир - Система управления
color 0B

echo.
echo ╔══════════════════════════════════════════════════════════════╗
echo ║                    🌊 АКВАПАРК ВОДНЫЙ МИР                    ║
echo ║                  Система управления v1.0.0                   ║
echo ╚══════════════════════════════════════════════════════════════╝
echo.

echo 🚀 Запуск приложения...
echo.

echo ⚠️  ВАЖНО! Если антивирус блокирует приложение:
echo.
echo    1. Нажмите "Подробнее" в предупреждении
echo    2. Выберите "Выполнить в любом случае"
echo    3. Или добавьте папку в исключения антивируса
echo.

echo 📞 Поддержка: support@aquapark.com
echo    Telegram: @aquapark_support
echo.

echo 🔧 Системные требования:
echo    - Windows 10/11 (64-bit)
echo    - 4 GB RAM
echo    - 500 MB свободного места
echo.

echo 📋 Функции:
echo    ✅ Управление аттракционами
echo    ✅ Бронирование билетов
echo    ✅ Система пользователей
echo    ✅ Админ-панель
echo    ✅ Отчеты и аналитика
echo    ✅ Красивый дизайн
echo.

echo ⏳ Запуск через 5 секунд...
echo    (Нажмите любую клавишу для немедленного запуска)
echo.

timeout /t 5 /nobreak >nul 2>&1

echo 🎯 Запуск AquaparkApp.exe...
echo.

if exist "AquaparkApp.exe" (
    start "" "AquaparkApp.exe"
    echo ✅ Приложение запущено!
    echo.
    echo 💡 Если приложение не запустилось:
    echo    - Проверьте, не блокирует ли антивирус
    echo    - Запустите от имени администратора
    echo    - Обратитесь в поддержку
    echo.
) else (
    echo ❌ Файл AquaparkApp.exe не найден!
    echo.
    echo 🔍 Проверьте:
    echo    - Распакован ли архив полностью
    echo    - Находитесь ли в правильной папке
    echo    - Не удалил ли антивирус файл
    echo.
)

echo 📞 Нужна помощь?
echo    Email: support@aquapark.com
echo    Telegram: @aquapark_support
echo    Телефон: +7 (XXX) XXX-XX-XX
echo.

echo 🎉 Спасибо за использование Аквапарк Водный мир!
echo.

pause
