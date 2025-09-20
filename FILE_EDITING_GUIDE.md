# 📁 Руководство по редактированию файлов
## Конкретные инструкции для отдела дизайна

---

## 🎯 Приоритетные файлы для редактирования

### **1. ВЫСОКИЙ ПРИОРИТЕТ - Основные стили**

#### **📄 `src/components/Header.css`**
**Что делать:** Обновить заголовок приложения
**Время:** 30 минут

**Конкретные изменения:**
```css
/* Строка 3 - изменить фон */
background: rgba(0, 122, 255, 0.1);  /* Вместо rgba(255, 255, 255, 0.15) */

/* Строка 4 - увеличить размытие */
backdrop-filter: blur(20px);  /* Вместо blur(10px) */

/* Строка 10 - увеличить скругление */
border-radius: 16px;  /* Вместо 15px */

/* Строка 24 - увеличить скругление кнопок */
border-radius: 12px;  /* Вместо 8px */
```

#### **📄 `src/components/Dashboard.css`**
**Что делать:** Обновить главную панель
**Время:** 45 минут

**Конкретные изменения:**
```css
/* Строка 8 - сделать фон более непрозрачным */
background: rgba(255, 255, 255, 0.8);  /* Вместо rgba(255, 255, 255, 0.1) */

/* Строка 9 - увеличить размытие */
backdrop-filter: blur(20px);  /* Вместо blur(10px) */

/* Строка 10 - увеличить скругление */
border-radius: 16px;  /* Вместо 15px */

/* Строка 38 - увеличить скругление карточек */
border-radius: 16px;  /* Вместо 15px */

/* Строка 41 - улучшить анимацию */
transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);  /* Вместо 0.2s */
```

#### **📄 `src/components/Attractions.css`**
**Что делать:** Обновить карточки аттракционов
**Время:** 45 минут

**Конкретные изменения:**
```css
/* Строка 8 - сделать фон более непрозрачным */
background: rgba(255, 255, 255, 0.8);  /* Вместо rgba(255, 255, 255, 0.1) */

/* Строка 9 - увеличить размытие */
backdrop-filter: blur(20px);  /* Вместо blur(10px) */

/* Строка 10 - увеличить скругление */
border-radius: 16px;  /* Вместо 15px */

/* Строка 37 - увеличить скругление карточек */
border-radius: 16px;  /* Вместо 15px */

/* Строка 40 - улучшить анимацию */
transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);  /* Вместо 0.2s */
```

#### **📄 `src/components/Tickets.css`**
**Что делать:** Обновить карточки билетов
**Время:** 45 минут

**Конкретные изменения:**
```css
/* Строка 8 - сделать фон более непрозрачным */
background: rgba(255, 255, 255, 0.8);  /* Вместо rgba(255, 255, 255, 0.1) */

/* Строка 9 - увеличить размытие */
backdrop-filter: blur(20px);  /* Вместо blur(10px) */

/* Строка 10 - увеличить скругление */
border-radius: 16px;  /* Вместо 15px */

/* Строка 132 - увеличить скругление карточек */
border-radius: 16px;  /* Вместо 15px */

/* Строка 137 - улучшить анимацию */
transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);  /* Вместо 0.2s */
```

#### **📄 `src/components/Sidebar.css`**
**Что делать:** Обновить боковую панель
**Время:** 30 минут

**Конкретные изменения:**
```css
/* Строка 7 - изменить фон на темный */
background: rgba(28, 28, 30, 0.8);  /* Вместо rgba(0, 0, 0, 0.2) */

/* Строка 8 - увеличить размытие */
backdrop-filter: blur(20px);  /* Вместо blur(10px) */

/* Строка 9 - добавить скругление */
border-radius: 0 16px 16px 0;  /* Добавить эту строку */

/* Строка 25 - увеличить скругление логотипа */
border-radius: 16px;  /* Добавить эту строку */
```

---

### **2. СРЕДНИЙ ПРИОРИТЕТ - Windows Forms**

#### **📄 `AquaparkApp/Controls/CustomControls.cs`**
**Что делать:** Обновить кастомные контролы
**Время:** 1 час

**Конкретные изменения:**
```csharp
/* Строка 16 - обновить основной цвет */
private Color _gradientStart = Color.FromArgb(0, 122, 255);  /* #007AFF */

/* Строка 17 - обновить вторичный цвет */
private Color _gradientEnd = Color.FromArgb(0, 86, 204);  /* Темнее синий */

/* Строка 20 - увеличить радиус скругления */
private int _cornerRadius = 16;  /* Вместо 12 */

/* Строка 29 - обновить шрифт */
Font = new Font("SF Pro Display", 17F, FontStyle.Regular);  /* Вместо 14F */

/* Строка 31 - увеличить размер кнопки */
Size = new Size(140, 44);  /* Вместо 120, 40 */
```

