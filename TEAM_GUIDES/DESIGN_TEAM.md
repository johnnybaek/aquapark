# 🎨 Руководство для отдела дизайна

## 🌊 Аквапарк Водный мир - Система управления

### 👥 Ваша команда: Отдел дизайна
**Ответственность:** CSS, HTML, дизайн, иконки, стилизация

---

## 📁 Файлы, которые вы редактируете

### **🌐 Веб-компоненты (HTML/CSS/JS):**
```
src/components/
├── Header.css          # Заголовок приложения
├── Header.js           # Логика заголовка
├── Sidebar.css         # Боковая панель
├── Sidebar.js          # Логика боковой панели
├── Dashboard.css       # Главная панель
├── Dashboard.js        # Логика дашборда
├── Attractions.css     # Карточки аттракционов
├── Attractions.js      # Логика аттракционов
├── Tickets.css         # Карточки билетов
└── Tickets.js          # Логика билетов
```

### **🖥️ Windows Forms (C#):**
```
AquaparkApp/Controls/
└── CustomControls.cs   # Кастомные контролы

AquaparkApp/Forms/
├── MainForm.cs         # Главная форма
├── LoginForm.cs        # Форма входа
├── RegisterForm.cs     # Форма регистрации
├── ProfileForm.cs      # Форма профиля
├── BookingForm.cs      # Форма бронирования
├── AdminPanelForm.cs   # Админ-панель
└── ReportViewerForm.cs # Просмотр отчетов
```

### **🎨 Ресурсы:**
```
public/icons/           # Иконки (создать папку)
├── attractions/        # Иконки аттракционов
├── tickets/           # Иконки билетов
├── users/             # Иконки пользователей
└── system/            # Системные иконки

AquaparkApp/Resources/
└── icon.ico           # Иконка приложения
```

---

## 🎯 Ваши задачи

### **1. Стилизация в стиле macOS Sequoia**
- ✅ Обновить цветовую схему
- ✅ Увеличить скругления (12-16px)
- ✅ Улучшить эффекты размытия (20px)
- ✅ Добавить тени и анимации
- ✅ Обновить шрифты на SF Pro

### **2. Создание иконок**
- ✅ Стиль SF Symbols
- ✅ Размеры: 16px, 20px, 24px, 28px, 32px
- ✅ Формат: SVG для веб, PNG для Windows Forms
- ✅ Цвета: монохромные или акцентные

### **3. Адаптивность**
- ✅ Desktop (1920x1080+)
- ✅ Laptop (1366x768 - 1920x1080)
- ✅ Tablet (768x1024)
- ✅ Mobile (375x667 - 414x896)

---

## 🔄 Рабочий процесс

### **1. Создание ветки:**
```bash
git checkout main
git pull origin main
git checkout -b design/macos-sequoia-theme
```

### **2. Внесение изменений:**
- Редактируйте CSS/HTML файлы
- Тестируйте в браузере
- Проверяйте на разных разрешениях

### **3. Коммит:**
```bash
git add .
git commit -m "🎨 Update design: macOS Sequoia theme

- Updated color scheme to #007AFF
- Increased border radius to 16px
- Enhanced blur effects to 20px
- Added smooth animations
- Updated fonts to SF Pro"
```

### **4. Push и Pull Request:**
```bash
git push origin design/macos-sequoia-theme
# Создать Pull Request на GitHub
```

---

## 🎨 Стандарты дизайна

### **Цветовая палитра macOS Sequoia:**
```css
:root {
  --system-blue: #007AFF;           /* Основной синий */
  --system-green: #34C759;          /* Успех */
  --system-orange: #FF9500;         /* Предупреждение */
  --system-red: #FF3B30;            /* Ошибка */
  --system-purple: #AF52DE;         /* Акцент */
  --system-pink: #FF2D92;           /* Акцент */
  --system-yellow: #FFCC00;         /* Акцент */
  --system-gray: #8E8E93;           /* Вторичный текст */
  --system-gray2: #AEAEB2;          /* Третичный текст */
  --system-gray3: #C7C7CC;          /* Разделители */
  --system-gray4: #D1D1D6;          /* Фон разделителей */
  --system-gray5: #E5E5EA;          /* Фон */
  --system-gray6: #F2F2F7;          /* Основной фон */
  --system-dark: #1C1C1E;           /* Темный фон */
  --system-dark-secondary: #2C2C2E; /* Вторичный темный */
  --system-dark-tertiary: #3A3A3C;  /* Третичный темный */
}
```

