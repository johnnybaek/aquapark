# 🚀 Инструкции по сборке приложения "Аквапарк"

## 📋 Предварительные требования

### 1. Установка PostgreSQL
1. Скачайте PostgreSQL с официального сайта: https://www.postgresql.org/download/
2. Установите PostgreSQL (рекомендуется версия 12+)
3. Запомните пароль для пользователя `postgres`
4. Убедитесь, что служба PostgreSQL запущена

### 2. Установка .NET 8.0 SDK
1. Скачайте .NET 8.0 SDK с официального сайта: https://dotnet.microsoft.com/download
2. Установите SDK
3. Проверьте установку командой: `dotnet --version`

### 3. Установка Visual Studio (рекомендуется)
1. Скачайте Visual Studio 2022 Community (бесплатная версия)
2. Установите с компонентами:
   - .NET desktop development
   - Data storage and processing tools

## 🗄️ Настройка базы данных

### 1. Создание базы данных
1. Откройте pgAdmin или psql
2. Подключитесь к серверу PostgreSQL
3. Создайте базу данных:
```sql
CREATE DATABASE aquapark_db;
```

### 2. Выполнение SQL скриптов
Выполните скрипты в следующем порядке:

1. **create_database.sql** - создание базы данных (если не создали вручную)
2. **create_tables.sql** - создание таблиц и индексов
3. **insert_sample_data.sql** - вставка тестовых данных

### 3. Проверка подключения
Убедитесь, что можете подключиться к базе данных с параметрами:
- Host: localhost
- Port: 5432
- Database: aquapark_db
- Username: postgres
- Password: [ваш пароль]

## 🔧 Настройка проекта

### 1. Открытие проекта
1. Откройте файл `AquaparkApp.sln` в Visual Studio
2. Дождитесь восстановления NuGet пакетов

### 2. Обновление строки подключения
Найдите и обновите строки подключения в следующих файлах:
- `AquaparkApp.DAL/DatabaseConnection.cs`
- `AquaparkApp/Forms/MainForm.cs`
- `AquaparkApp/Forms/LoginForm.cs`
- `AquaparkApp/Forms/RegisterForm.cs`
- `AquaparkApp/Forms/ProfileForm.cs`
- `AquaparkApp/Forms/ChangePasswordForm.cs`
- `AquaparkApp/Forms/AdminPanelForm.cs`

Замените:
```csharp
var connectionString = "Host=localhost;Database=aquapark_db;Username=postgres;Password=password";
```

На ваши параметры подключения.

## 🏗️ Сборка приложения

### 1. В Visual Studio
1. Выберите конфигурацию `Release`
2. Выберите платформу `Any CPU`
3. Нажмите `Build` → `Build Solution` (Ctrl+Shift+B)
4. Убедитесь, что сборка прошла успешно

### 2. Через командную строку
```bash
cd "D:\aquapark\aquapark\aquapark-1"
dotnet build --configuration Release
```

## 🚀 Запуск приложения

### 1. Из Visual Studio
1. Нажмите `F5` или `Debug` → `Start Debugging`
2. Приложение должно запуститься

### 2. Из командной строки
```bash
cd "D:\aquapark\aquapark\aquapark-1\AquaparkApp"
dotnet run
```

### 3. Запуск готового exe файла
После сборки в Release режиме:
```bash
cd "D:\aquapark\aquapark\aquapark-1\AquaparkApp\bin\Release\net6.0-windows"
AquaparkApp.exe
```

## 📦 Создание установочного пакета

### 1. Опубликование приложения
```bash
cd "D:\aquapark\aquapark\aquapark-1\AquaparkApp"
dotnet publish -c Release -r win-x64 --self-contained true
```

### 2. Создание MSI пакета (опционально)
Можно использовать WiX Toolset для создания MSI установщика.

## 🔍 Тестирование

### 1. Тестовые пользователи
После выполнения `insert_sample_data.sql` доступны:
- **Администратор**: admin / admin (пароль: admin)
- **Пользователь**: ivanov / ivanov (пароль: ivanov)

### 2. Проверка функций
1. ✅ Регистрация нового пользователя
2. ✅ Вход в систему
3. ✅ Просмотр аттракционов
4. ✅ Бронирование билетов
5. ✅ Просмотр профиля
6. ✅ Админ-панель (только для admin)
7. ✅ Генерация отчетов
8. ✅ Видеопроигрыватель

## 🐛 Устранение неполадок

### Ошибка подключения к БД
- Проверьте, что PostgreSQL запущен
- Проверьте правильность строки подключения
- Убедитесь, что база данных `aquapark_db` существует

### Ошибки компиляции
- Восстановите NuGet пакеты: `dotnet restore`
- Очистите решение: `dotnet clean`
- Пересоберите: `dotnet build`

### Ошибки времени выполнения
- Проверьте, что все DLL файлы находятся в папке с exe
- Убедитесь, что .NET 8.0 Runtime установлен

## 📁 Структура выходных файлов

После сборки в папке `bin\Release\net8.0-windows` будут:
- `AquaparkApp.exe` - основной исполняемый файл
- `AquaparkApp.dll` - основная библиотека
- `AquaparkApp.BLL.dll` - бизнес-логика
- `AquaparkApp.DAL.dll` - доступ к данным
- `AquaparkApp.Models.dll` - модели данных
- Различные NuGet пакеты

## 🎯 Готово!

Приложение готово к использованию! 

**Важно**: Убедитесь, что PostgreSQL запущен перед запуском приложения, иначе возникнут ошибки подключения к базе данных.

## 🆕 Обновления для .NET 8.0

Проект обновлен до .NET 8.0 с улучшениями:
- **Улучшенная производительность** - .NET 8.0 быстрее предыдущих версий
- **Новые возможности C#** - доступ к последним языковым возможностям
- **Обновленные пакеты** - все NuGet пакеты обновлены до совместимых версий
- **Лучшая совместимость** - с современными Windows версиями
