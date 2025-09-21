using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.DAL;
using AquaparkApp.Models;

namespace AquaparkApp.Forms
{
    public partial class InteractiveMapForm : Form
    {
        private Panel _mainPanel;
        private Panel _headerPanel;
        private Panel _mapPanel;
        private Panel _infoPanel;
        private Label _titleLabel;
        private Label _zoneInfoLabel;
        private ZoneRepository _zoneRepository;
        private EmployeeRepository _employeeRepository;
        private List<Zone> _zones;
        private List<Employee> _employees;
        private Zone _selectedZone;
        private Dictionary<Zone, Rectangle> _zoneRectangles;
        private Dictionary<Zone, Color> _zoneColors;

        public InteractiveMapForm()
        {
            InitializeComponent();
            InitializeRepositories();
            SetupUI();
            LoadData();
            DrawMap();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "🗺️ Интерактивная карта аквапарка";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(248, 248, 248);
            this.Font = new Font("SF Pro Display", 12F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1200, 800);
            
            this.ResumeLayout(false);
        }

        private void InitializeRepositories()
        {
            _zoneRepository = new ZoneRepository();
            _employeeRepository = new EmployeeRepository();
        }

        private void SetupUI()
        {
            // Главная панель
            _mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            // Панель заголовка
            CreateHeaderPanel();

            // Панель карты
            CreateMapPanel();

            // Панель информации
            CreateInfoPanel();

            _mainPanel.Controls.AddRange(new Control[] { _headerPanel, _mapPanel, _infoPanel });
            this.Controls.Add(_mainPanel);
        }

