using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;
using AquaparkApp.BLL;

namespace AquaparkApp.Forms
{
    public partial class BookingForm : Form
    {
        private Attraction _attraction;
        private User _user;
        private TicketService _ticketService;
        private MacOSTextBox _quantityTextBox;
        private DateTimePicker _visitDatePicker;
        private ComboBox _visitTimeComboBox;
        private MacOSTextBox _notesTextBox;
        private MacOSButton _bookButton;
        private MacOSButton _cancelButton;
        private Label _titleLabel;
        private Label _attractionInfoLabel;
        private Label _priceLabel;
        private Label _totalPriceLabel;
        private Label _errorLabel;
        private Panel _mainPanel;
        private GlassPanel _glassPanel;
        private PictureBox _attractionImage;

        public BookingForm(Attraction attraction, User user, TicketService ticketService)
        {
            _attraction = attraction;
            _user = user;
            _ticketService = ticketService;
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Бронирование билета";
            this.Size = new Size(600, 700);
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
                Size = new Size(550, 650),
                Location = new Point(25, 25),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = "🎫 Бронирование билета",
                Font = new Font("SF Pro Display", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 20),
                Size = new Size(450, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Изображение аттракциона
            _attractionImage = new PictureBox
            {
                Location = new Point(50, 80),
                Size = new Size(200, 150),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Информация об аттракционе
            _attractionInfoLabel = new Label
            {
                Text = $"{_attraction.Name}\n\n{_attraction.Description}\n\n" +
                      $"Возраст: {_attraction.MinAge}-{_attraction.MaxAge} лет\n" +
                      $"Рост: {_attraction.MinHeight}-{_attraction.MaxHeight} см\n" +
                      $"Продолжительность: {_attraction.Duration} мин\n" +
                      $"Сложность: {_attraction.DifficultyLevel}",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(270, 80),
                Size = new Size(250, 150),
                TextAlign = ContentAlignment.TopLeft
            };

            // Цена
            _priceLabel = new Label
            {
                Text = $"Цена за билет: ₽ {_attraction.Price:N0}",
                Font = new Font("SF Pro Display", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 250),
                Size = new Size(200, 30)
            };

            // Количество
            var quantityLabel = new Label
            {
                Text = "Количество билетов:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, 300),
                Size = new Size(150, 25)
            };

            _quantityTextBox = new MacOSTextBox
            {
                Location = new Point(50, 330),
                Size = new Size(100, 40),
                Text = "1"
            };
            _quantityTextBox.TextChanged += QuantityTextBox_TextChanged;

            // Дата посещения
            var visitDateLabel = new Label
            {
                Text = "Дата посещения:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(200, 300),
                Size = new Size(150, 25)
            };

            _visitDatePicker = new DateTimePicker
            {
                Location = new Point(200, 330),
                Size = new Size(150, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today,
                MaxDate = DateTime.Today.AddDays(30)
            };

            // Время посещения
            var visitTimeLabel = new Label
            {
                Text = "Время посещения:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, 390),
                Size = new Size(150, 25)
            };

            _visitTimeComboBox = new ComboBox
            {
                Location = new Point(50, 420),
                Size = new Size(150, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Заполняем время посещения
            for (int hour = 9; hour <= 21; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30)
                {
                    _visitTimeComboBox.Items.Add($"{hour:D2}:{minute:D2}");
                }
            }
            _visitTimeComboBox.SelectedIndex = 0;

            // Примечания
            var notesLabel = new Label
            {
                Text = "Примечания (необязательно):",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, 480),
                Size = new Size(200, 25)
            };

            _notesTextBox = new MacOSTextBox
            {
                Location = new Point(50, 510),
                Size = new Size(450, 40),
                PlaceholderText = "Дополнительная информация..."
            };

            // Общая стоимость
            _totalPriceLabel = new Label
            {
                Text = $"Общая стоимость: ₽ {_attraction.Price:N0}",
                Font = new Font("SF Pro Display", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 570),
                Size = new Size(300, 30)
            };

            // Кнопки
            _bookButton = new MacOSButton
            {
                Text = "Забронировать",
                Location = new Point(50, 610),
                Size = new Size(150, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold)
            };
            _bookButton.Click += BookButton_Click;

            _cancelButton = new MacOSButton
            {
                Text = "Отмена",
                Location = new Point(220, 610),
                Size = new Size(150, 45),
                Font = new Font("SF Pro Display", 14F, FontStyle.Regular)
            };
            _cancelButton.Click += CancelButton_Click;

            // Метка ошибки
            _errorLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(255, 59, 48),
                Location = new Point(50, 570),
                Size = new Size(450, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            // Добавляем контролы
            _glassPanel.Controls.AddRange(new Control[] 
            {
                _titleLabel, _attractionImage, _attractionInfoLabel, _priceLabel,
                quantityLabel, _quantityTextBox, visitDateLabel, _visitDatePicker,
                visitTimeLabel, _visitTimeComboBox, notesLabel, _notesTextBox,
                _totalPriceLabel, _bookButton, _cancelButton, _errorLabel
            });

            _mainPanel.Controls.Add(_glassPanel);
            this.Controls.Add(_mainPanel);

            // Загружаем изображение аттракциона
            LoadAttractionImage();
        }

        private void LoadAttractionImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(_attraction.ImagePath) && System.IO.File.Exists(_attraction.ImagePath))
                {
                    _attractionImage.Image = Image.FromFile(_attraction.ImagePath);
                }
                else
                {
                    // Создаем заглушку
                    var placeholder = new Bitmap(200, 150);
                    using (var g = Graphics.FromImage(placeholder))
                    {
                        g.Clear(Color.FromArgb(248, 248, 248));
                        g.DrawString("🌊", new Font("Arial", 48), Brushes.LightBlue, 75, 50);
                    }
                    _attractionImage.Image = placeholder;
                }
            }
            catch
            {
                // В случае ошибки показываем заглушку
                var placeholder = new Bitmap(200, 150);
                using (var g = Graphics.FromImage(placeholder))
                {
                    g.Clear(Color.FromArgb(248, 248, 248));
                    g.DrawString("🌊", new Font("Arial", 48), Brushes.LightBlue, 75, 50);
                }
                _attractionImage.Image = placeholder;
            }
        }

        private void QuantityTextBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(_quantityTextBox.Text, out int quantity) && quantity > 0)
            {
                var totalPrice = _attraction.Price * quantity;
                _totalPriceLabel.Text = $"Общая стоимость: ₽ {totalPrice:N0}";
            }
        }