### **Типографика:**
```css
/* Шрифты */
font-family: 'SF Pro Display', -apple-system, BlinkMacSystemFont, sans-serif;
font-family: 'SF Pro Text', -apple-system, BlinkMacSystemFont, sans-serif;

/* Размеры */
--font-size-large-title: 34px;      /* Заголовки */
--font-size-title1: 28px;           /* Заголовки */
--font-size-title2: 22px;           /* Подзаголовки */
--font-size-title3: 20px;           /* Подзаголовки */
--font-size-headline: 17px;         /* Заголовки секций */
--font-size-body: 17px;             /* Основной текст */
--font-size-callout: 16px;          /* Выделенный текст */
--font-size-subhead: 15px;          /* Подзаголовки */
--font-size-footnote: 13px;         /* Сноски */
--font-size-caption1: 12px;         /* Подписи */
--font-size-caption2: 11px;         /* Мелкие подписи */
```

### **Скругления и отступы:**
```css
/* Радиусы скругления */
--radius-small: 6px;                /* Мелкие элементы */
--radius-medium: 8px;               /* Кнопки, поля ввода */
--radius-large: 12px;               /* Карточки */
--radius-extra-large: 16px;         /* Панели */
--radius-max: 20px;                 /* Модальные окна */

/* Отступы */
--spacing-xs: 4px;
--spacing-sm: 8px;
--spacing-md: 16px;
--spacing-lg: 24px;
--spacing-xl: 32px;
--spacing-xxl: 48px;
```

---

## 🛠️ Инструменты

### **Для CSS/HTML:**
- **Visual Studio Code** (рекомендуется)
- **Sublime Text**
- **Atom**
- **WebStorm**

### **Для создания иконок:**
- **Figma** (рекомендуется)
- **Sketch** (для Mac)
- **Adobe Illustrator**
- **Inkscape** (бесплатный)

### **Для тестирования:**
- **Браузер** (Chrome, Firefox, Safari, Edge)
- **DevTools** для отладки
- **Responsive Design Mode**

---

## 📋 Чек-лист для коммитов

### **Перед коммитом проверить:**
- [ ] Цвета соответствуют macOS Sequoia
- [ ] Скругления увеличены (12-16px)
- [ ] Эффекты размытия усилены (20px)
- [ ] Шрифты обновлены на SF Pro
- [ ] Анимации плавные
- [ ] Адаптивность работает
- [ ] Иконки консистентны
- [ ] Нет ошибок в консоли

### **Тестирование:**
- [ ] Desktop (1920x1080)
- [ ] Laptop (1366x768)
- [ ] Tablet (768x1024)
- [ ] Mobile (375x667)
- [ ] Темная тема (если есть)

---

## 🚨 Важные правила

### **✅ Можно делать:**
- Редактировать CSS/HTML файлы
- Создавать иконки
- Обновлять стили
- Улучшать анимации
- Работать с адаптивностью

### **❌ Нельзя делать:**
- Изменять C# код без согласования
- Редактировать файлы БД
- Удалять чужие ветки
- Делать коммиты в main
- Изменять логику приложения

---

## 📞 Поддержка

### **При возникновении вопросов:**
- **Дизайн-лидер** - вопросы по стилю
- **Frontend разработчик** - технические вопросы
- **GitHub Issues** - баги и предложения
- **Discussions** - обсуждения

### **Контакты:**
- **Email:** design@aquapark.com
- **Telegram:** @aquapark_design
- **Slack:** #design-team

---

## 🎯 Примеры задач

### **Задача 1: Обновить цветовую схему**
```bash
git checkout -b design/update-colors
# Редактировать CSS файлы
git add .
git commit -m "🎨 Update colors to macOS Sequoia palette"
git push origin design/update-colors
# Создать Pull Request
```

### **Задача 2: Создать иконки**
```bash
git checkout -b design/create-icons
# Создать SVG иконки в public/icons/
git add .
git commit -m "🎨 Add SF Symbols style icons"
git push origin design/create-icons
# Создать Pull Request
```

### **Задача 3: Улучшить анимации**
```bash
git checkout -b design/enhance-animations
# Обновить CSS анимации
git add .
git commit -m "🎨 Enhance animations with cubic-bezier"
git push origin design/enhance-animations
# Создать Pull Request
```

---

## 🎉 Удачи в работе!

Помните: ваша работа делает приложение красивым и современным! 

**Создавайте потрясающий дизайн в стиле macOS Sequoia! 🎨✨**
