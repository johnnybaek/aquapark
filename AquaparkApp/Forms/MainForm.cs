using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;
using AquaparkApp.BLL;
using AquaparkApp.DAL;

namespace AquaparkApp.Forms
{
    public partial class MainForm : Form
    {
        private Panel _sidebarPanel;
        private Panel _contentPanel;
        private Panel _headerPanel;
        private Label _welcomeLabel;
        private Button _loginButton;
        private Button _registerButton;
        private Button _logoutButton;
        private FlowLayoutPanel _attractionsPanel;
        private TabControl _mainTabControl;
        
        // Сервисы
        private TicketService _ticketService;
        private PaymentService _paymentService;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            SetupUI();
        }

        private void InitializeServices()
        {
            _ticketService = new TicketService();
            _paymentService = new PaymentService();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Аквапарк \"Водный мир\" - Система управления";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 248, 248);
            this.Font = new Font("SF Pro Display", 12F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.MinimumSize = new Size(1200, 800);
            
            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Создаем панель заголовка
            CreateHeaderPanel();
            
            // Создаем боковую панель
            CreateSidebarPanel();
            
            // Создаем основную панель контента
            CreateContentPanel();
            
            // Создаем панель аттракционов
            CreateAttractionsPanel();
            
            // Создаем главные вкладки
            CreateMainTabs();
        }

