using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;
using AquaparkApp.BLL;

namespace AquaparkApp.Forms
{
    public partial class ProfileForm : Form
    {
        private User _user;
        private AuthenticationService _authService;
        private MacOSTextBox _usernameTextBox;
        private MacOSTextBox _emailTextBox;
        private MacOSTextBox _firstNameTextBox;
        private MacOSTextBox _lastNameTextBox;
        private MacOSTextBox _phoneTextBox;
        private DateTimePicker _birthDatePicker;
        private ComboBox _genderComboBox;
        private MacOSTextBox _addressTextBox;
        private MacOSButton _saveButton;
        private MacOSButton _cancelButton;
        private MacOSButton _changePasswordButton;
        private Label _titleLabel;
        private Label _errorLabel;
        private Label _successLabel;
        private Panel _mainPanel;
        private GlassPanel _glassPanel;
        private TabControl _profileTabs;
        private DataGridView _ordersDataGrid;
        private DataGridView _ticketsDataGrid;

        public ProfileForm(User user)
        {
            _user = user;
            _authService = new AuthenticationService("Host=localhost;Database=aquapark_db;Username=postgres;Password=password");
            InitializeComponent();
            SetupUI();
            LoadUserData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Профиль пользователя";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
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
                Size = new Size(750, 650),
                Location = new Point(25, 25),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = "👤 Профиль пользователя",
                Font = new Font("SF Pro Display", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 20),
                Size = new Size(650, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Создаем вкладки
            _profileTabs = new TabControl
            {
                Location = new Point(50, 80),
                Size = new Size(650, 500),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Appearance = TabAppearance.FlatButtons,
                ItemSize = new Size(120, 40),
                SizeMode = TabSizeMode.Fixed
            };

            // Стилизация вкладок
            _profileTabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            _profileTabs.DrawItem += TabControl_DrawItem;

            // Вкладка "Личная информация"
            var personalTab = new TabPage("👤 Личная информация");
            CreatePersonalInfoTab(personalTab);

            // Вкладка "Мои заказы"
            var ordersTab = new TabPage("📋 Мои заказы");
            CreateOrdersTab(ordersTab);

            // Вкладка "Мои билеты"
            var ticketsTab = new TabPage("🎫 Мои билеты");
            CreateTicketsTab(ticketsTab);

            _profileTabs.TabPages.AddRange(new TabPage[] { personalTab, ordersTab, ticketsTab });

            // Кнопки
            _saveButton = new MacOSButton
            {
                Text = "Сохранить",
                Location = new Point(50, 600),
                Size = new Size(120, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Bold)
            };
            _saveButton.Click += SaveButton_Click;

            _changePasswordButton = new MacOSButton
            {
                Text = "Изменить пароль",
                Location = new Point(180, 600),
                Size = new Size(150, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _changePasswordButton.Click += ChangePasswordButton_Click;

            _cancelButton = new MacOSButton
            {
                Text = "Закрыть",
                Location = new Point(580, 600),
                Size = new Size(120, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _cancelButton.Click += CancelButton_Click;

            // Метки сообщений
            _errorLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(255, 59, 48),
                Location = new Point(50, 650),
                Size = new Size(300, 30),
                Visible = false
            };

            _successLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 199, 89),
                Location = new Point(50, 650),
                Size = new Size(300, 30),
                Visible = false
            };

            // Добавляем контролы
            _glassPanel.Controls.AddRange(new Control[] 
            {
                _titleLabel, _profileTabs, _saveButton, _changePasswordButton, 
                _cancelButton, _errorLabel, _successLabel
            });

            _mainPanel.Controls.Add(_glassPanel);
            this.Controls.Add(_mainPanel);
        }

        private void CreatePersonalInfoTab(TabPage tab)
        {
            int y = 20;

            // Имя пользователя
            var usernameLabel = CreateLabel("Имя пользователя:", 20, y);
            _usernameTextBox = CreateTextBox(20, y + 25, "Введите имя пользователя");
            y += 70;

            // Email
            var emailLabel = CreateLabel("Email:", 20, y);
            _emailTextBox = CreateTextBox(20, y + 25, "Введите email");
            y += 70;

            // Имя
            var firstNameLabel = CreateLabel("Имя:", 20, y);
            _firstNameTextBox = CreateTextBox(20, y + 25, "Введите имя");
            y += 70;

            // Фамилия
            var lastNameLabel = CreateLabel("Фамилия:", 20, y);
            _lastNameTextBox = CreateTextBox(20, y + 25, "Введите фамилию");
            y += 70;

            // Телефон
            var phoneLabel = CreateLabel("Телефон:", 20, y);
            _phoneTextBox = CreateTextBox(20, y + 25, "Введите телефон");
            y += 70;

            // Дата рождения
            var birthDateLabel = CreateLabel("Дата рождения:", 20, y);
            _birthDatePicker = new DateTimePicker
            {
                Location = new Point(20, y + 25),
                Size = new Size(300, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Format = DateTimePickerFormat.Short
            };
            y += 70;

            // Пол
            var genderLabel = CreateLabel("Пол:", 20, y);
            _genderComboBox = new ComboBox
            {
                Location = new Point(20, y + 25),
                Size = new Size(300, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _genderComboBox.Items.AddRange(new[] { "Мужской", "Женский" });
            y += 70;

            // Адрес
            var addressLabel = CreateLabel("Адрес:", 20, y);
            _addressTextBox = CreateTextBox(20, y + 25, "Введите адрес");
            y += 70;

            // Статистика
            var statsLabel = new Label
            {
                Text = $"Всего заказов: {_user.TotalOrders}\nПотрачено: ₽ {_user.TotalSpent:N0}",
                Font = new Font("SF Pro Display", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(20, y),
                Size = new Size(300, 60)
            };

            tab.Controls.AddRange(new Control[] 
            {
                usernameLabel, _usernameTextBox, emailLabel, _emailTextBox,
                firstNameLabel, _firstNameTextBox, lastNameLabel, _lastNameTextBox,
                phoneLabel, _phoneTextBox, birthDateLabel, _birthDatePicker,
                genderLabel, _genderComboBox, addressLabel, _addressTextBox, statsLabel
            });
        }

        private void CreateOrdersTab(TabPage tab)
        {
            _ordersDataGrid = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(610, 400),
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
                Size = new Size(610, 400),
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
            _ticketsDataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
            _ticketsDataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _ticketsDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            _ticketsDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);

            tab.Controls.Add(_ticketsDataGrid);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(x, y),
                Size = new Size(200, 25)
            };
        }

        private MacOSTextBox CreateTextBox(int x, int y, string placeholder)
        {
            return new MacOSTextBox
            {
                Location = new Point(x, y),
                Size = new Size(300, 40),
                PlaceholderText = placeholder
            };
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

        private void LoadUserData()
        {
            _usernameTextBox.Text = _user.Username;
            _emailTextBox.Text = _user.Email;
            _firstNameTextBox.Text = _user.FirstName;
            _lastNameTextBox.Text = _user.LastName;
            _phoneTextBox.Text = _user.Phone;
            _birthDatePicker.Value = _user.DateOfBirth;
            _genderComboBox.SelectedItem = _user.Gender;
            _addressTextBox.Text = _user.Address;
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                _saveButton.Enabled = false;
                _saveButton.Text = "Сохранение...";

                // Обновляем данные пользователя
                _user.Username = _usernameTextBox.Text.Trim();
                _user.Email = _emailTextBox.Text.Trim();
                _user.FirstName = _firstNameTextBox.Text.Trim();
                _user.LastName = _lastNameTextBox.Text.Trim();
                _user.Phone = _phoneTextBox.Text.Trim();
                _user.DateOfBirth = _birthDatePicker.Value;
                _user.Gender = _genderComboBox.SelectedItem?.ToString() ?? "";
                _user.Address = _addressTextBox.Text.Trim();

                var result = await _authService.UpdateUserAsync(_user);
                
                if (result)
                {
                    ShowSuccess("Профиль успешно обновлен!");
                }
                else
                {
                    ShowError("Ошибка обновления профиля. Попробуйте еще раз.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка обновления: {ex.Message}");
            }
            finally
            {
                _saveButton.Enabled = true;
                _saveButton.Text = "Сохранить";
            }
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            var changePasswordForm = new ChangePasswordForm(_user, _authService);
            changePasswordForm.ShowDialog();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(_usernameTextBox.Text))
            {
                ShowError("Введите имя пользователя");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_emailTextBox.Text) || !_emailTextBox.Text.Contains("@"))
            {
                ShowError("Введите корректный email");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_firstNameTextBox.Text))
            {
                ShowError("Введите имя");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_lastNameTextBox.Text))
            {
                ShowError("Введите фамилию");
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            _errorLabel.Text = message;
            _errorLabel.Visible = true;
            _successLabel.Visible = false;
        }

        private void ShowSuccess(string message)
        {
            _successLabel.Text = message;
            _successLabel.Visible = true;
            _errorLabel.Visible = false;
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
