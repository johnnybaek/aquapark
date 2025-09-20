using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;
using AquaparkApp.BLL;

namespace AquaparkApp.Forms
{
    public partial class RegisterForm : Form
    {
        private AuthenticationService _authService;
        private MacOSTextBox _usernameTextBox;
        private MacOSTextBox _emailTextBox;
        private MacOSTextBox _passwordTextBox;
        private MacOSTextBox _confirmPasswordTextBox;
        private MacOSTextBox _firstNameTextBox;
        private MacOSTextBox _lastNameTextBox;
        private MacOSTextBox _phoneTextBox;
        private DateTimePicker _birthDatePicker;
        private ComboBox _genderComboBox;
        private MacOSButton _registerButton;
        private MacOSButton _cancelButton;
        private Label _titleLabel;
        private Label _errorLabel;
        private Panel _mainPanel;
        private GlassPanel _glassPanel;
        private ScrollableControl _scrollPanel;

        public User? CurrentUser { get; private set; }

        public RegisterForm(AuthenticationService authService)
        {
            _authService = authService;
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Регистрация";
            this.Size = new Size(500, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.None;
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

            // Создаем прокручиваемую панель
            _scrollPanel = new ScrollableControl
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            // Создаем стеклянную панель
            _glassPanel = new GlassPanel
            {
                Size = new Size(450, 650),
                Location = new Point(25, 25),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = "🌊 Регистрация",
                Font = new Font("SF Pro Display", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 20),
                Size = new Size(350, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            int y = 80;

            // Имя пользователя
            var usernameLabel = CreateLabel("Имя пользователя:", 50, y);
            _usernameTextBox = CreateTextBox(50, y + 25, "Введите имя пользователя");
            y += 70;

            // Email
            var emailLabel = CreateLabel("Email:", 50, y);
            _emailTextBox = CreateTextBox(50, y + 25, "Введите email");
            y += 70;

            // Пароль
            var passwordLabel = CreateLabel("Пароль:", 50, y);
            _passwordTextBox = CreateTextBox(50, y + 25, "Введите пароль");
            _passwordTextBox.UseSystemPasswordChar = true;
            y += 70;

            // Подтверждение пароля
            var confirmPasswordLabel = CreateLabel("Подтвердите пароль:", 50, y);
            _confirmPasswordTextBox = CreateTextBox(50, y + 25, "Подтвердите пароль");
            _confirmPasswordTextBox.UseSystemPasswordChar = true;
            y += 70;

            // Имя
            var firstNameLabel = CreateLabel("Имя:", 50, y);
            _firstNameTextBox = CreateTextBox(50, y + 25, "Введите имя");
            y += 70;

            // Фамилия
            var lastNameLabel = CreateLabel("Фамилия:", 50, y);
            _lastNameTextBox = CreateTextBox(50, y + 25, "Введите фамилию");
            y += 70;

            // Телефон
            var phoneLabel = CreateLabel("Телефон:", 50, y);
            _phoneTextBox = CreateTextBox(50, y + 25, "Введите телефон");
            y += 70;

            // Дата рождения
            var birthDateLabel = CreateLabel("Дата рождения:", 50, y);
            _birthDatePicker = new DateTimePicker
            {
                Location = new Point(50, y + 25),
                Size = new Size(350, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddYears(-25)
            };
            y += 70;

            // Пол
            var genderLabel = CreateLabel("Пол:", 50, y);
            _genderComboBox = new ComboBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(350, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _genderComboBox.Items.AddRange(new[] { "Мужской", "Женский" });
            _genderComboBox.SelectedIndex = 0;
            y += 70;

            // Кнопки
            _registerButton = new MacOSButton
            {
                Text = "Зарегистрироваться",
                Location = new Point(50, y + 20),
                Size = new Size(170, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            _registerButton.Click += RegisterButton_Click;

            _cancelButton = new MacOSButton
            {
                Text = "Отмена",
                Location = new Point(230, y + 20),
                Size = new Size(170, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Regular)
            };
            _cancelButton.Click += CancelButton_Click;

            // Метка ошибки
            _errorLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(255, 59, 48),
                Location = new Point(50, y + 80),
                Size = new Size(350, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Ссылка на вход
            var loginLink = new LinkLabel
            {
                Text = "Уже есть аккаунт? Войти",
                Font = new Font("SF Pro Display", 11F, FontStyle.Underline),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, y + 150),
                Size = new Size(350, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            loginLink.Click += LoginLink_Click;

            // Добавляем контролы
            _glassPanel.Controls.AddRange(new Control[] 
            {
                _titleLabel, usernameLabel, _usernameTextBox, emailLabel, _emailTextBox,
                passwordLabel, _passwordTextBox, confirmPasswordLabel, _confirmPasswordTextBox,
                firstNameLabel, _firstNameTextBox, lastNameLabel, _lastNameTextBox,
                phoneLabel, _phoneTextBox, birthDateLabel, _birthDatePicker,
                genderLabel, _genderComboBox, _registerButton, _cancelButton,
                _errorLabel, loginLink
            });

            _scrollPanel.Controls.Add(_glassPanel);
            _mainPanel.Controls.Add(_scrollPanel);
            this.Controls.Add(_mainPanel);
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
                Size = new Size(350, 40),
                PlaceholderText = placeholder
            };
        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                _registerButton.Enabled = false;
                _registerButton.Text = "Регистрация...";

                var user = new User
                {
                    Username = _usernameTextBox.Text.Trim(),
                    Email = _emailTextBox.Text.Trim(),
                    FirstName = _firstNameTextBox.Text.Trim(),
                    LastName = _lastNameTextBox.Text.Trim(),
                    Phone = _phoneTextBox.Text.Trim(),
                    DateOfBirth = _birthDatePicker.Value,
                    Gender = _genderComboBox.SelectedItem.ToString(),
                    IsAdmin = false,
                    IsActive = true
                };

                var registeredUser = await _authService.RegisterAsync(user, _passwordTextBox.Text);
                
                if (registeredUser != null)
                {
                    CurrentUser = registeredUser;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError("Ошибка регистрации. Попробуйте еще раз.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка регистрации: {ex.Message}");
            }
            finally
            {
                _registerButton.Enabled = true;
                _registerButton.Text = "Зарегистрироваться";
            }
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

            if (string.IsNullOrWhiteSpace(_passwordTextBox.Text) || _passwordTextBox.Text.Length < 6)
            {
                ShowError("Пароль должен содержать минимум 6 символов");
                return false;
            }

            if (_passwordTextBox.Text != _confirmPasswordTextBox.Text)
            {
                ShowError("Пароли не совпадают");
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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LoginLink_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm(_authService);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                CurrentUser = loginForm.CurrentUser;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ShowError(string message)
        {
            _errorLabel.Text = message;
            _errorLabel.Visible = true;
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
