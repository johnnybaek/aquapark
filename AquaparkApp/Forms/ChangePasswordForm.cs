using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;
using AquaparkApp.BLL;

namespace AquaparkApp.Forms
{
    public partial class ChangePasswordForm : Form
    {
        private User _user;
        private AuthenticationService _authService;
        private MacOSTextBox _currentPasswordTextBox;
        private MacOSTextBox _newPasswordTextBox;
        private MacOSTextBox _confirmPasswordTextBox;
        private MacOSButton _changeButton;
        private MacOSButton _cancelButton;
        private Label _titleLabel;
        private Label _errorLabel;
        private Panel _mainPanel;
        private GlassPanel _glassPanel;

        public ChangePasswordForm(User user, AuthenticationService authService)
        {
            _user = user;
            _authService = authService;
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Смена пароля";
            this.Size = new Size(450, 500);
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
                Size = new Size(400, 400),
                Location = new Point(25, 50),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = "🔐 Смена пароля",
                Font = new Font("SF Pro Display", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 30),
                Size = new Size(300, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            int y = 100;

            // Текущий пароль
            var currentPasswordLabel = new Label
            {
                Text = "Текущий пароль:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, y),
                Size = new Size(150, 25)
            };

            _currentPasswordTextBox = new MacOSTextBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(300, 40),
                UseSystemPasswordChar = true,
                PlaceholderText = "Введите текущий пароль"
            };
            y += 80;

            // Новый пароль
            var newPasswordLabel = new Label
            {
                Text = "Новый пароль:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, y),
                Size = new Size(150, 25)
            };

            _newPasswordTextBox = new MacOSTextBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(300, 40),
                UseSystemPasswordChar = true,
                PlaceholderText = "Введите новый пароль"
            };
            y += 80;

            // Подтверждение пароля
            var confirmPasswordLabel = new Label
            {
                Text = "Подтвердите пароль:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, y),
                Size = new Size(150, 25)
            };

            _confirmPasswordTextBox = new MacOSTextBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(300, 40),
                UseSystemPasswordChar = true,
                PlaceholderText = "Подтвердите новый пароль"
            };
            y += 80;

            // Кнопки
            _changeButton = new MacOSButton
            {
                Text = "Изменить пароль",
                Location = new Point(50, y),
                Size = new Size(150, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            _changeButton.Click += ChangeButton_Click;

            _cancelButton = new MacOSButton
            {
                Text = "Отмена",
                Location = new Point(220, y),
                Size = new Size(130, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Regular)
            };
            _cancelButton.Click += CancelButton_Click;

            // Метка ошибки
            _errorLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(255, 59, 48),
                Location = new Point(50, y + 60),
                Size = new Size(300, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Добавляем контролы
            _glassPanel.Controls.AddRange(new Control[] 
            {
                _titleLabel, currentPasswordLabel, _currentPasswordTextBox,
                newPasswordLabel, _newPasswordTextBox, confirmPasswordLabel, _confirmPasswordTextBox,
                _changeButton, _cancelButton, _errorLabel
            });

            _mainPanel.Controls.Add(_glassPanel);
            this.Controls.Add(_mainPanel);

            // Обработчики событий
            _currentPasswordTextBox.KeyPress += TextBox_KeyPress;
            _newPasswordTextBox.KeyPress += TextBox_KeyPress;
            _confirmPasswordTextBox.KeyPress += TextBox_KeyPress;
        }

        private async void ChangeButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                _changeButton.Enabled = false;
                _changeButton.Text = "Изменение...";

                var result = await _authService.ChangePasswordAsync(
                    _user.Id, 
                    _currentPasswordTextBox.Text, 
                    _newPasswordTextBox.Text);

                if (result)
                {
                    MessageBox.Show("Пароль успешно изменен!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError("Неверный текущий пароль или ошибка изменения");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка изменения пароля: {ex.Message}");
            }
            finally
            {
                _changeButton.Enabled = true;
                _changeButton.Text = "Изменить пароль";
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ChangeButton_Click(sender, e);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(_currentPasswordTextBox.Text))
            {
                ShowError("Введите текущий пароль");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_newPasswordTextBox.Text) || _newPasswordTextBox.Text.Length < 6)
            {
                ShowError("Новый пароль должен содержать минимум 6 символов");
                return false;
            }

            if (_newPasswordTextBox.Text != _confirmPasswordTextBox.Text)
            {
                ShowError("Пароли не совпадают");
                return false;
            }

            if (_currentPasswordTextBox.Text == _newPasswordTextBox.Text)
            {
                ShowError("Новый пароль должен отличаться от текущего");
                return false;
            }

            return true;
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