        private void CreateHeaderPanel()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 122, 255)
            };

            _titleLabel = new Label
            {
                Text = "🗺️ Интерактивная карта аквапарка \"Водный мир\"",
                Font = new Font("SF Pro Display", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            var legendLabel = new Label
            {
                Text = "Нажмите на зону для получения информации",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(30, 50),
                AutoSize = true
            };

            _headerPanel.Controls.AddRange(new Control[] { _titleLabel, legendLabel });
        }

        private void CreateMapPanel()
        {
            _mapPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 248, 255),
                Padding = new Padding(20)
            };

            _mapPanel.Paint += MapPanel_Paint;
            _mapPanel.MouseClick += MapPanel_MouseClick;
            _mapPanel.MouseMove += MapPanel_MouseMove;
        }

        private void CreateInfoPanel()
        {
            _infoPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 300,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(15)
            };

            _zoneInfoLabel = new Label
            {
                Text = "Выберите зону на карте",
                Font = new Font("SF Pro Display", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(15, 15),
                Size = new Size(270, 100),
                AutoSize = true
            };

            _infoPanel.Controls.Add(_zoneInfoLabel);
        }

        private async void LoadData()
        {
            try
            {
                _zones = (await _zoneRepository.GetAllAsync()).ToList();
                _employees = (await _employeeRepository.GetAllAsync()).ToList();
                
                InitializeZoneRectangles();
                InitializeZoneColors();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeZoneRectangles()
        {
            _zoneRectangles = new Dictionary<Zone, Rectangle>();
            
            // Размещаем зоны на карте в виде схемы аквапарка
            var zonePositions = new Dictionary<string, Rectangle>
            {
                // Входная зона
                { "Касса", new Rectangle(50, 50, 120, 80) },
                { "Раздевалки", new Rectangle(200, 50, 150, 100) },
                
                // Бассейны
                { "Детский бассейн", new Rectangle(50, 200, 150, 100) },
                { "Взрослый бассейн", new Rectangle(250, 200, 200, 120) },
                { "Волновой бассейн", new Rectangle(500, 200, 180, 100) },
                { "Ленивая река", new Rectangle(720, 200, 200, 80) },
                
                // Горки
                { "Горка \"Водопад\"", new Rectangle(50, 350, 120, 150) },
                { "Горка \"Радуга\"", new Rectangle(200, 350, 120, 120) },
                { "Экстремальная воронка", new Rectangle(350, 350, 120, 100) },
                
                // СПА зона
                { "Джакузи", new Rectangle(500, 350, 100, 80) },
                { "Сауна", new Rectangle(650, 350, 100, 80) },
                { "Баня", new Rectangle(800, 350, 100, 80) },
                
                // Развлечения
                { "Детская площадка", new Rectangle(50, 550, 150, 100) },
                { "Спортивная зона", new Rectangle(250, 550, 150, 100) },
                { "Зона отдыха", new Rectangle(450, 550, 200, 100) },
                
                // Питание
                { "Кафе \"Волна\"", new Rectangle(700, 550, 150, 80) },
                { "Бар \"Аква\"", new Rectangle(900, 550, 100, 80) },
                
                // Служебные зоны
                { "Администрация", new Rectangle(50, 700, 120, 60) },
                { "Медпункт", new Rectangle(200, 700, 100, 60) },
                { "Прокат инвентаря", new Rectangle(350, 700, 150, 60) },
                
                // Тематические зоны
                { "Храм Посейдона", new Rectangle(550, 700, 120, 60) },
                { "Тропический грот", new Rectangle(700, 700, 120, 60) },
                { "Детский лабиринт", new Rectangle(850, 700, 120, 60) },
                { "VIP зона", new Rectangle(1000, 700, 100, 60) }
            };

            foreach (var zone in _zones)
            {
                if (zonePositions.ContainsKey(zone.ZoneName))
                {
                    _zoneRectangles[zone] = zonePositions[zone.ZoneName];
                }
                else
                {
                    // Размещаем неопознанные зоны в случайных местах
                    var random = new Random(zone.ZoneId);
                    _zoneRectangles[zone] = new Rectangle(
                        random.Next(50, 800), random.Next(50, 600), 100, 60);
                }
            }
        }

        private void InitializeZoneColors()
        {
            _zoneColors = new Dictionary<Zone, Color>();
            var colors = new[]
            {
                Color.FromArgb(0, 122, 255),    // Синий - бассейны
                Color.FromArgb(255, 149, 0),    // Оранжевый - горки
                Color.FromArgb(255, 45, 85),    // Красный - СПА
                Color.FromArgb(52, 199, 89),    // Зеленый - развлечения
                Color.FromArgb(255, 204, 0),    // Желтый - питание
                Color.FromArgb(175, 82, 222),   // Фиолетовый - служебные
                Color.FromArgb(255, 159, 10),   // Золотой - VIP
                Color.FromArgb(90, 200, 250)    // Голубой - тематические
            };

            var colorIndex = 0;
            foreach (var zone in _zones)
            {
                _zoneColors[zone] = colors[colorIndex % colors.Length];
                colorIndex++;
            }
        }

        private void DrawMap()
        {
            _mapPanel.Invalidate();
        }

        private void MapPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисуем фон карты
            using (var brush = new LinearGradientBrush(
                _mapPanel.ClientRectangle,
                Color.FromArgb(240, 248, 255),
                Color.FromArgb(220, 235, 255),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, _mapPanel.ClientRectangle);
            }

            // Рисуем зоны
            foreach (var kvp in _zoneRectangles)
            {
                var zone = kvp.Key;
                var rect = kvp.Value;
                var color = _zoneColors[zone];

                // Основной прямоугольник зоны
                using (var brush = new SolidBrush(Color.FromArgb(200, color)))
                {
                    g.FillRectangle(brush, rect);
                }

                // Граница зоны
                using (var pen = new Pen(color, 2))
                {
                    g.DrawRectangle(pen, rect);
                }

                // Название зоны
                var font = new Font("SF Pro Display", 9F, FontStyle.Bold);
                var textRect = new Rectangle(rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);
                
                using (var textBrush = new SolidBrush(Color.FromArgb(51, 51, 51)))
                {
                    g.DrawString(zone.ZoneName, font, textBrush, textRect, 
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }

                // Выделяем выбранную зону
                if (_selectedZone == zone)
                {
                    using (var pen = new Pen(Color.FromArgb(255, 45, 85), 4))
                    {
                        g.DrawRectangle(pen, rect);
                    }
                }
            }

            // Рисуем легенду
            DrawLegend(g);
        }

        private void DrawLegend(Graphics g)
        {
            var legendRect = new Rectangle(20, 20, 200, 200);
            
            // Фон легенды
            using (var brush = new SolidBrush(Color.FromArgb(240, 240, 240)))
            {
                g.FillRectangle(brush, legendRect);
            }
            
            using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
            {
                g.DrawRectangle(pen, legendRect);
            }

            // Заголовок легенды
            var titleFont = new Font("SF Pro Display", 12F, FontStyle.Bold);
            g.DrawString("Легенда", titleFont, Brushes.Black, legendRect.X + 10, legendRect.Y + 10);

            // Элементы легенды
            var legendItems = new[]
            {
                ("Бассейны", Color.FromArgb(0, 122, 255)),
                ("Горки", Color.FromArgb(255, 149, 0)),
                ("СПА зона", Color.FromArgb(255, 45, 85)),
                ("Развлечения", Color.FromArgb(52, 199, 89)),
                ("Питание", Color.FromArgb(255, 204, 0)),
                ("Служебные", Color.FromArgb(175, 82, 222))
            };

            var itemFont = new Font("SF Pro Display", 9F, FontStyle.Regular);
            int y = 40;
            
            foreach (var (name, color) in legendItems)
            {
                // Цветной квадрат
                using (var brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, legendRect.X + 15, legendRect.Y + y, 12, 12);
                }
                
                // Название
                g.DrawString(name, itemFont, Brushes.Black, legendRect.X + 35, legendRect.Y + y);
                y += 20;
            }
        }

        private void MapPanel_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var kvp in _zoneRectangles)
            {
                if (kvp.Value.Contains(e.Location))
                {
                    _selectedZone = kvp.Key;
                    UpdateZoneInfo();
                    _mapPanel.Invalidate();
                    break;
                }
            }
        }

        private void MapPanel_MouseMove(object sender, MouseEventArgs e)
        {
            bool found = false;
            foreach (var kvp in _zoneRectangles)
            {
                if (kvp.Value.Contains(e.Location))
                {
                    _mapPanel.Cursor = Cursors.Hand;
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                _mapPanel.Cursor = Cursors.Default;
            }
        }

        private void UpdateZoneInfo()
        {
            if (_selectedZone == null)
            {
                _zoneInfoLabel.Text = "Выберите зону на карте";
                return;
            }

            var employeesInZone = _employees.Where(e => e.ZoneId == _selectedZone.ZoneId).ToList();
            
            var info = $"🏊 {_selectedZone.ZoneName}\n\n" +
                      $"📝 Описание:\n{_selectedZone.Description ?? "Описание отсутствует"}\n\n" +
                      $"👥 Вместимость: {_selectedZone.Capacity} чел.\n\n" +
                      $"👷 Сотрудники ({employeesInZone.Count}):\n";

            foreach (var employee in employeesInZone.Take(5))
            {
                info += $"• {employee.FullName} - {employee.Position}\n";
            }

            if (employeesInZone.Count > 5)
            {
                info += $"• ... и еще {employeesInZone.Count - 5} сотрудников\n";
            }

            _zoneInfoLabel.Text = info;
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
