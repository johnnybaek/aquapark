# 🚀 Настройка GitHub для командной работы

## 📋 План действий

### **1. Создание репозитория**
### **2. Настройка веток для отделов**
### **3. Создание GitHub Actions**
### **4. Настройка прав доступа**
### **5. Документация для команд**

---

## 🎯 Шаг 1: Создание репозитория

### **Создать новый репозиторий на GitHub:**

1. **Перейти на GitHub.com**
2. **Нажать "New repository"**
3. **Заполнить данные:**
   - **Repository name:** `aquapark-management-system`
   - **Description:** `🌊 Система управления аквапарком с красивым дизайном в стиле macOS Sequoia`
   - **Visibility:** Private (для команды) или Public (если хотите показать)
   - **Initialize with:** README, .gitignore (.NET), License (MIT)

### **Команды для первого коммита:**
```bash
# Клонировать репозиторий
git clone https://github.com/ваш-username/aquapark-management-system.git
cd aquapark-management-system

# Добавить все файлы
git add .

# Сделать первый коммит
git commit -m "🎉 Initial commit: Aquapark Management System

- ✅ Windows Forms приложение
- ✅ Веб-компоненты (HTML/CSS/JS)
- ✅ База данных PostgreSQL
- ✅ Дизайн в стиле macOS Sequoia
- ✅ Решение проблем с антивирусами
- ✅ Документация для команд"

# Отправить на GitHub
git push origin main
```

---

## 🌿 Шаг 2: Настройка веток для отделов

### **Создать ветки для каждого отдела:**

```bash
# Ветка для отдела дизайна
git checkout -b design/macos-sequoia-theme
git push origin design/macos-sequoia-theme

# Ветка для отдела разработки БД
git checkout -b database/optimization
git push origin database/optimization

# Ветка для разработки функций
git checkout -b feature/new-features
git push origin feature/new-features

# Ветка для исправления багов
git checkout -b bugfix/antivirus-issues
git push origin bugfix/antivirus-issues

# Вернуться на main
git checkout main
```

### **Структура веток:**
- **`main`** - основная ветка (только стабильный код)
- **`design/*`** - ветки для отдела дизайна
- **`database/*`** - ветки для отдела БД
- **`feature/*`** - новые функции
- **`bugfix/*`** - исправления багов
- **`hotfix/*`** - критические исправления

---

## ⚙️ Шаг 3: GitHub Actions для автоматической сборки

### **Создать файл `.github/workflows/build.yml`:**
```yaml
name: 🏗️ Build and Test

on:
  push:
    branches: [ main, design/*, database/*, feature/*, bugfix/* ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: windows-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish AquaparkApp/AquaparkApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
    
    - name: Upload artifacts
      uses: actions/upload-artifact@v4
      with:
        name: aquapark-app
        path: AquaparkApp/bin/Release/net8.0-windows/win-x64/publish/
```

### **Создать файл `.github/workflows/design-check.yml`:**
```yaml
name: 🎨 Design Check

on:
  push:
    branches: [ design/* ]
  pull_request:
    branches: [ main ]

jobs:
  design-check:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Check CSS files
      run: |
        echo "🔍 Checking CSS files for macOS Sequoia compliance..."
        if grep -r "backdrop-filter: blur(10px)" src/components/; then
          echo "❌ Found old blur values, should be 20px"
          exit 1
        fi
        if grep -r "border-radius: 15px" src/components/; then
          echo "❌ Found old border radius, should be 16px"
          exit 1
        fi
        echo "✅ CSS files are compliant with macOS Sequoia design"
    
    - name: Check color scheme
      run: |
        echo "🎨 Checking color scheme..."
        if grep -r "#007AFF" src/components/; then
          echo "✅ Found correct macOS Sequoia blue color"
        else
          echo "❌ Missing macOS Sequoia blue color"
          exit 1
        fi
```

---

## 👥 Шаг 4: Настройка прав доступа

### **Создать команды на GitHub:**

1. **Перейти в Settings → Manage access**
2. **Создать команды:**
   - **`design-team`** - отдел дизайна
   - **`database-team`** - отдел БД
   - **`developers`** - разработчики
   - **`admins`** - администраторы

### **Настроить права:**
- **`design-team`**: Write (могут создавать PR, редактировать файлы дизайна)
- **`database-team`**: Write (могут редактировать файлы БД)
- **`developers`**: Write (могут редактировать код)
- **`admins`**: Admin (могут управлять репозиторием)

---

## 📚 Шаг 5: Документация для команд

