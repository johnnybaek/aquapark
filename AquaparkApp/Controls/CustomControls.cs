using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AquaparkApp.Controls
{
    /// <summary>
    /// Кастомная кнопка в стиле macOS Tahoe
    /// </summary>
    public class MacOSButton : Button
    {
        private bool _isHovered = false;
        private bool _isPressed = false;
        private Color _gradientStart = Color.FromArgb(0, 122, 255);
        private Color _gradientEnd = Color.FromArgb(0, 100, 200);
        private Color _hoverStart = Color.FromArgb(0, 140, 255);
        private Color _hoverEnd = Color.FromArgb(0, 120, 220);
        private int _cornerRadius = 12;

        public MacOSButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = new Font("SF Pro Display", 14F, FontStyle.Regular);
            Cursor = Cursors.Hand;
            Size = new Size(120, 40);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isPressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isPressed = false;
            Invalidate();
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

            // Рисуем тень
            if (!_isPressed)
            {
                Rectangle shadowRect = new Rectangle(0, 2, Width - 1, Height - 1);
                GraphicsPath shadowPath = GetRoundedRectangle(shadowRect, _cornerRadius);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
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

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Кастомное текстовое поле в стиле macOS
    /// </summary>
    public class MacOSTextBox : TextBox
    {
        private Color _borderColor = Color.FromArgb(200, 200, 200);
        private Color _focusColor = Color.FromArgb(0, 122, 255);
        private int _borderRadius = 8;
        private bool _isFocused = false;

        public MacOSTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            BorderStyle = BorderStyle.None;
            BackColor = Color.FromArgb(248, 248, 248);
            ForeColor = Color.FromArgb(51, 51, 51);
            Font = new Font("SF Pro Text", 12F, FontStyle.Regular);
            Padding = new Padding(12, 8, 12, 8);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            _isFocused = true;
            Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _isFocused = false;
            Invalidate();
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
            using (Pen pen = new Pen(borderColor, _isFocused ? 2 : 1))
            {
                g.DrawPath(pen, path);
            }

            // Рисуем текст
            Rectangle textRect = new Rectangle(Padding.Left, Padding.Top, 
                Width - Padding.Left - Padding.Right, Height - Padding.Top - Padding.Bottom);
            
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, 
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Кастомная панель с эффектом стекла
    /// </summary>
    public class GlassPanel : Panel
    {
        private int _blurRadius = 20;
        private Color _glassColor = Color.FromArgb(100, 255, 255, 255);

        public GlassPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисуем эффект стекла
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            GraphicsPath path = GetRoundedRectangle(rect, 15);

            // Градиент для эффекта стекла
            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect, 
                Color.FromArgb(150, 255, 255, 255), 
                Color.FromArgb(50, 255, 255, 255), 
                LinearGradientMode.Vertical))
            {
                g.FillPath(brush, path);
            }

            // Рисуем границу
            using (Pen pen = new Pen(Color.FromArgb(100, 255, 255, 255), 1))
            {
                g.DrawPath(pen, path);
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Кастомная карточка для отображения аттракционов
    /// </summary>
    public class AttractionCard : Panel
    {
        private Attraction _attraction;
        private bool _isHovered = false;
        private PictureBox _imageBox;
        private Label _nameLabel;
        private Label _priceLabel;
        private Label _descriptionLabel;
        private MacOSButton _bookButton;

        public event EventHandler<Attraction> BookClicked;

        public Attraction Attraction
        {
            get => _attraction;
            set
            {
                _attraction = value;
                UpdateCard();
            }
        }

        public AttractionCard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            BackColor = Color.White;
            Size = new Size(300, 400);
            Cursor = Cursors.Hand;
        }

        private void InitializeComponent()
        {
            _imageBox = new PictureBox
            {
                Size = new Size(280, 180),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 248, 248)
            };

            _nameLabel = new Label
            {
                Location = new Point(15, 200),
                Size = new Size(270, 30),
                Font = new Font("SF Pro Display", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Text = "Название аттракциона"
            };

            _descriptionLabel = new Label
            {
                Location = new Point(15, 235),
                Size = new Size(270, 60),
                Font = new Font("SF Pro Text", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(102, 102, 102),
                Text = "Описание аттракциона"
            };

            _priceLabel = new Label
            {
                Location = new Point(15, 300),
                Size = new Size(120, 30),
                Font = new Font("SF Pro Display", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Text = "₽ 0"
            };

            _bookButton = new MacOSButton
            {
                Location = new Point(150, 300),
                Size = new Size(120, 35),
                Text = "Забронировать",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _bookButton.Click += (s, e) => BookClicked?.Invoke(this, _attraction);

            Controls.AddRange(new Control[] { _imageBox, _nameLabel, _descriptionLabel, _priceLabel, _bookButton });
        }

        private void UpdateCard()
        {
            if (_attraction == null) return;

            _nameLabel.Text = _attraction.Name;
            _descriptionLabel.Text = _attraction.Description.Length > 80 ? 
                _attraction.Description.Substring(0, 80) + "..." : _attraction.Description;
            _priceLabel.Text = $"₽ {_attraction.Price:N0}";

            // Загружаем изображение если есть
            if (!string.IsNullOrEmpty(_attraction.ImagePath) && File.Exists(_attraction.ImagePath))
            {
                _imageBox.Image = Image.FromFile(_attraction.ImagePath);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath path = GetRoundedRectangle(rect, 15);

            // Рисуем тень
            if (_isHovered)
            {
                Rectangle shadowRect = new Rectangle(2, 4, Width - 1, Height - 1);
                GraphicsPath shadowPath = GetRoundedRectangle(shadowRect, 15);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
                {
                    g.FillPath(shadowBrush, shadowPath);
                }
            }

            // Рисуем фон карточки
            g.FillPath(new SolidBrush(BackColor), path);

            // Рисуем границу
            using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                g.DrawPath(pen, path);
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
