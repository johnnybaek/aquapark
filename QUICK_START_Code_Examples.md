# 🚀 Быстрый старт - Примеры кода для macOS Sequoia

## 📋 Готовые CSS классы для копирования

### **1. Основные цвета (добавить в начало каждого CSS файла)**
```css
:root {
  /* macOS Sequoia Colors */
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
  
  /* Spacing */
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;
  --spacing-xxl: 48px;
  
  /* Border Radius */
  --radius-small: 6px;
  --radius-medium: 8px;
  --radius-large: 12px;
  --radius-extra-large: 16px;
  --radius-max: 20px;
}
```

### **2. Готовые классы для карточек**
```css
/* Карточка в стиле macOS Sequoia */
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

/* Темная карточка */
.macos-card-dark {
  background: rgba(28, 28, 30, 0.8);
  backdrop-filter: blur(20px);
  border-radius: var(--radius-extra-large);
  border: 1px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
}
```

### **3. Готовые классы для кнопок**
```css
/* Основная кнопка */
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

.macos-button-primary:active {
  transform: translateY(0);
  box-shadow: 0 2px 10px rgba(0, 122, 255, 0.3);
}

/* Вторичная кнопка */
.macos-button-secondary {
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: var(--radius-large);
  color: white;
  padding: var(--spacing-sm) var(--spacing-lg);
  font-family: 'SF Pro Display', -apple-system, BlinkMacSystemFont, sans-serif;
  font-weight: 600;
  font-size: 17px;
  cursor: pointer;
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
}

.macos-button-secondary:hover {
  background: rgba(255, 255, 255, 0.2);
  transform: translateY(-1px);
}
```

### **4. Готовые классы для полей ввода**
```css
/* Поле ввода в стиле macOS */
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

.macos-input::placeholder {
  color: var(--system-gray2);
}
```

### **5. Готовые классы для иконок**
```css
/* Иконка с градиентом */
.macos-icon-gradient {
  width: 50px;
  height: 50px;
  border-radius: var(--radius-large);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 24px;
  font-weight: 600;
}

.macos-icon-blue {
  background: linear-gradient(135deg, var(--system-blue) 0%, #0056CC 100%);
}

.macos-icon-green {
  background: linear-gradient(135deg, var(--system-green) 0%, #2E7D32 100%);
}

.macos-icon-orange {
  background: linear-gradient(135deg, var(--system-orange) 0%, #F57C00 100%);
}

.macos-icon-red {
  background: linear-gradient(135deg, var(--system-red) 0%, #D32F2F 100%);
}

.macos-icon-purple {
  background: linear-gradient(135deg, var(--system-purple) 0%, #7B1FA2 100%);
}
```

---

## 🔧 Готовые C# классы для Windows Forms

### **1. Обновленный MacOSButton**
```csharp
public class MacOSButton : Button
{
    private bool _isHovered = false;
    private bool _isPressed = false;
    private Color _gradientStart = Color.FromArgb(0, 122, 255);    // #007AFF
    private Color _gradientEnd = Color.FromArgb(0, 86, 204);      // Темнее синий
    private Color _hoverStart = Color.FromArgb(0, 140, 255);      // Светлее при hover
    private Color _hoverEnd = Color.FromArgb(0, 120, 220);        // Светлее при hover
    private int _cornerRadius = 16;  // Увеличено с 12 до 16

    public MacOSButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = Color.White;
        ForeColor = Color.White;
        Font = new Font("SF Pro Display", 17F, FontStyle.Regular);  // Увеличено с 14 до 17
        Cursor = Cursors.Hand;
        Size = new Size(140, 44);  // Увеличено для лучших пропорций
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
        GraphicsPath path = GetRoundedRectangle(rect, _cornerRadius);

        // Определяем цвета в зависимости от состояния
        Color startColor = _isPressed ? Color.FromArgb(0, 80, 180) : 
                          _isHovered ? _hoverStart : _gradientStart;
        Color endColor = _isPressed ? Color.FromArgb(0, 60, 160) : 
                        _isHovered ? _hoverEnd : _gradientEnd;

        // Рисуем градиентный фон
        using (LinearGradientBrush brush = new LinearGradientBrush(
            rect, startColor, endColor, LinearGradientMode.Vertical))
        {
            g.FillPath(brush, path);
        }

        // Рисуем тень (только если не нажата)
        if (!_isPressed)
        {
            Rectangle shadowRect = new Rectangle(0, 3, Width - 1, Height - 1);
            GraphicsPath shadowPath = GetRoundedRectangle(shadowRect, _cornerRadius);
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
            {
                g.FillPath(shadowBrush, shadowPath);
            }
        }

        // Рисуем текст
        StringFormat sf = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        g.DrawString(Text, Font, new SolidBrush(ForeColor), rect, sf);
    }

    // ... остальные методы остаются без изменений
}
```