#### **📄 `AquaparkApp/Forms/MainForm.cs`**
**Что делать:** Обновить главную форму
**Время:** 30 минут

**Конкретные изменения:**
```csharp
/* Строка 55 - обновить фон формы */
this.BackColor = Color.FromArgb(242, 242, 247);  /* #F2F2F7 */

/* Строка 56 - обновить шрифт */
this.Font = new Font("SF Pro Display", 17F, FontStyle.Regular);  /* Вместо 12F */

/* Строка 88 - обновить цвет заголовка */
BackColor = Color.FromArgb(0, 122, 255);  /* #007AFF */

/* Строка 95 - обновить шрифт логотипа */
Font = new Font("SF Pro Display", 24F, FontStyle.Bold);  /* Вместо 20F */
```

#### **📄 `AquaparkApp/Forms/LoginForm.cs`**
**Что делать:** Обновить форму входа
**Время:** 30 минут

**Конкретные изменения:**
```csharp
/* Найти строку с BackColor формы и заменить на: */
this.BackColor = Color.FromArgb(242, 242, 247);  /* #F2F2F7 */

/* Найти строки с Font и заменить на: */
Font = new Font("SF Pro Display", 17F, FontStyle.Regular);

/* Найти строки с цветами кнопок и заменить на: */
BackColor = Color.FromArgb(0, 122, 255);  /* #007AFF */
```

#### **📄 `AquaparkApp/Forms/RegisterForm.cs`**
**Что делать:** Обновить форму регистрации
**Время:** 30 минут

**Конкретные изменения:** (аналогично LoginForm.cs)

#### **📄 `AquaparkApp/Forms/ProfileForm.cs`**
**Что делать:** Обновить форму профиля
**Время:** 30 минут

**Конкретные изменения:** (аналогично LoginForm.cs)

---

### **3. НИЗКИЙ ПРИОРИТЕТ - Дополнительные файлы**

#### **📄 `AquaparkApp/Forms/BookingForm.cs`**
**Что делать:** Обновить форму бронирования
**Время:** 30 минут

#### **📄 `AquaparkApp/Forms/AdminPanelForm.cs`**
**Что делать:** Обновить админ-панель
**Время:** 45 минут

#### **📄 `AquaparkApp/Forms/ReportViewerForm.cs`**
**Что делать:** Обновить просмотр отчетов
**Время:** 30 минут

---

## 🎨 Создание новых файлов

### **📄 `public/icons/` (создать папку)**
**Что делать:** Создать папку для иконок
**Время:** 5 минут

**Структура папки:**
```
public/icons/
├── attractions/
│   ├── water-slide.svg
│   ├── pool.svg
│   └── lazy-river.svg
├── tickets/
│   ├── adult.svg
│   ├── child.svg
│   └── family.svg
├── users/
│   ├── profile.svg
│   ├── settings.svg
│   └── logout.svg
└── system/
    ├── search.svg
    ├── add.svg
    └── edit.svg
```

### **📄 `src/styles/macos-sequoia.css` (создать файл)**
**Что делать:** Создать общий файл стилей
**Время:** 30 минут

**Содержимое файла:**
```css
/* macOS Sequoia Design System */
:root {
  /* Цвета */
  --system-blue: #007AFF;
  --system-green: #34C759;
  --system-orange: #FF9500;
  --system-red: #FF3B30;
  --system-purple: #AF52DE;
  --system-pink: #FF2D92;
  --system-yellow: #FFCC00;
  --system-gray: #8E8E93;
  --system-gray2: #AEAEB2;
  --system-gray3: #C7C7CC;
  --system-gray4: #D1D1D6;
  --system-gray5: #E5E5EA;
  --system-gray6: #F2F2F7;
  --system-dark: #1C1C1E;
  --system-dark-secondary: #2C2C2E;
  --system-dark-tertiary: #3A3A3C;
  
  /* Отступы */
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;
  --spacing-xxl: 48px;
  
  /* Скругления */
  --radius-small: 6px;
  --radius-medium: 8px;
  --radius-large: 12px;
  --radius-extra-large: 16px;
  --radius-max: 20px;
}

/* Глобальные стили */
* {
  box-sizing: border-box;
}

body {
  font-family: 'SF Pro Display', -apple-system, BlinkMacSystemFont, sans-serif;
  background: var(--system-gray6);
  color: var(--system-dark);
  margin: 0;
  padding: 0;
}

/* Утилитарные классы */
.macos-card {
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(20px);
  border-radius: var(--radius-extra-large);
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.macos-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
}

.macos-button-primary {
  background: linear-gradient(135deg, var(--system-blue) 0%, #0056CC 100%);
  border: none;
  border-radius: var(--radius-large);
  color: white;
  padding: var(--spacing-sm) var(--spacing-lg);
  font-family: 'SF Pro Display', -apple-system, BlinkMacSystemFont, sans-serif;
  font-weight: 600;
  font-size: 17px;
  cursor: pointer;
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 2px 10px rgba(0, 122, 255, 0.3);
}

.macos-button-primary:hover {
  transform: translateY(-1px);
  box-shadow: 0 4px 20px rgba(0, 122, 255, 0.4);
}

.macos-input {
  width: 100%;
  padding: var(--spacing-md);
  border: 1px solid var(--system-gray4);
  border-radius: var(--radius-large);
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(10px);
  color: var(--system-dark);
  font-family: 'SF Pro Text', -apple-system, BlinkMacSystemFont, sans-serif;
  font-size: 17px;
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
}

.macos-input:focus {
  outline: none;
  border-color: var(--system-blue);
  box-shadow: 0 0 0 3px rgba(0, 122, 255, 0.1);
  background: rgba(255, 255, 255, 0.95);
}
```

