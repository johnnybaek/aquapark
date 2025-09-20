using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;
using AquaparkApp.BLL;

namespace AquaparkApp.Forms
{
    public partial class LoginForm : Form
    {
        private AuthenticationService _authService;
        private MacOSTextBox _usernameTextBox;
        private MacOSTextBox _passwordTextBox;
        private MacOSButton _loginButton;
        private MacOSButton _cancelButton;
        private Label _titleLabel;
        private Label _errorLabel;
        private Panel _mainPanel;
        private GlassPanel _glassPanel;

        public User? CurrentUser { get; private set; }

        public LoginForm(AuthenticationService authService)
        {
            _authService = authService;
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Вход в систему";
            this.Size = new Size(450, 600);
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
                Size = new Size(400, 500),
                Location = new Point(25, 50),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = "🌊 Вход в систему",
                Font = new Font("SF Pro Display", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 30),
                Size = new Size(300, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Поле имени пользователя
            var usernameLabel = new Label
            {
                Text = "Имя пользователя:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, 100),
                Size = new Size(150, 25)
            };

            _usernameTextBox = new MacOSTextBox
            {
                Location = new Point(50, 130),
                Size = new Size(300, 40),
                PlaceholderText = "Введите имя пользователя"
            };

            // Поле пароля
            var passwordLabel = new Label
            {
                Text = "Пароль:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, 190),
                Size = new Size(150, 25)
            };

            _passwordTextBox = new MacOSTextBox
            {
                Location = new Point(50, 220),
                Size = new Size(300, 40),
                UseSystemPasswordChar = true,
                PlaceholderText = "Введите пароль"
            };

            // Кнопки
            _loginButton = new MacOSButton
            {
                Text = "Войти",
                Location = new Point(50, 300),
                Size = new Size(140, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            _loginButton.Click += LoginButton_Click;

            _cancelButton = new MacOSButton
            {
                Text = "Отмена",
                Location = new Point(210, 300),
                Size = new Size(140, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Regular)
            };
            _cancelButton.Click += CancelButton_Click;

            // Метка ошибки
            _errorLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(255, 59, 48),
                Location = new Point(50, 370),
                Size = new Size(300, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Ссылка на регистрацию
            var registerLink = new LinkLabel
            {
                Text = "Нет аккаунта? Зарегистрироваться",
                Font = new Font("SF Pro Display", 11F, FontStyle.Underline),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 450),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            registerLink.Click += RegisterLink_Click;

            // Добавляем контролы
            _glassPanel.Controls.AddRange(new Control[] 
            {
                _titleLabel, usernameLabel, _usernameTextBox, passwordLabel, _passwordTextBox,
                _loginButton, _cancelButton, _errorLabel, registerLink
            });

            _mainPanel.Controls.Add(_glassPanel);
            this.Controls.Add(_mainPanel);

            // Обработчики событий
            _usernameTextBox.KeyPress += TextBox_KeyPress;
            _passwordTextBox.KeyPress += TextBox_KeyPress;
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_usernameTextBox.Text) || string.IsNullOrWhiteSpace(_passwordTextBox.Text))
            {
                ShowError("Пожалуйста, заполните все поля");
                return;
            }

            try
            {
                _loginButton.Enabled = false;
                _loginButton.Text = "Вход...";

                var user = await _authService.AuthenticateAsync(_usernameTextBox.Text, _passwordTextBox.Text);
                
                if (user != null)
                {
                    CurrentUser = user;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError("Неверное имя пользователя или пароль");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка входа: {ex.Message}");
            }
            finally
            {
                _loginButton.Enabled = true;
                _loginButton.Text = "Войти";
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RegisterLink_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm(_authService);
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                CurrentUser = registerForm.CurrentUser;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LoginButton_Click(sender, e);
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