### **2. Обновленный MacOSTextBox**
```csharp
public class MacOSTextBox : TextBox
{
    private Color _borderColor = Color.FromArgb(199, 199, 204);    // #C7C7CC
    private Color _focusColor = Color.FromArgb(0, 122, 255);       // #007AFF
    private int _borderRadius = 16;  // Увеличено с 8 до 16
    private bool _isFocused = false;

    public MacOSTextBox()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        BorderStyle = BorderStyle.None;
        BackColor = Color.FromArgb(242, 242, 247);  // #F2F2F7
        ForeColor = Color.FromArgb(28, 28, 30);     // #1C1C1E
        Font = new Font("SF Pro Text", 17F, FontStyle.Regular);  // Увеличено с 12 до 17
        Padding = new Padding(16, 12, 16, 12);  // Увеличено для лучших пропорций
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Рисуем фон
        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
        GraphicsPath path = GetRoundedRectangle(rect, _borderRadius);
        g.FillPath(new SolidBrush(BackColor), path);

        // Рисуем границу
        Color borderColor = _isFocused ? _focusColor : _borderColor;
        int borderWidth = _isFocused ? 2 : 1;
        using (Pen pen = new Pen(borderColor, borderWidth))
        {
            g.DrawPath(pen, path);
        }

        // Рисуем текст
        Rectangle textRect = new Rectangle(Padding.Left, Padding.Top, 
            Width - Padding.Left - Padding.Right, Height - Padding.Top - Padding.Bottom);
        
        TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, 
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
    }

    // ... остальные методы остаются без изменений
}
```

### **3. Обновленный GlassPanel**
```csharp
public class GlassPanel : Panel
{
    private int _blurRadius = 30;  // Увеличено с 20 до 30
    private Color _glassColor = Color.FromArgb(120, 255, 255, 255);  // Более непрозрачный

    public GlassPanel()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Рисуем эффект стекла
        Rectangle rect = new Rectangle(0, 0, Width, Height);
        GraphicsPath path = GetRoundedRectangle(rect, 20);  // Увеличено с 15 до 20

        // Градиент для эффекта стекла
        using (LinearGradientBrush brush = new LinearGradientBrush(
            rect, 
            Color.FromArgb(180, 255, 255, 255),  // Более непрозрачный
            Color.FromArgb(80, 255, 255, 255),   // Более непрозрачный
            LinearGradientMode.Vertical))
        {
            g.FillPath(brush, path);
        }

        // Рисуем границу
        using (Pen pen = new Pen(Color.FromArgb(120, 255, 255, 255), 1))
        {
            g.DrawPath(pen, path);
        }

        // Добавляем тень
        Rectangle shadowRect = new Rectangle(0, 4, Width, Height);
        GraphicsPath shadowPath = GetRoundedRectangle(shadowRect, 20);
        using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
        {
            g.FillPath(shadowBrush, shadowPath);
        }
    }

    // ... остальные методы остаются без изменений
}
```

---

