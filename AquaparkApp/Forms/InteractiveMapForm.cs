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

			// Островок и окружение (добавлено поверх фона, под зонами)
            DrawPebbleRoadsBetweenZones(g);
            DrawGrassBushesOnIsland(g);
			DrawWavyCircularIsland(g); // более фигурный, волнообразный контур
			DrawExtraBushes(g, 18);   // дополнительные кусты
			DrawExtraPebbleRoads(g);  // дополнительные дорожки

			// Если данные зон ещё не загружены, завершаем отрисовку без ошибок
			if (_zoneRectangles == null || _zoneColors == null)
			{
				DrawLegend(g);
				return;
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

        // === Дополнительная отрисовка: островок, дорожки из камушков и кусты ===
        private void DrawIslandUnderZones(Graphics g)
        {
            if (_zoneRectangles == null || _zoneRectangles.Count == 0)
            {
                return;
            }

            // Рассчитываем общий прямоугольник, который охватывает все зоны
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            foreach (var rect in _zoneRectangles.Values)
            {
                if (rect.Left < minX) minX = rect.Left;
                if (rect.Top < minY) minY = rect.Top;
                if (rect.Right > maxX) maxX = rect.Right;
                if (rect.Bottom > maxY) maxY = rect.Bottom;
            }

            // Небольшой отступ, чтобы островок выглядел шире объектов
            var padding = 40;
            var islandRect = Rectangle.Inflate(new Rectangle(minX, minY, maxX - minX, maxY - minY), padding, padding);

            // Формируем сглаженный контур островка как скруглённый прямоугольник с дополнительными волнами по краям
            using (var path = CreateRoundedRectPath(islandRect, 45))
            {
                // Песчаная заливка
                using (var sandBrush = new LinearGradientBrush(islandRect,
                    Color.FromArgb(255, 240, 210), // светлый песок
                    Color.FromArgb(230, 208, 170), // теневой песок
                    LinearGradientMode.ForwardDiagonal))
                {
                    g.FillPath(sandBrush, path);
                }

                // Мягкая тень по краю островка
                using (var pen = new Pen(Color.FromArgb(120, 194, 178, 128), 10))
                {
                    pen.LineJoin = LineJoin.Round;
                    g.DrawPath(pen, path);
                }

                using (var pen = new Pen(Color.FromArgb(180, 214, 188, 150), 2f))
                {
                    pen.LineJoin = LineJoin.Round;
                    g.DrawPath(pen, path);
                }
            }
        }

        private void DrawPebbleRoadsBetweenZones(Graphics g)
        {
            if (_zoneRectangles == null || _zoneRectangles.Count < 2)
            {
                return;
            }

            // Соединяем каждую зону с двумя ближайшими соседями
            var zones = _zoneRectangles.Keys.ToList();
            var centerByZone = zones.ToDictionary(z => z, z => new PointF(
                _zoneRectangles[z].X + _zoneRectangles[z].Width / 2f,
                _zoneRectangles[z].Y + _zoneRectangles[z].Height / 2f));

            var connections = new HashSet<string>();
            foreach (var z in zones)
            {
                var center = centerByZone[z];
                var nearest = zones.Where(o => !ReferenceEquals(o, z))
                    .OrderBy(o => Distance(center, centerByZone[o]))
                    .Take(2)
                    .ToList();

                foreach (var n in nearest)
                {
                    var keyA = Math.Min(z.ZoneId, n.ZoneId);
                    var keyB = Math.Max(z.ZoneId, n.ZoneId);
                    var key = keyA + ":" + keyB;
                    if (connections.Contains(key))
                    {
                        continue;
                    }
                    connections.Add(key);

                    DrawPebbleLine(g, centerByZone[z], centerByZone[n]);
                }
            }
        }

        private void DrawGrassBushesOnIsland(Graphics g)
        {
            if (_zoneRectangles == null || _zoneRectangles.Count == 0)
            {
                return;
            }

            // Детерминированный рандом, чтобы кусты не «прыгали» при перерисовках
            var rnd = new Random(12345);

            int index = 0;
            foreach (var kvp in _zoneRectangles)
            {
                // Рисуем кусты не у каждой зоны, чтобы выглядело естественно
                if (index % 3 != 0)
                {
                    index++;
                    continue;
                }

                var rect = kvp.Value;
                // Выбираем точку рядом с зоной, слегка смещённую кнаружи
                var anchor = new PointF(
                    rect.X + rect.Width + rnd.Next(5, 25),
                    rect.Y + rect.Height / 2f + rnd.Next(-10, 10));

                DrawBushCluster(g, anchor, rnd);
                index++;
            }
        }

        private void DrawPebbleLine(Graphics g, PointF a, PointF b)
        {
            var vector = new PointF(b.X - a.X, b.Y - a.Y);
            var length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (length < 10f)
            {
                return;
            }

            var dir = new PointF(vector.X / length, vector.Y / length);
            var step = 8f; // шаг между камушками
            int count = (int)(length / step);

            var rnd = new Random((int)(a.X * 31 + a.Y * 17 + b.X * 13 + b.Y * 7));

            for (int i = 1; i < count; i++)
            {
                var t = i * step;
                var px = a.X + dir.X * t + (float)rnd.NextDouble() * 3f - 1.5f;
                var py = a.Y + dir.Y * t + (float)rnd.NextDouble() * 3f - 1.5f;

                float size = 3f + (float)rnd.NextDouble() * 2.5f;
                var pebbleRect = new RectangleF(px - size / 2f, py - size / 2f, size, size);

                using (var brush = new SolidBrush(Color.FromArgb(200, 180, 180, 180)))
                {
                    g.FillEllipse(brush, pebbleRect);
                }

                using (var pen = new Pen(Color.FromArgb(160, 140, 140, 140), 0.8f))
                {
                    g.DrawEllipse(pen, pebbleRect);
                }
            }
        }

        private void DrawBushCluster(Graphics g, PointF center, Random rnd)
        {
            int leaves = rnd.Next(3, 6);
            for (int i = 0; i < leaves; i++)
            {
                float angle = (float)(i * (Math.PI * 2 / leaves)) + (float)(rnd.NextDouble() * 0.5 - 0.25);
                float radius = 8f + (float)rnd.NextDouble() * 6f;
                var leafCenter = new PointF(
                    center.X + (float)Math.Cos(angle) * radius,
                    center.Y + (float)Math.Sin(angle) * radius);

                float w = 14f + (float)rnd.NextDouble() * 8f;
                float h = 10f + (float)rnd.NextDouble() * 6f;
                var leafRect = new RectangleF(leafCenter.X - w / 2f, leafCenter.Y - h / 2f, w, h);

                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(leafRect);
                    using (var brush = new LinearGradientBrush(leafRect,
                        Color.FromArgb(50, 140, 60),
                        Color.FromArgb(34, 120, 48),
                        LinearGradientMode.Vertical))
                    {
                        g.FillPath(brush, path);
                    }

                    using (var pen = new Pen(Color.FromArgb(80, 90, 90), 1f))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            // Центральная насыщенная часть куста
            var coreRect = new RectangleF(center.X - 7f, center.Y - 6f, 14f, 12f);
            using (var brush = new SolidBrush(Color.FromArgb(255, 46, 160, 70)))
            {
                g.FillEllipse(brush, coreRect);
            }
        }

        private static float Distance(PointF a, PointF b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Дополнительный фигурный волнообразный островок поверх базового
        private void DrawWavyCircularIsland(Graphics g)
        {
            if (_zoneRectangles == null || _zoneRectangles.Count == 0)
            {
                return;
            }

            // На основе центра всех зон рисуем волнообразный круг
            float cx = 0, cy = 0;
            foreach (var rect in _zoneRectangles.Values)
            {
                cx += rect.X + rect.Width / 2f;
                cy += rect.Y + rect.Height / 2f;
            }
            cx /= _zoneRectangles.Count;
            cy /= _zoneRectangles.Count;

            // Радиус охватывает большинство зон
            float maxDist = 0f;
            foreach (var rect in _zoneRectangles.Values)
            {
                var px = rect.X + rect.Width / 2f;
                var py = rect.Y + rect.Height / 2f;
                var d = Distance(new PointF(cx, cy), new PointF(px, py));
                if (d > maxDist) maxDist = d;
            }
            float radius = maxDist + 70f;

            // Волнообразный контур
            using (var path = new GraphicsPath())
            {
                var points = new List<PointF>();
                int segments = 64;
                for (int i = 0; i < segments; i++)
                {
                    float t = (float)i / segments;
                    float ang = t * (float)(Math.PI * 2);
                    // волновая деформация радиуса
                    float wave = (float)(Math.Sin(ang * 5) * 18 + Math.Cos(ang * 3) * 12);
                    float r = radius + wave;
                    points.Add(new PointF(
                        cx + (float)Math.Cos(ang) * r,
                        cy + (float)Math.Sin(ang) * r));
                }
                path.AddClosedCurve(points.ToArray(), 0.6f);

                using (var sand = new PathGradientBrush(points.ToArray()))
                {
                    sand.CenterColor = Color.FromArgb(255, 245, 220);
                    sand.SurroundColors = Enumerable.Repeat(Color.FromArgb(228, 202, 164), points.Count).ToArray();
                    g.FillPath(sand, path);
                }

                using (var outline = new Pen(Color.FromArgb(185, 170, 140), 3f))
                {
                    outline.LineJoin = LineJoin.Round;
                    g.DrawPath(outline, path);
                }
            }
        }

        private void DrawExtraBushes(Graphics g, int count)
        {
            if (_zoneRectangles == null || _zoneRectangles.Count == 0)
            {
                return;
            }

            var rnd = new Random(67890);

            // Рассчитаем общий охватывающий прямоугольник зон
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            foreach (var rect in _zoneRectangles.Values)
            {
                if (rect.Left < minX) minX = rect.Left;
                if (rect.Top < minY) minY = rect.Top;
                if (rect.Right > maxX) maxX = rect.Right;
                if (rect.Bottom > maxY) maxY = rect.Bottom;
            }
            var area = Rectangle.Inflate(new Rectangle(minX, minY, maxX - minX, maxY - minY), 30, 30);

            for (int i = 0; i < count; i++)
            {
                var p = new PointF(
                    rnd.Next(area.Left, area.Right),
                    rnd.Next(area.Top, area.Bottom));
                DrawBushCluster(g, p, rnd);
            }
        }

        private void DrawExtraPebbleRoads(Graphics g)
        {
            if (_zoneRectangles == null || _zoneRectangles.Count < 2)
            {
                return;
            }

            var zones = _zoneRectangles.Keys.ToList();
            var centers = zones.ToDictionary(z => z, z => new PointF(
                _zoneRectangles[z].X + _zoneRectangles[z].Width / 2f,
                _zoneRectangles[z].Y + _zoneRectangles[z].Height / 2f));

            // Построим дополнительные связи: k-nearest с k=4
            var seen = new HashSet<string>();
            foreach (var z in zones)
            {
                var cz = centers[z];
                var nearest = zones.Where(o => !ReferenceEquals(o, z))
                    .OrderBy(o => Distance(cz, centers[o]))
                    .Take(4)
                    .ToList();

                foreach (var n in nearest)
                {
                    var a = Math.Min(z.ZoneId, n.ZoneId);
                    var b = Math.Max(z.ZoneId, n.ZoneId);
                    var key = a + ":" + b;
                    if (seen.Contains(key))
                    {
                        continue;
                    }
                    seen.Add(key);

                    // Лёгкая кривизна дорожек: промежуточная точка для изящного изгиба
                    var p0 = cz;
                    var p2 = centers[n];
                    var mid = new PointF((p0.X + p2.X) / 2f, (p0.Y + p2.Y) / 2f);
                    var normal = new PointF(-(p2.Y - p0.Y), p2.X - p0.X);
                    var normLen = (float)Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y);
                    if (normLen > 1f)
                    {
                        normal = new PointF(normal.X / normLen, normal.Y / normLen);
                    }
                    var p1 = new PointF(mid.X + normal.X * 18f, mid.Y + normal.Y * 18f);

                    DrawPebbleCurve(g, p0, p1, p2);
                }
            }
        }

        private void DrawPebbleCurve(Graphics g, PointF p0, PointF p1, PointF p2)
        {
            // Апроксимация квадратичной Безье мелкими сегментами и рисование камушков
            float total = Distance(p0, p1) + Distance(p1, p2);
            float step = 8f;
            int count = Math.Max(2, (int)(total / step));
            var rnd = new Random((int)(p0.X * 17 + p0.Y * 29 + p1.X * 31 + p1.Y * 37 + p2.X * 41 + p2.Y * 43));
            for (int i = 1; i < count; i++)
            {
                float t = i / (float)count;
                var q = QuadraticBezier(p0, p1, p2, t);
                float size = 2.8f + (float)rnd.NextDouble() * 3.2f;
                var pebble = new RectangleF(q.X - size / 2f, q.Y - size / 2f, size, size);
                using (var brush = new SolidBrush(Color.FromArgb(205, 182, 182, 182)))
                {
                    g.FillEllipse(brush, pebble);
                }
                using (var pen = new Pen(Color.FromArgb(160, 145, 145, 145), 0.8f))
                {
                    g.DrawEllipse(pen, pebble);
                }
            }
        }

        private static PointF QuadraticBezier(PointF p0, PointF p1, PointF p2, float t)
        {
            float u = 1 - t;
            return new PointF(
                u * u * p0.X + 2 * u * t * p1.X + t * t * p2.X,
                u * u * p0.Y + 2 * u * t * p1.Y + t * t * p2.Y);
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