        private void CreateHeaderPanel()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 122, 255)
            };

            // Логотип
            var logoLabel = new Label
            {
                Text = "🌊 Аквапарк \"Водный мир\"",
                Font = new Font("SF Pro Text", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Кнопки авторизации
            // Создаем панель для кнопок справа
            var rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 250,
                Height = 80
            };

            _loginButton = new MacOSButton
            {
                Text = "Войти",
                Size = new Size(100, 35),
                Location = new Point(10, 20),
                Font = new Font("SF Pro Text", 12F, FontStyle.Regular),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _loginButton.Click += LoginButton_Click;

            _registerButton = new MacOSButton
            {
                Text = "Регистрация",
                Size = new Size(120, 35),
                Location = new Point(120, 20),
                Font = new Font("SF Pro Text", 12F, FontStyle.Regular),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _registerButton.Click += RegisterButton_Click;

            _logoutButton = new MacOSButton
            {
                Text = "Выйти",
                Size = new Size(100, 35),
                Location = new Point(10, 20),
                Font = new Font("SF Pro Text", 12F, FontStyle.Regular),
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _logoutButton.Click += LogoutButton_Click;

            _welcomeLabel = new Label
            {
                Text = "Добро пожаловать!",
                Font = new Font("SF Pro Text", 14F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(10, 25),
                AutoSize = true,
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            rightPanel.Controls.AddRange(new Control[] { _loginButton, _registerButton, _logoutButton, _welcomeLabel });

            _headerPanel.Controls.AddRange(new Control[] { logoLabel, rightPanel });
            this.Controls.Add(_headerPanel);
        }

        private void CreateSidebarPanel()
        {
            _sidebarPanel = new GlassPanel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Создаем панель для прокрутки меню
            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(5),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Меню навигации - названия соответствуют таблицам БД
            var menuItems = new[]
            {
                new { Text = "🏠 Главная", Tag = "home" },
                new { Text = "👥 Клиенты", Tag = "clients" },
                new { Text = "🎫 Билеты", Tag = "tickets" },
                new { Text = "🛍️ Услуги", Tag = "services" },
                new { Text = "🏊 Зоны", Tag = "zones" },
                new { Text = "👷 Сотрудники", Tag = "employees" },
                new { Text = "📅 Расписание", Tag = "schedule" },
                new { Text = "🎒 Инвентарь", Tag = "inventory" },
                new { Text = "🏃 Аренда", Tag = "rentals" },
                new { Text = "🚪 Посещения", Tag = "visits" },
                new { Text = "💳 Оплаты", Tag = "payments" },
                new { Text = "📊 Отчеты", Tag = "reports" },
                new { Text = "🎬 Видео", Tag = "video" },
                new { Text = "🗺️ Карта", Tag = "map" },
                new { Text = "⚙️ Настройки", Tag = "settings" },
                new { Text = "👤 Профиль", Tag = "profile" },
                new { Text = "🔧 Админ-панель", Tag = "admin" }
            };

            int y = 10;
            foreach (var item in menuItems)
            {
                var menuButton = new MacOSButton
                {
                    Text = item.Text,
                    Size = new Size(220, 35),
                    Location = new Point(10, y),
                    Font = new Font("SF Pro Text", 10F, FontStyle.Regular),
                    Tag = item.Tag
                };
                menuButton.Click += MenuButton_Click;
                scrollPanel.Controls.Add(menuButton);
                y += 40;
            }

            _sidebarPanel.Controls.Add(scrollPanel);
            this.Controls.Add(_sidebarPanel);
        }

        private void CreateContentPanel()
        {
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 248, 248),
                Padding = new Padding(20)
            };

            this.Controls.Add(_contentPanel);
        }

        private void CreateAttractionsPanel()
        {
            _attractionsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };

            _contentPanel.Controls.Add(_attractionsPanel);
        }

        private void CreateMainTabs()
        {
            _mainTabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Appearance = TabAppearance.FlatButtons,
                ItemSize = new Size(120, 40),
                SizeMode = TabSizeMode.Fixed
            };

            // Стилизация вкладок
            _mainTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            _mainTabControl.DrawItem += TabControl_DrawItem;

            // Добавляем вкладки
            var homeTab = new TabPage("🏠 Главная");
            var attractionsTab = new TabPage("🎢 Аттракционы");
            var ticketsTab = new TabPage("🎫 Билеты");
            var reportsTab = new TabPage("📊 Отчеты");
            var settingsTab = new TabPage("⚙️ Настройки");

            _mainTabControl.TabPages.AddRange(new TabPage[] { homeTab, attractionsTab, ticketsTab, reportsTab, settingsTab });

            _contentPanel.Controls.Add(_mainTabControl);
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabPage = tabControl.TabPages[e.Index];
            var tabRect = tabControl.GetTabRect(e.Index);

            // Рисуем фон вкладки
            using (var brush = new LinearGradientBrush(tabRect, 
                Color.FromArgb(240, 240, 240), 
                Color.FromArgb(220, 220, 220), 
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // Рисуем границу
            using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
            {
                e.Graphics.DrawRectangle(pen, tabRect);
            }

            // Рисуем текст
            var textRect = new Rectangle(tabRect.X + 5, tabRect.Y + 5, tabRect.Width - 10, tabRect.Height - 10);
            TextRenderer.DrawText(e.Graphics, tabPage.Text, 
                new Font("SF Pro Display", 11F, FontStyle.Regular), 
                textRect, Color.FromArgb(51, 51, 51), 
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }


        private void MenuButton_Click(object sender, EventArgs e)
        {
            var button = sender as MacOSButton;
            var tag = button.Tag.ToString();

            switch (tag)
            {
                case "home":
                    ShowHomePage();
                    break;
                case "clients":
                    ShowClientsPage();
                    break;
                case "tickets":
                    ShowTicketsPage();
                    break;
                case "services":
                    ShowServicesPage();
                    break;
                case "zones":
                    ShowZonesPage();
                    break;
                case "employees":
                    ShowEmployeesPage();
                    break;
                case "schedule":
                    ShowSchedulePage();
                    break;
                case "inventory":
                    ShowInventoryPage();
                    break;
                case "rentals":
                    ShowRentalsPage();
                    break;
                case "visits":
                    ShowVisitsPage();
                    break;
                case "payments":
                    ShowPaymentsPage();
                    break;
                case "reports":
                    ShowReportsPage();
                    break;
                case "video":
                    ShowVideoPlayer();
                    break;
                case "map":
                    ShowInteractiveMap();
                    break;
                case "settings":
                    ShowSettingsPage();
                    break;
                case "profile":
                    ShowPlaceholderPage("👤 Профиль", "Управление профилем пользователя");
                    break;
                case "admin":
                    ShowPlaceholderPage("🔧 Админ-панель", "Административные функции");
                    break;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage("🔐 Вход", "Функция входа в систему");
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage("📝 Регистрация", "Функция регистрации нового пользователя");
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage("🚪 Выход", "Выход из системы");
        }

        private void UpdateUserInterface()
        {
            _welcomeLabel.Text = "Добро пожаловать в систему управления аквапарком!";
        }

        // Методы для отображения различных страниц
        private void ShowHomePage()
        {
            _contentPanel.Controls.Clear();
            
            // Создаем панель с прокруткой для контента
            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 10, 20, 20)
            };

            var welcomeLabel = new Label
            {
                Text = "🌊 Добро пожаловать в систему управления аквапарком!",
                Font = new Font("SF Pro Text", 28F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(20, 10),
                AutoSize = true
            };

            var descLabel = new Label
            {
                Text = "Комплексная система для управления всеми аспектами работы аквапарка",
                Font = new Font("SF Pro Text", 16F, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(20, 60),
                AutoSize = true
            };

            // Создаем панель с функциями
            var featuresPanel = new Panel
            {
                Location = new Point(20, 110),
                Size = new Size(800, 350),
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.FixedSingle
            };

            var featuresLabel = new Label
            {
                Text = "🎯 Основные возможности системы:\n\n" +
                       "👥 Управление клиентами - регистрация, редактирование, поиск\n" +
                       "🎫 Система билетов - продажа, валидация, статистика\n" +
                       "🛍️ Управление услугами - каталог услуг, ценообразование\n" +
                       "🏊 Зоны аквапарка - планировка, вместимость, контроль\n" +
                       "👷 Персонал - сотрудники, расписание, зоны ответственности\n" +
                       "🎒 Инвентарь - учет, аренда, статус оборудования\n" +
                       "🚪 Посещения - контроль входа/выхода, статистика\n" +
                       "💳 Платежи - обработка оплат, отчеты по выручке\n" +
                       "📊 Отчеты - аналитика, экспорт в Excel\n" +
                       "🎬 Видео - промо-материалы аквапарка\n" +
                       "🗺️ Интерактивная карта - визуализация зон",
                Font = new Font("SF Pro Text", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(15, 15),
                Size = new Size(770, 320),
                TextAlign = ContentAlignment.TopLeft
            };

            featuresPanel.Controls.Add(featuresLabel);
            scrollPanel.Controls.AddRange(new Control[] { welcomeLabel, descLabel, featuresPanel });
            _contentPanel.Controls.Add(scrollPanel);
        }

        private void ShowClientsPage()
        {
            ShowPlaceholderPage("👥 Клиенты", "Управление клиентами аквапарка");
        }

        private void ShowTicketsPage()
        {
            ShowPlaceholderPage("🎫 Билеты", "Управление билетами и бронированием");
        }

        private void ShowServicesPage()
        {
            ShowPlaceholderPage("🛍️ Услуги", "Управление услугами аквапарка");
        }

        private void ShowZonesPage()
        {
            ShowPlaceholderPage("🏊 Зоны", "Управление зонами аквапарка");
        }

        private void ShowEmployeesPage()
        {
            ShowPlaceholderPage("👷 Сотрудники", "Управление персоналом");
        }

        private void ShowSchedulePage()
        {
            ShowPlaceholderPage("📅 Расписание", "Управление рабочим расписанием");
        }

        private void ShowInventoryPage()
        {
            ShowPlaceholderPage("🎒 Инвентарь", "Управление инвентарем");
        }

        private void ShowRentalsPage()
        {
            ShowPlaceholderPage("🏃 Аренда", "Управление арендой инвентаря");
        }

        private void ShowVisitsPage()
        {
            ShowPlaceholderPage("🚪 Посещения", "Управление посещениями клиентов");
        }

        private void ShowPaymentsPage()
        {
            ShowPlaceholderPage("💳 Оплаты", "Управление платежами");
        }

        private void ShowPlaceholderPage(string title, string description)
        {
            _contentPanel.Controls.Clear();
            
            // Создаем панель с прокруткой для контента
            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 10, 20, 20)
            };
            
            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("SF Pro Text", 28F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(20, 10),
                AutoSize = true
            };

            var descLabel = new Label
            {
                Text = description,
                Font = new Font("SF Pro Text", 16F, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(20, 60),
                AutoSize = true
            };

            var statusLabel = new Label
            {
                Text = "Функционал находится в разработке",
                Font = new Font("SF Pro Text", 14F, FontStyle.Italic),
                ForeColor = Color.FromArgb(150, 150, 150),
                Location = new Point(20, 110),
                AutoSize = true
            };

            // Добавляем информационную панель
            var infoPanel = new Panel
            {
                Location = new Point(20, 160),
                Size = new Size(800, 200),
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.FixedSingle
            };

            var infoLabel = new Label
            {
                Text = "📋 Информация о модуле:\n\n" +
                       "• Данный раздел предназначен для управления соответствующими данными\n" +
                       "• В будущем здесь будет реализован полный функционал CRUD операций\n" +
                       "• Поддерживается экспорт данных в Excel\n" +
                       "• Интегрирована система отчетов",
                Font = new Font("SF Pro Text", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(15, 15),
                Size = new Size(770, 170),
                TextAlign = ContentAlignment.TopLeft
            };

            infoPanel.Controls.Add(infoLabel);
            scrollPanel.Controls.AddRange(new Control[] { titleLabel, descLabel, statusLabel, infoPanel });
            _contentPanel.Controls.Add(scrollPanel);
        }

        private void ShowReportsPage()
        {
            _contentPanel.Controls.Clear();
            var reportsForm = new ReportsForm();
            reportsForm.TopLevel = false;
            reportsForm.Dock = DockStyle.Fill;
            _contentPanel.Controls.Add(reportsForm);
            reportsForm.Show();
        }

        private void ShowVideoPlayer()
        {
            var videoForm = new VideoPlayerForm();
            videoForm.ShowDialog();
        }

        private void ShowInteractiveMap()
        {
            _contentPanel.Controls.Clear();
            var mapForm = new InteractiveMapForm();
            mapForm.TopLevel = false;
            mapForm.Dock = DockStyle.Fill;
            _contentPanel.Controls.Add(mapForm);
            mapForm.Show();
        }

        private void ShowSettingsPage()
        {
            _contentPanel.Controls.Clear();
            var settingsLabel = new Label
            {
                Text = "⚙️ Настройки системы",
                Font = new Font("SF Pro Display", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 50),
                AutoSize = true
            };
            _contentPanel.Controls.Add(settingsLabel);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Рисуем градиентный фон
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(248, 248, 248),
                Color.FromArgb(235, 235, 235),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