## 📝 Быстрые замены в существующих файлах

### **1. В Header.css (строка 3):**
```css
/* Было: */
background: rgba(255, 255, 255, 0.15);

/* Заменить на: */
background: rgba(0, 122, 255, 0.1);
```

### **2. В Header.css (строка 4):**
```css
/* Было: */
backdrop-filter: blur(10px);

/* Заменить на: */
backdrop-filter: blur(20px);
```

### **3. В Dashboard.css (строка 8):**
```css
/* Было: */
background: rgba(255, 255, 255, 0.1);

/* Заменить на: */
background: rgba(255, 255, 255, 0.8);
```

### **4. В Dashboard.css (строка 9):**
```css
/* Было: */
backdrop-filter: blur(10px);

/* Заменить на: */
backdrop-filter: blur(20px);
```

### **5. В MainForm.cs (строка 55):**
```csharp
/* Было: */
this.BackColor = Color.FromArgb(248, 248, 248);

/* Заменить на: */
this.BackColor = Color.FromArgb(242, 242, 247);
```

### **6. В MainForm.cs (строка 88):**
```csharp
/* Было: */
BackColor = Color.FromArgb(0, 122, 255)

/* Заменить на: */
BackColor = Color.FromArgb(0, 122, 255)  // Уже правильно!
```

---

## 🎯 Шаблоны для быстрого копирования

### **1. HTML структура для карточки:**
```html
<div class="macos-card">
  <div class="macos-icon-gradient macos-icon-blue">
    🎢
  </div>
  <h3>Название аттракциона</h3>
  <p>Описание аттракциона</p>
  <button class="macos-button-primary">Забронировать</button>
</div>
```

### **2. HTML структура для формы:**
```html
<form class="macos-form">
  <input type="text" class="macos-input" placeholder="Введите название">
  <input type="email" class="macos-input" placeholder="Введите email">
  <button type="submit" class="macos-button-primary">Отправить</button>
</form>
```

### **3. C# код для создания кнопки:**
```csharp
var button = new MacOSButton
{
    Text = "Забронировать",
    Location = new Point(100, 200),
    Size = new Size(140, 44),
    Font = new Font("SF Pro Display", 17F, FontStyle.Regular)
};
```

### **4. C# код для создания поля ввода:**
```csharp
var textBox = new MacOSTextBox
{
    Location = new Point(100, 100),
    Size = new Size(300, 44),
    Font = new Font("SF Pro Text", 17F, FontStyle.Regular)
};
```

---

## 🚀 Команды для быстрого запуска

### **1. Установка шрифтов (PowerShell):**
```powershell
# Скачать SF Pro шрифты
Invoke-WebRequest -Uri "https://developer.apple.com/design/downloads/SF-Pro.dmg" -OutFile "SF-Pro.dmg"
# Установить шрифты вручную из DMG файла
```

### **2. Создание папки для иконок:**
```powershell
mkdir public\icons
mkdir public\icons\attractions
mkdir public\icons\tickets
mkdir public\icons\users
mkdir public\icons\system
```

### **3. Запуск приложения:**
```powershell
# Windows Forms
cd AquaparkApp
dotnet run

# Веб-компоненты
# Открыть index.html в браузере
```

---

## ✅ Чек-лист для быстрой проверки

### **После внесения изменений проверить:**
- [ ] Цвета изменились на синие (#007AFF)
- [ ] Скругления увеличились (12-16px)
- [ ] Шрифты стали SF Pro
- [ ] Эффекты размытия усилились (20px)
- [ ] Тени стали мягче
- [ ] Анимации плавнее
- [ ] Иконки консистентны

### **Тестирование:**
- [ ] Открыть все формы приложения
- [ ] Проверить веб-компоненты в браузере
- [ ] Протестировать на разных разрешениях
- [ ] Проверить темную тему (если есть)

**Готово! Теперь приложение выглядит как macOS Sequoia! 🎨✨**
