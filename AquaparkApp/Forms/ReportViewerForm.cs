using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.Models;

namespace AquaparkApp.Forms
{
    public partial class ReportViewerForm : Form
    {
        private string _reportTitle;
        private object _reportData;
        private Panel _mainPanel;
        private GlassPanel _glassPanel;
        private Label _titleLabel;
        private DataGridView _dataGrid;
        private MacOSButton _exportButton;
        private MacOSButton _closeButton;
        private Label _summaryLabel;

        public ReportViewerForm(string reportTitle, object reportData)
        {
            _reportTitle = reportTitle;
            _reportData = reportData;
            InitializeComponent();
            SetupUI();
            LoadReportData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = _reportTitle;
            this.Size = new Size(1000, 700);
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
                Size = new Size(950, 650),
                Location = new Point(25, 25),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = $"📊 {_reportTitle}",
                Font = new Font("SF Pro Display", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255),
                Location = new Point(50, 20),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Сводка отчета
            _summaryLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(50, 60),
                Size = new Size(850, 60),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // DataGridView для отображения данных
            _dataGrid = new DataGridView
            {
                Location = new Point(50, 130),
                Size = new Size(850, 400),
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
            _dataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
            _dataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            _dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);

            // Кнопки
            _exportButton = new MacOSButton
            {
                Text = "📊 Экспорт в Excel",
                Location = new Point(50, 550),
                Size = new Size(150, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _exportButton.Click += ExportButton_Click;

            _closeButton = new MacOSButton
            {
                Text = "❌ Закрыть",
                Location = new Point(750, 550),
                Size = new Size(150, 40),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _closeButton.Click += CloseButton_Click;

            // Добавляем контролы
            _glassPanel.Controls.AddRange(new Control[] 
            {
                _titleLabel, _summaryLabel, _dataGrid, _exportButton, _closeButton
            });

            _mainPanel.Controls.Add(_glassPanel);
            this.Controls.Add(_mainPanel);
        }

        private void LoadReportData()
        {
            try
            {
                switch (_reportData)
                {
                    case SalesReport salesReport:
                        LoadSalesReport(salesReport);
                        break;
                    case UserReport userReport:
                        LoadUserReport(userReport);
                        break;
                    case AttendanceReport attendanceReport:
                        LoadAttendanceReport(attendanceReport);
                        break;
                    default:
                        _summaryLabel.Text = "Неизвестный тип отчета";
                        break;
                }
            }
            catch (Exception ex)
            {
                _summaryLabel.Text = $"Ошибка загрузки отчета: {ex.Message}";
            }
        }

        private void LoadSalesReport(SalesReport report)
        {
            _summaryLabel.Text = $"Период: {report.StartDate:dd.MM.yyyy} - {report.EndDate:dd.MM.yyyy}\n" +
                               $"Общая выручка: ₽ {report.TotalRevenue:N0} | " +
                               $"Заказов: {report.TotalOrders} | " +
                               $"Билетов: {report.TotalTickets} | " +
                               $"Средний чек: ₽ {report.AverageOrderValue:N0}";

            _dataGrid.DataSource = report.DailySales.ToList();
        }

        private void LoadUserReport(UserReport report)
        {
            _summaryLabel.Text = $"Всего пользователей: {report.TotalUsers} | " +
                               $"Активных: {report.ActiveUsers} | " +
                               $"Новых за месяц: {report.NewUsersThisMonth} | " +
                               $"Средние траты: ₽ {report.AverageSpendingPerUser:N0}";

            _dataGrid.DataSource = report.TopSpenders.ToList();
        }

        private void LoadAttendanceReport(AttendanceReport report)
        {
            _summaryLabel.Text = $"Период: {report.StartDate:dd.MM.yyyy} - {report.EndDate:dd.MM.yyyy}\n" +
                               $"Всего посетителей: {report.TotalVisitors} | " +
                               $"Пиковый день: {report.PeakDay:dd.MM.yyyy} ({report.PeakDayVisitors} чел.)";

            _dataGrid.DataSource = report.HourlyStats.ToList();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                Title = "Экспорт отчета в Excel"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Здесь можно добавить логику экспорта
                    MessageBox.Show("Функция экспорта будет реализована", "Информация", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
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