        private async void BookButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                _bookButton.Enabled = false;
                _bookButton.Text = "Бронирование...";

                var quantity = int.Parse(_quantityTextBox.Text);
                var visitDate = _visitDatePicker.Value.Date;
                var visitTime = TimeSpan.Parse(_visitTimeComboBox.SelectedItem.ToString());

                var ticket = new Ticket
                {
                    UserId = _user.Id,
                    AttractionId = _attraction.Id,
                    VisitDate = visitDate,
                    VisitTime = visitTime,
                    Price = _attraction.Price,
                    Quantity = quantity,
                    TotalPrice = _attraction.Price * quantity,
                    Status = "Pending",
                    Notes = _notesTextBox.Text.Trim()
                };

                var result = await _ticketService.CreateTicketAsync(ticket);
                
                if (result)
                {
                    MessageBox.Show("Билет успешно забронирован!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError("Ошибка бронирования. Попробуйте еще раз.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка бронирования: {ex.Message}");
            }
            finally
            {
                _bookButton.Enabled = true;
                _bookButton.Text = "Забронировать";
            }
        }

        private bool ValidateInput()
        {
            if (!int.TryParse(_quantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                ShowError("Введите корректное количество билетов");
                return false;
            }

            if (quantity > 10)
            {
                ShowError("Максимальное количество билетов: 10");
                return false;
            }

            if (_visitDatePicker.Value < DateTime.Today)
            {
                ShowError("Дата посещения не может быть в прошлом");
                return false;
            }

            if (_visitTimeComboBox.SelectedItem == null)
            {
                ShowError("Выберите время посещения");
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