### **Создать файл `CONTRIBUTING.md`:**
```markdown
# 🤝 Руководство по участию в проекте

## 🌊 Аквапарк Водный мир - Система управления

### 👥 Команды

#### 🎨 **Отдел дизайна**
- **Ответственность:** CSS, HTML, дизайн, иконки
- **Ветки:** `design/*`
- **Файлы:** `src/components/*.css`, `src/components/*.html`

#### 🗄️ **Отдел БД**
- **Ответственность:** База данных, SQL, модели
- **Ветки:** `database/*`
- **Файлы:** `Database/*`, `AquaparkApp.Models/*`, `AquaparkApp.DAL/*`

#### 💻 **Разработчики**
- **Ответственность:** C# код, функции, исправления
- **Ветки:** `feature/*`, `bugfix/*`
- **Файлы:** `AquaparkApp/*`, `AquaparkApp.BLL/*`

### 🔄 Процесс работы

1. **Создать ветку** для своей задачи
2. **Внести изменения**
3. **Создать Pull Request**
4. **Дождаться ревью**
5. **Слить в main**

### 📋 Правила

- ✅ Всегда создавайте ветки от `main`
- ✅ Называйте ветки понятно: `design/macos-theme`, `database/optimization`
- ✅ Делайте коммиты с понятными сообщениями
- ✅ Создавайте Pull Request для слияния
- ✅ Тестируйте изменения перед отправкой

### 🚫 Запрещено

- ❌ Прямые коммиты в `main`
- ❌ Удаление чужих веток
- ❌ Изменение файлов других отделов без согласования
```

### **Создать файл `TEAM_GUIDES/`:**
```
TEAM_GUIDES/
├── DESIGN_TEAM.md          # Руководство для отдела дизайна
├── DATABASE_TEAM.md        # Руководство для отдела БД
├── DEVELOPERS.md           # Руководство для разработчиков
└── WORKFLOW.md             # Общий рабочий процесс
```

---

## 🎯 Шаг 6: Создание файлов для команд

### **Создать `.gitignore` для .NET:**
```gitignore
# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio
.vs/
*.user
*.userosscache
*.sln.docstates

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# NuGet
*.nupkg
*.snupkg
.nuget/
packages/

# Node.js
node_modules/
npm-debug.log*
yarn-debug.log*
yarn-error.log*

# Environment variables
.env
.env.local
.env.development.local
.env.test.local
.env.production.local

# IDE
.vscode/
.idea/
*.swp
*.swo

# OS
.DS_Store
Thumbs.db

# Temporary files
*.tmp
*.temp
*.log
```

### **Создать `README.md` для репозитория:**
```markdown
# 🌊 Аквапарк Водный мир - Система управления

[![Build Status](https://github.com/ваш-username/aquapark-management-system/workflows/Build/badge.svg)](https://github.com/ваш-username/aquapark-management-system/actions)
[![Design Check](https://github.com/ваш-username/aquapark-management-system/workflows/Design%20Check/badge.svg)](https://github.com/ваш-username/aquapark-management-system/actions)

Современное приложение для управления аквапарком с красивым интерфейсом в стиле macOS Sequoia.

## ✨ Особенности

- 🎨 **Красивый дизайн** в стиле macOS Sequoia
- 🖥️ **Windows Forms** приложение
- 🌐 **Веб-компоненты** (HTML/CSS/JS)
- 🗄️ **PostgreSQL** база данных
- 👥 **Система пользователей**
- 🎫 **Бронирование билетов**
- 📊 **Отчеты и аналитика**

## 🚀 Быстрый старт

### Требования
- .NET 8.0 SDK
- PostgreSQL 12+
- Visual Studio 2022

### Установка
```bash
git clone https://github.com/ваш-username/aquapark-management-system.git
cd aquapark-management-system
dotnet restore
dotnet build
```

### Запуск
```bash
dotnet run --project AquaparkApp
```

## 👥 Команды

- 🎨 **Отдел дизайна** - CSS, HTML, дизайн
- 🗄️ **Отдел БД** - База данных, SQL
- 💻 **Разработчики** - C# код, функции

## 📚 Документация

- [Руководство по участию](CONTRIBUTING.md)
- [Отдел дизайна](TEAM_GUIDES/DESIGN_TEAM.md)
- [Отдел БД](TEAM_GUIDES/DATABASE_TEAM.md)
- [Разработчики](TEAM_GUIDES/DEVELOPERS.md)

## 🛡️ Решение проблем с антивирусами

См. [ANTIVIRUS_SOLUTION.md](ANTIVIRUS_SOLUTION.md)

## 📞 Поддержка

- Email: support@aquapark.com
- Telegram: @aquapark_support

## 📄 Лицензия

MIT License - см. [LICENSE](LICENSE)
```

---

## 🔧 Шаг 7: Команды для настройки

### **Выполнить все команды:**
```bash
# 1. Клонировать репозиторий
git clone https://github.com/ваш-username/aquapark-management-system.git
cd aquapark-management-system

# 2. Создать ветки
git checkout -b design/macos-sequoia-theme
git push origin design/macos-sequoia-theme

git checkout -b database/optimization
git push origin database/optimization

git checkout -b feature/new-features
git push origin feature/new-features

git checkout main

# 3. Добавить все файлы
git add .

# 4. Сделать коммит
git commit -m "🎉 Initial setup: GitHub repository with team structure

- ✅ Created branch structure for teams
- ✅ Added GitHub Actions for CI/CD
- ✅ Created documentation for teams
- ✅ Set up .gitignore for .NET
- ✅ Added team guides and workflows"

# 5. Отправить на GitHub
git push origin main
```

---

## 🎯 Результат

После выполнения всех шагов у вас будет:

1. ✅ **GitHub репозиторий** с правильной структурой
2. ✅ **Ветки для каждого отдела** (дизайн, БД, разработка)
3. ✅ **GitHub Actions** для автоматической сборки
4. ✅ **Документация** для команд
5. ✅ **Права доступа** для разных ролей
6. ✅ **Автоматические проверки** дизайна и кода

### **Команды смогут:**
- 🎨 **Отдел дизайна** - редактировать CSS/HTML файлы
- 🗄️ **Отдел БД** - работать с базой данных
- 💻 **Разработчики** - разрабатывать функции
- 👑 **Администраторы** - управлять репозиторием

---

## 📞 Поддержка

При возникновении проблем:
- **GitHub Issues** - для багов и предложений
- **Discussions** - для обсуждений
- **Email** - support@aquapark.com

**Удачи в командной работе! 🚀✨**

