using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;
using AquaparkApp.BLL;

namespace AquaparkApp.Forms
{
    public partial class AdminPanelForm : Form
    {
        private User _currentUser;
        private TabControl _adminTabs;
        private DataGridView _usersDataGrid;
        private DataGridView _attractionsDataGrid;
        private DataGridView _ordersDataGrid;
        private DataGridView _ticketsDataGrid;
        private MacOSButton _addUserButton;
        private MacOSButton _editUserButton;
        private MacOSButton _deleteUserButton;
        private MacOSButton _addAttractionButton;
        private MacOSButton _editAttractionButton;
        private MacOSButton _deleteAttractionButton;
        private MacOSButton _exportReportButton;
        private MacOSButton _refreshButton;
        private Label _titleLabel;
        private Panel _mainPanel;
        private GlassPanel _glassPanel;

        // Сервисы
        private AuthenticationService _authService;
        private AttractionService _attractionService;
        private TicketService _ticketService;
        private ReportService _reportService;

        public AdminPanelForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeServices();
            InitializeComponent();
            SetupUI();
            LoadData();
        }

        private void InitializeServices()
        {
            var connectionString = "Host=localhost;Database=aquapark_db;Username=postgres;Password=password";
            _authService = new AuthenticationService(connectionString);
            _attractionService = new AttractionService(connectionString);
            _ticketService = new TicketService(connectionString);
            _reportService = new ReportService(connectionString);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Админ-панель - Аквапарк \"Водный мир\"";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Font = new Font("SF Pro Display", 12F, FontStyle.Regular);
            
            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Создаем главную панель
            _mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            // Создаем стеклянную панель
            _glassPanel = new GlassPanel
            {
                Size = new Size(1150, 750),
                Location = new Point(25, 25),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = "⚙️ Админ-панель",
                Font = new Font("SF Pro Display", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 20),
                Size = new Size(400, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Создаем вкладки
            _adminTabs = new TabControl
            {
                Location = new Point(50, 80),
                Size = new Size(1050, 600),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Appearance = TabAppearance.FlatButtons,
                ItemSize = new Size(120, 40),
                SizeMode = TabSizeMode.Fixed
            };

            // Стилизация вкладок
            _adminTabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            _adminTabs.DrawItem += TabControl_DrawItem;

            // Вкладка "Пользователи"
            var usersTab = new TabPage("👥 Пользователи");
            CreateUsersTab(usersTab);

            // Вкладка "Аттракционы"
            var attractionsTab = new TabPage("🎢 Аттракционы");
            CreateAttractionsTab(attractionsTab);

            // Вкладка "Заказы"
            var ordersTab = new TabPage("📋 Заказы");
            CreateOrdersTab(ordersTab);

            // Вкладка "Билеты"
            var ticketsTab = new TabPage("🎫 Билеты");
            CreateTicketsTab(ticketsTab);

            // Вкладка "Отчеты"
            var reportsTab = new TabPage("📊 Отчеты");
            CreateReportsTab(reportsTab);

            _adminTabs.TabPages.AddRange(new TabPage[] { usersTab, attractionsTab, ordersTab, ticketsTab, reportsTab });

            // Кнопки управления
            _refreshButton = new MacOSButton
            {
                Text = "🔄 Обновить",
                Location = new Point(50, 700),
                Size = new Size(120, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _refreshButton.Click += RefreshButton_Click;

            _exportReportButton = new MacOSButton
            {
                Text = "📊 Экспорт отчета",
                Location = new Point(180, 700),
                Size = new Size(150, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _exportReportButton.Click += ExportReportButton_Click;

            // Добавляем контролы
            _glassPanel.Controls.AddRange(new Control[] 
            {
                _titleLabel, _adminTabs, _refreshButton, _exportReportButton
            });

            _mainPanel.Controls.Add(_glassPanel);
            this.Controls.Add(_mainPanel);
        }

        private void CreateUsersTab(TabPage tab)
        {
            // DataGridView для пользователей
            _usersDataGrid = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(1000, 400),
                Font = new Font("SF Pro Display", 10F, FontStyle.Regular),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 230, 230),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Настройка стиля
            _usersDataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
            _usersDataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _usersDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            _usersDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);

            // Кнопки управления пользователями
            _addUserButton = new MacOSButton
            {
                Text = "➕ Добавить",
                Location = new Point(20, 440),
                Size = new Size(120, 35),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _addUserButton.Click += AddUserButton_Click;

            _editUserButton = new MacOSButton
            {
                Text = "✏️ Редактировать",
                Location = new Point(150, 440),
                Size = new Size(140, 35),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _editUserButton.Click += EditUserButton_Click;

            _deleteUserButton = new MacOSButton
            {
                Text = "🗑️ Удалить",
                Location = new Point(300, 440),
                Size = new Size(120, 35),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _deleteUserButton.Click += DeleteUserButton_Click;

            tab.Controls.AddRange(new Control[] 
            {
                _usersDataGrid, _addUserButton, _editUserButton, _deleteUserButton
            });
        }

        private void CreateAttractionsTab(TabPage tab)
        {
            // DataGridView для аттракционов
            _attractionsDataGrid = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(1000, 400),
                Font = new Font("SF Pro Display", 10F, FontStyle.Regular),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 230, 230),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Настройка стиля
            _attractionsDataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
            _attractionsDataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _attractionsDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            _attractionsDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);

            // Кнопки управления аттракционами
            _addAttractionButton = new MacOSButton
            {
                Text = "➕ Добавить",
                Location = new Point(20, 440),
                Size = new Size(120, 35),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _addAttractionButton.Click += AddAttractionButton_Click;

            _editAttractionButton = new MacOSButton
            {
                Text = "✏️ Редактировать",
                Location = new Point(150, 440),
                Size = new Size(140, 35),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _editAttractionButton.Click += EditAttractionButton_Click;

            _deleteAttractionButton = new MacOSButton
            {
                Text = "🗑️ Удалить",
                Location = new Point(300, 440),
                Size = new Size(120, 35),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _deleteAttractionButton.Click += DeleteAttractionButton_Click;

            tab.Controls.AddRange(new Control[] 
            {
                _attractionsDataGrid, _addAttractionButton, _editAttractionButton, _deleteAttractionButton
            });
        }

        private void CreateOrdersTab(TabPage tab)
        {
            _ordersDataGrid = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(1000, 500),
                Font = new Font("SF Pro Display", 10F, FontStyle.Regular),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 230, 230),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            _ordersDataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
            _ordersDataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _ordersDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            _ordersDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);

            tab.Controls.Add(_ordersDataGrid);
        }

        private void CreateTicketsTab(TabPage tab)
        {
            _ticketsDataGrid = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(1000, 500),
                Font = new Font("SF Pro Display", 10F, FontStyle.Regular),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 230, 230),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            _ticketsDataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
            _ticketsDataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _ticketsDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            _ticketsDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);

            tab.Controls.Add(_ticketsDataGrid);
        }

        private void CreateReportsTab(TabPage tab)
        {
            var reportPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1000, 500),
                BackColor = Color.Transparent
            };

            var salesReportButton = new MacOSButton
            {
                Text = "📊 Отчет по продажам",
                Location = new Point(50, 50),
                Size = new Size(200, 60),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            salesReportButton.Click += SalesReportButton_Click;

            var userReportButton = new MacOSButton
            {
                Text = "👥 Отчет по пользователям",
                Location = new Point(270, 50),
                Size = new Size(200, 60),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            userReportButton.Click += UserReportButton_Click;

            var attendanceReportButton = new MacOSButton
            {
                Text = "🎫 Отчет по посещаемости",
                Location = new Point(490, 50),
                Size = new Size(200, 60),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            attendanceReportButton.Click += AttendanceReportButton_Click;

            var videoButton = new MacOSButton
            {
                Text = "🎬 Демо-ролик",
                Location = new Point(710, 50),
                Size = new Size(200, 60),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            videoButton.Click += VideoButton_Click;

            reportPanel.Controls.AddRange(new Control[] 
            {
                salesReportButton, userReportButton, attendanceReportButton, videoButton
            });

            tab.Controls.Add(reportPanel);
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

        private async void LoadData()
        {
            try
            {
                // Загружаем пользователей
                var users = await _authService.GetTopUsersBySpendingAsync(100);
                _usersDataGrid.DataSource = users.ToList();

                // Загружаем аттракционы
                var attractions = await _attractionService.GetAllAttractionsAsync();
                _attractionsDataGrid.DataSource = attractions.ToList();

                // Здесь можно добавить загрузку заказов и билетов
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ExportReportButton_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                Title = "Экспорт отчета"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var startDate = DateTime.Today.AddDays(-30);
                    var endDate = DateTime.Today;
                    _reportService.ExportSalesReportToExcelAsync(startDate, endDate, saveDialog.FileName);
                    MessageBox.Show("Отчет успешно экспортирован!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            // Здесь можно открыть форму добавления пользователя
            MessageBox.Show("Функция добавления пользователя будет реализована", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EditUserButton_Click(object sender, EventArgs e)
        {
            // Здесь можно открыть форму редактирования пользователя
            MessageBox.Show("Функция редактирования пользователя будет реализована", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteUserButton_Click(object sender, EventArgs e)
        {
            // Здесь можно реализовать удаление пользователя
            MessageBox.Show("Функция удаления пользователя будет реализована", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddAttractionButton_Click(object sender, EventArgs e)
        {
            // Здесь можно открыть форму добавления аттракциона
            MessageBox.Show("Функция добавления аттракциона будет реализована", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EditAttractionButton_Click(object sender, EventArgs e)
        {
            // Здесь можно открыть форму редактирования аттракциона
            MessageBox.Show("Функция редактирования аттракциона будет реализована", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteAttractionButton_Click(object sender, EventArgs e)
        {
            // Здесь можно реализовать удаление аттракциона
            MessageBox.Show("Функция удаления аттракциона будет реализована", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SalesReportButton_Click(object sender, EventArgs e)
        {
            var startDate = DateTime.Today.AddDays(-30);
            var endDate = DateTime.Today;
            var report = _reportService.GetSalesReportAsync(startDate, endDate).Result;
            
            var reportForm = new ReportViewerForm("Отчет по продажам", report);
            reportForm.ShowDialog();
        }

        private void UserReportButton_Click(object sender, EventArgs e)
        {
            var report = _reportService.GetUserReportAsync().Result;
            
            var reportForm = new ReportViewerForm("Отчет по пользователям", report);
            reportForm.ShowDialog();
        }

        private void AttendanceReportButton_Click(object sender, EventArgs e)
        {
            var startDate = DateTime.Today.AddDays(-30);
            var endDate = DateTime.Today;
            var report = _reportService.GetAttendanceReportAsync(startDate, endDate).Result;
            
            var reportForm = new ReportViewerForm("Отчет по посещаемости", report);
            reportForm.ShowDialog();
        }

        private void VideoButton_Click(object sender, EventArgs e)
        {
            var videoForm = new VideoPlayerForm();
            videoForm.ShowDialog();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Рисуем градиентный фон
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(240, 240, 240),
                Color.FromArgb(220, 220, 220),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            // Рисуем тень формы
            var shadowRect = new Rectangle(5, 5, this.Width - 10, this.Height - 10);
            using (var shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(shadowBrush, shadowRect);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Handle, 0xA1, new IntPtr(0x2), IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }
    }
}