---

## 📋 Пошаговый план действий

### **День 1: Основные стили (4 часа)**
1. **09:00-09:30** - Обновить `Header.css`
2. **09:30-10:15** - Обновить `Dashboard.css`
3. **10:15-11:00** - Обновить `Attractions.css`
4. **11:00-11:45** - Обновить `Tickets.css`
5. **11:45-12:00** - Обновить `Sidebar.css`

### **День 2: Windows Forms (3 часа)**
1. **09:00-10:00** - Обновить `CustomControls.cs`
2. **10:00-10:30** - Обновить `MainForm.cs`
3. **10:30-11:00** - Обновить `LoginForm.cs`
4. **11:00-11:30** - Обновить `RegisterForm.cs`
5. **11:30-12:00** - Обновить `ProfileForm.cs`

### **День 3: Дополнительные файлы (2 часа)**
1. **09:00-09:30** - Обновить `BookingForm.cs`
2. **09:30-10:15** - Обновить `AdminPanelForm.cs`
3. **10:15-10:45** - Обновить `ReportViewerForm.cs`
4. **10:45-11:00** - Создать папку `public/icons/`

### **День 4: Создание ресурсов (4 часа)**
1. **09:00-10:00** - Создать `macos-sequoia.css`
2. **10:00-12:00** - Создать SVG иконки
3. **12:00-13:00** - Тестирование и исправления

---

## 🔧 Инструменты для работы

### **Для CSS файлов:**
- **Visual Studio Code** (рекомендуется)
- **Sublime Text**
- **Atom**
- **WebStorm**

### **Для C# файлов:**
- **Visual Studio 2022** (обязательно)
- **Visual Studio Code** с C# расширением
- **JetBrains Rider**

### **Для создания иконок:**
- **Figma** (рекомендуется)
- **Sketch** (для Mac)
- **Adobe Illustrator**
- **Inkscape** (бесплатный)

---

## ✅ Чек-лист готовности

### **После каждого файла проверить:**
- [ ] Цвета изменились на синие (#007AFF)
- [ ] Скругления увеличились (12-16px)
- [ ] Шрифты стали SF Pro
- [ ] Эффекты размытия усилились (20px)
- [ ] Тени стали мягче
- [ ] Анимации плавнее

### **После всех изменений:**
- [ ] Запустить Windows Forms приложение
- [ ] Открыть веб-компоненты в браузере
- [ ] Протестировать на разных разрешениях
- [ ] Проверить все формы и контролы
- [ ] Убедиться в консистентности дизайна

---

## 🚨 Важные замечания

### **Перед началом работы:**
1. **Сделать резервную копию** всех файлов
2. **Установить шрифты SF Pro** в систему
3. **Проверить версию .NET** (должна быть 8.0)
4. **Убедиться в наличии Visual Studio 2022**

### **Во время работы:**
1. **Сохранять файлы** после каждого изменения
2. **Тестировать изменения** в браузере/приложении
3. **Следовать принципам** macOS Sequoia
4. **Поддерживать консистентность** дизайна

### **После завершения:**
1. **Провести полное тестирование**
2. **Проверить на разных устройствах**
3. **Убедиться в отсутствии ошибок**
4. **Подготовить отчет** о проделанной работе

---

## 📞 Поддержка

**При возникновении вопросов:**
- **Технический лидер:** вопросы по архитектуре
- **Frontend разработчик:** CSS/JS вопросы
- **Backend разработчик:** C# вопросы
- **Дизайн-лидер:** вопросы по стилю

**Удачи в создании красивого дизайна! 🎨✨**
