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
        private User? _currentUser;
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
        private AttractionService _attractionService;
        private AuthenticationService _authService;
        private TicketService _ticketService;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            SetupUI();
            LoadAttractions();
        }

        private void InitializeServices()
        {
            var connectionString = "Host=localhost;Database=aquapark_db;Username=postgres;Password=password";
            _attractionService = new AttractionService(connectionString);
            _authService = new AuthenticationService(connectionString);
            _ticketService = new TicketService(connectionString);
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
                Font = new Font("SF Pro Display", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Кнопки авторизации
            _loginButton = new MacOSButton
            {
                Text = "Войти",
                Size = new Size(100, 35),
                Location = new Point(1200, 20),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _loginButton.Click += LoginButton_Click;

            _registerButton = new MacOSButton
            {
                Text = "Регистрация",
                Size = new Size(120, 35),
                Location = new Point(1320, 20),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _registerButton.Click += RegisterButton_Click;

            _logoutButton = new MacOSButton
            {
                Text = "Выйти",
                Size = new Size(100, 35),
                Location = new Point(1200, 20),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Visible = false
            };
            _logoutButton.Click += LogoutButton_Click;

            _welcomeLabel = new Label
            {
                Text = "Добро пожаловать!",
                Font = new Font("SF Pro Display", 14F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(1000, 25),
                AutoSize = true,
                Visible = false
            };

            _headerPanel.Controls.AddRange(new Control[] { logoLabel, _loginButton, _registerButton, _logoutButton, _welcomeLabel });
            this.Controls.Add(_headerPanel);
        }

        private void CreateSidebarPanel()
        {
            _sidebarPanel = new GlassPanel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Меню навигации
            var menuItems = new[]
            {
                new { Text = "🏠 Главная", Tag = "home" },
                new { Text = "🎢 Аттракционы", Tag = "attractions" },
                new { Text = "🎫 Мои билеты", Tag = "tickets" },
                new { Text = "📊 Отчеты", Tag = "reports" },
                new { Text = "⚙️ Настройки", Tag = "settings" },
                new { Text = "👤 Профиль", Tag = "profile" },
                new { Text = "🔧 Админ-панель", Tag = "admin" }
            };

            int y = 20;
            foreach (var item in menuItems)
            {
                var menuButton = new MacOSButton
                {
                    Text = item.Text,
                    Size = new Size(210, 45),
                    Location = new Point(20, y),
                    Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                    Tag = item.Tag
                };
                menuButton.Click += MenuButton_Click;
                _sidebarPanel.Controls.Add(menuButton);
                y += 55;
            }

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

        private async void LoadAttractions()
        {
            try
            {
                var attractions = await _attractionService.GetAllAttractionsAsync();
                
                _attractionsPanel.Controls.Clear();
                
                foreach (var attraction in attractions.Where(a => a.IsActive))
                {
                    var card = new AttractionCard
                    {
                        Attraction = attraction
                    };
                    card.BookClicked += AttractionCard_BookClicked;
                    _attractionsPanel.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки аттракционов: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AttractionCard_BookClicked(object sender, Attraction attraction)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Для бронирования необходимо войти в систему.", "Требуется авторизация", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Открываем форму бронирования
            var bookingForm = new BookingForm(attraction, _currentUser, _ticketService);
            if (bookingForm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Билет успешно забронирован!", "Успех", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            var button = sender as MacOSButton;
            var tag = button.Tag.ToString();

            switch (tag)
            {
                case "home":
                    _mainTabControl.SelectedIndex = 0;
                    break;
                case "attractions":
                    _mainTabControl.SelectedIndex = 1;
                    break;
                case "tickets":
                    _mainTabControl.SelectedIndex = 2;
                    break;
                case "reports":
                    _mainTabControl.SelectedIndex = 3;
                    break;
                case "settings":
                    _mainTabControl.SelectedIndex = 4;
                    break;
                case "profile":
                    if (_currentUser != null)
                    {
                        var profileForm = new ProfileForm(_currentUser);
                        profileForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Для просмотра профиля необходимо войти в систему.", 
                            "Требуется авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case "admin":
                    if (_currentUser != null && _currentUser.IsAdmin)
                    {
                        var adminForm = new AdminPanelForm(_currentUser);
                        adminForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Доступ к админ-панели ограничен.", 
                            "Недостаточно прав", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm(_authService);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                _currentUser = loginForm.CurrentUser;
                UpdateUserInterface();
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm(_authService);
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                _currentUser = registerForm.CurrentUser;
                UpdateUserInterface();
            }
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            _currentUser = null;
            UpdateUserInterface();
        }

        private void UpdateUserInterface()
        {
            if (_currentUser != null)
            {
                _welcomeLabel.Text = $"Добро пожаловать, {_currentUser.FirstName}!";
                _welcomeLabel.Visible = true;
                _loginButton.Visible = false;
                _registerButton.Visible = false;
                _logoutButton.Visible = true;
            }
            else
            {
                _welcomeLabel.Visible = false;
                _loginButton.Visible = true;
                _registerButton.Visible = true;
                _logoutButton.Visible = false;
            }
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
