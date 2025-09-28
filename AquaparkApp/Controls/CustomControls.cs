using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AquaparkApp.Models;

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
            Font = new Font("SF Pro Text", 14F, FontStyle.Regular);
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
            // Очищаем фон цветом родителя, чтобы не было белых углов на цветной панели
            var bgColor = Parent?.BackColor ?? BackColor;
            g.Clear(bgColor);

            Rectangle rect = new Rectangle(0, 0, Width, Height);
            GraphicsPath path = GetRoundedRectangle(rect, _cornerRadius);
            // Ограничиваем область кнопки скругленной формой, чтобы фон по углам не был виден
            this.Region = new Region(path);

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

            // Рисуем иконку, если есть
            int textOffset = 0;
            if (Image != null)
            {
                int imageX = Padding.Left;
                int imageY = (Height - Image.Height) / 2;
                g.DrawImage(Image, imageX, imageY);
                textOffset = Image.Width + 8; // 8px spacing
            }

            // Рисуем текст
            Rectangle textRect = new Rectangle(Padding.Left + textOffset, 0, Width - Padding.Left - Padding.Right - textOffset, Height);
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            };

            g.DrawString(Text, Font, new SolidBrush(ForeColor), textRect, sf);
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
            BackColor = Color.White;
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

}
