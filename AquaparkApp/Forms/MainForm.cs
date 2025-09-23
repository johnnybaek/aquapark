using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
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
        // Сервисы
        private TicketService _ticketService;
        private PaymentService _paymentService;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            SetupUI();
        }

        private System.Data.DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var dataTable = new System.Data.DataTable();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string) || Nullable.GetUnderlyingType(p.PropertyType) != null)
                .ToList();

            foreach (var prop in props)
            {
                var columnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dataTable.Columns.Add(prop.Name, columnType);
            }

            foreach (var item in items)
            {
                var values = props.Select(p => p.GetValue(item, null) ?? DBNull.Value).ToArray();
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private void InitializeServices()
        {
            _ticketService = new TicketService();
            _paymentService = new PaymentService();
        }


        private void SetupUI()
        {
            // Настраиваем панель заголовка
            SetupHeaderPanel();
            
            // Настраиваем боковую панель
            SetupSidebarPanel();
            
            // Настраиваем основную панель контента
            SetupContentPanel();
            
            // Настраиваем панель аттракционов
            SetupAttractionsPanel();
            
            // Настраиваем главные вкладки
            SetupMainTabs();
        }

        private void SetupHeaderPanel()
        {
            // Логотип
            var logoLabel = new Label
            {
                Text = "🌊 Аквапарк \"Водный мир\"",
                Font = new Font("SF Pro Text", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            _headerPanel.Controls.Add(logoLabel);

            // Настраиваем обработчики событий
            _loginButton.Click += LoginButton_Click;
            _registerButton.Click += RegisterButton_Click;
            _logoutButton.Click += LogoutButton_Click;
        }

        private void SetupSidebarPanel()
        {
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
                    Tag = item.Tag,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left
                };
                menuButton.Click += MenuButton_Click;
                scrollPanel.Controls.Add(menuButton);
                y += 40;
            }

            // Обновляем ширину кнопок при изменении размера боковой панели
            scrollPanel.Resize += (s, e) => UpdateSidebarButtonsLayout(scrollPanel);
            UpdateSidebarButtonsLayout(scrollPanel);

            _sidebarPanel.Controls.Add(scrollPanel);
        }

        private void UpdateSidebarButtonsLayout(Panel container)
        {
            int leftPadding = 10;
            int rightPadding = 10;
            int verticalSpacing = 5;
            int y = 10;
            int maxWidth = Math.Max(120, container.ClientSize.Width - leftPadding - rightPadding);

            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is MacOSButton btn)
                {
                    // Ширина по тексту с небольшим запасом, но не больше доступной
                    var textSize = TextRenderer.MeasureText(btn.Text, btn.Font);
                    int desiredWidth = Math.Min(maxWidth, textSize.Width + 24);
                    btn.Left = leftPadding;
                    btn.Top = y;
                    btn.Width = desiredWidth;
                    y += btn.Height + verticalSpacing;
                }
            }
        }

        private void SetupContentPanel()
        {
            // Панель контента уже настроена в Designer
        }

        private void SetupAttractionsPanel()
        {
            // Панель аттракционов уже настроена в Designer
        }

        private void SetupMainTabs()
        {
            // Стилизация вкладок
            _mainTabControl.DrawItem += TabControl_DrawItem;

            // Добавляем вкладки
            var homeTab = new TabPage("🏠 Главная");
            var attractionsTab = new TabPage("🎢 Аттракционы");
            var ticketsTab = new TabPage("🎫 Билеты");
            var reportsTab = new TabPage("📊 Отчеты");
            var settingsTab = new TabPage("⚙️ Настройки");

            _mainTabControl.TabPages.AddRange(new TabPage[] { homeTab, attractionsTab, ticketsTab, reportsTab, settingsTab });
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

        private async void ShowClientsPage()
        {
            await ShowDataPageAsync("👥 Клиенты", async () =>
            {
                var repo = new ClientRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowTicketsPage()
        {
            await ShowDataPageAsync("🎫 Билеты", async () =>
            {
                var repo = new TicketRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowServicesPage()
        {
            await ShowDataPageAsync("🛍️ Услуги", async () =>
            {
                var repo = new ServiceRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowZonesPage()
        {
            await ShowDataPageAsync("🏊 Зоны", async () =>
            {
                var repo = new ZoneRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowEmployeesPage()
        {
            await ShowDataPageAsync("👷 Сотрудники", async () =>
            {
                var repo = new EmployeeRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowSchedulePage()
        {
            await ShowDataPageAsync("📅 Расписание", async () =>
            {
                var repo = new ScheduleRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowInventoryPage()
        {
            await ShowDataPageAsync("🎒 Инвентарь", async () =>
            {
                var repo = new InventoryRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowRentalsPage()
        {
            await ShowDataPageAsync("🏃 Аренда", async () =>
            {
                var repo = new InventoryRentalRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowVisitsPage()
        {
            await ShowDataPageAsync("🚪 Посещения", async () =>
            {
                var repo = new VisitRepository();
                return await repo.GetAllAsync();
            });
        }

        private async void ShowPaymentsPage()
        {
            await ShowDataPageAsync("💳 Оплаты", async () =>
            {
                var repo = new PaymentRepository();
                return await repo.GetAllAsync();
            });
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

        private async Task ShowDataPageAsync<T>(string title, Func<Task<IEnumerable<T>>> loadData)
        {
            _contentPanel.Controls.Clear();

            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 10, 20, 20)
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("SF Pro Text", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(20, 10),
                AutoSize = true
            };

            var grid = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(Math.Max(800, _contentPanel.Width - 80), Math.Max(500, _contentPanel.Height - 140)),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                GridColor = Color.FromArgb(230, 230, 230),
                RowHeadersVisible = false
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);
            grid.EnableHeadersVisualStyles = false;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            scrollPanel.Controls.Add(titleLabel);
            scrollPanel.Controls.Add(grid);
            _contentPanel.Controls.Add(scrollPanel);

            try
            {
                var data = (await loadData()).ToList();
                var table = ToDataTable(data);
                grid.DataSource = table;
                ConfigureGridAppearance(grid);
                ConfigureGridColumns<T>(grid);
                // Автоподбор по данным
                grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if (col.Visible)
                    {
                        col.SortMode = DataGridViewColumnSortMode.Automatic;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных для '{title}': {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGridAppearance(DataGridView grid)
        {
            // Общий вид
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersVisible = true;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToOrderColumns = true;
            grid.MultiSelect = false;

            // Высота строк и отступы
            grid.RowTemplate.Height = 36;
            grid.DefaultCellStyle.Padding = new Padding(6, 6, 6, 6);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 10, 6, 10);

            // Цвета выбора
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(229, 241, 255);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 64, 140);

            // Цвета шапки (контрастные, чтобы текст был виден всегда)
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 245, 245);
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 40;

            // Скругленный стиль шапки (имитация) и фон
            grid.BorderStyle = BorderStyle.None;

            // Улучшаем скролл (DoubleBuffer через отражение)
            typeof(DataGridView)
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)?
                .SetValue(grid, true, null);
        }

        private void ConfigureGridColumns<T>(DataGridView grid)
        {
            var modelType = typeof(T);

            // Словарь отображаемых заголовков по свойствам
            var headers = GetHeaderMapForType(modelType);

            // Форматы по свойствам
            var dateColumns = new HashSet<string>(new[] { "BirthDate", "RegistrationDate", "PurchaseDate", "ValidUntil", "PaymentDate", "WorkDate", "RentalDate", "ReturnDate" });
            var dateTimeColumns = new HashSet<string>(new[] { "EntryTime", "ExitTime" });
            var moneyColumns = new HashSet<string>(new[] { "Price", "Amount", "DepositAmount" });
            var integerColumnsHideZeros = new HashSet<string>(new[] { "ResponsibleEmployeeId" });

            // Скрываем навигационные и коллекционные свойства
            foreach (DataGridViewColumn column in grid.Columns)
            {
                var key = string.IsNullOrEmpty(column.DataPropertyName) ? column.Name : column.DataPropertyName;
                var prop = modelType.GetProperty(key);
                if (prop != null)
                {
                    var propType = prop.PropertyType;
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
                    {
                        column.Visible = false;
                        continue;
                    }
                    if (!propType.IsValueType && propType != typeof(string))
                    {
                        // Навигационные ссылки (Client, Employee, Zone, ...)
                        column.Visible = false;
                        continue;
                    }
                }

                // Заголовок
                if (headers.TryGetValue(key, out var headerText))
                {
                    column.HeaderText = headerText;
                    column.HeaderCell.ToolTipText = headerText;
                }
                else
                {
                    // Убираем добавленные/вычисляемые столбцы, для которых нет русской локализации
                    column.Visible = false;
                    continue;
                }

                // Форматы
                if (dateColumns.Contains(key))
                {
                    column.DefaultCellStyle.Format = "dd.MM.yyyy";
                }
                else if (dateTimeColumns.Contains(key))
                {
                    column.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                }
                else if (moneyColumns.Contains(key))
                {
                    column.DefaultCellStyle.Format = "#,0.## ₽";
                    column.DefaultCellStyle.NullValue = "";
                }

                // Выравнивание чисел по правому краю
                if (column.ValueType == typeof(int) || column.ValueType == typeof(decimal) || column.ValueType == typeof(double))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.DefaultCellStyle.NullValue = "";

                    // Скрываем лишние нули для некоторых тех. полей
                    if (integerColumnsHideZeros.Contains(key))
                    {
                        column.DefaultCellStyle.Format = "#";
                    }
                }
            }

            // Приоритет ширины для текстовых полей
            SetPreferredWidth(grid, new[] { "FullName", "Name", "Description", "ZoneName", "EmployeeName", "ClientName", "InventoryName", "ServiceName", "TicketType" }, 180);
            SetPreferredWidth(grid, new[] { "Email", "Phone" }, 140);
            SetPreferredWidth(grid, new[] { "Status", "StatusDisplayName" }, 120);

            grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.Refresh();
        }

        private string LocalizeHeader(string header)
        {
            // Простая локализация общих английских заголовков на русский, если вдруг встретятся
            return header switch
            {
                "Id" => "ID",
                "Full Name" => "ФИО",
                "Phone" => "Телефон",
                "Email" => "Email",
                "Birth Date" => "Дата рождения",
                "Registration Date" => "Дата регистрации",
                "Position" => "Должность",
                "Hire Date" => "Дата приема",
                "Ticket Type" => "Тип билета",
                "Price" => "Цена",
                "Purchase Date" => "Дата покупки",
                "Valid Until" => "Действует до",
                "Name" => "Название",
                "Description" => "Описание",
                "Amount" => "Сумма",
                "Payment Date" => "Дата оплаты",
                "Payment Method" => "Способ оплаты",
                "Zone Name" => "Зона",
                "Capacity" => "Вместимость",
                "Entry Time" => "Вход",
                "Exit Time" => "Выход",
                "Quantity" => "Количество",
                "Status" => "Статус",
                "Employee Name" => "Сотрудник",
                "Client Name" => "Клиент",
                "Service Name" => "Услуга",
                "Inventory Name" => "Инвентарь",
                "Work Date" => "Дата",
                "Shift Start" => "Начало смены",
                "Shift End" => "Конец смены",
                _ => header
            };
        }

        private Dictionary<string, string> GetHeaderMapForType(Type modelType)
        {
            // Базовый набор
            var map = new Dictionary<string, string>
            {
                // Общие
                { "ClientId", "ID клиента" },
                { "TicketId", "ID билета" },
                { "ServiceId", "ID услуги" },
                { "ZoneId", "ID зоны" },
                { "EmployeeId", "ID сотрудника" },
                { "PaymentId", "ID оплаты" },
                { "InventoryId", "ID инвентаря" },
                { "RentalId", "ID аренды" },
                { "ScheduleId", "ID смены" },

                { "FullName", "ФИО" },
                { "Phone", "Телефон" },
                { "Email", "Email" },
                { "BirthDate", "Дата рождения" },
                { "RegistrationDate", "Дата регистрации" },
                { "Position", "Должность" },
                { "HireDate", "Дата приема" },

                { "TicketType", "Тип билета" },
                { "Price", "Цена" },
                { "PurchaseDate", "Дата покупки" },
                { "ValidUntil", "Действует до" },

                { "Name", "Название" },
                { "Description", "Описание" },
                { "Amount", "Сумма" },
                { "PaymentDate", "Дата оплаты" },
                { "PaymentMethod", "Способ оплаты" },

                { "ZoneName", "Зона" },
                { "Capacity", "Вместимость" },

                { "EntryTime", "Вход" },
                { "ExitTime", "Выход" },

                { "Quantity", "Количество" },
                { "Status", "Статус" },
                { "ResponsibleEmployeeId", "Ответственный (ID)" },

                { "EmployeeName", "Сотрудник" },
                { "ClientName", "Клиент" },
                { "ServiceName", "Услуга" },
                { "InventoryName", "Инвентарь" },

                { "WorkDate", "Дата" },
                { "ShiftStart", "Начало смены" },
                { "ShiftEnd", "Конец смены" }
            };

            return map;
        }

        private void SetPreferredWidth(DataGridView grid, IEnumerable<string> columnNames, int width)
        {
            foreach (var name in columnNames)
            {
                var col = grid.Columns.Cast<DataGridViewColumn>().FirstOrDefault(c => c.DataPropertyName == name && c.Visible);
                if (col != null)
                {
                    col.Width = width;
                }
            }
        }

        private string SplitPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var chars = input.SelectMany((ch, i) => i > 0 && char.IsUpper(ch) && (i + 1 < input.Length ? char.IsLower(input[i + 1]) : true) ? new[] { ' ', ch } : new[] { ch }).ToArray();
            return new string(chars);
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
