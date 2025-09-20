using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using AquaparkApp.Controls;

namespace AquaparkApp.Forms
{
    public partial class VideoPlayerForm : Form
    {
        private Panel _mainPanel;
        private GlassPanel _videoPanel;
        private Panel _controlsPanel;
        private MacOSButton _playButton;
        private MacOSButton _pauseButton;
        private MacOSButton _stopButton;
        private MacOSButton _closeButton;
        private Label _titleLabel;
        private TrackBar _progressBar;
        private Label _timeLabel;
        private System.Windows.Forms.Timer _timer;
        private bool _isPlaying = false;
        private string _videoPath;

        public VideoPlayerForm(string videoPath = null)
        {
            _videoPath = videoPath;
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройки формы
            this.Text = "Видеопроигрыватель";
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

            // Создаем панель для видео
            _videoPanel = new GlassPanel
            {
                Size = new Size(900, 500),
                Location = new Point(50, 50),
                BackColor = Color.FromArgb(50, 0, 0, 0)
            };

            // Заголовок
            _titleLabel = new Label
            {
                Text = "🎬 Демонстрационный ролик аквапарка",
                Font = new Font("SF Pro Display", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 20),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Панель управления
            _controlsPanel = new Panel
            {
                Size = new Size(900, 80),
                Location = new Point(50, 570),
                BackColor = Color.FromArgb(200, 255, 255, 255)
            };

            // Кнопка воспроизведения
            _playButton = new MacOSButton
            {
                Text = "▶️ Воспроизвести",
                Size = new Size(120, 40),
                Location = new Point(50, 20),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _playButton.Click += PlayButton_Click;

            // Кнопка паузы
            _pauseButton = new MacOSButton
            {
                Text = "⏸️ Пауза",
                Size = new Size(100, 40),
                Location = new Point(180, 20),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Enabled = false
            };
            _pauseButton.Click += PauseButton_Click;

            // Кнопка остановки
            _stopButton = new MacOSButton
            {
                Text = "⏹️ Стоп",
                Size = new Size(100, 40),
                Location = new Point(290, 20),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                Enabled = false
            };
            _stopButton.Click += StopButton_Click;

            // Прогресс-бар
            _progressBar = new TrackBar
            {
                Size = new Size(300, 40),
                Location = new Point(450, 20),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                TickStyle = TickStyle.None
            };
            _progressBar.ValueChanged += ProgressBar_ValueChanged;

            // Метка времени
            _timeLabel = new Label
            {
                Text = "00:00 / 00:00",
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(760, 25),
                Size = new Size(100, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Кнопка закрытия
            _closeButton = new MacOSButton
            {
                Text = "❌ Закрыть",
                Size = new Size(100, 40),
                Location = new Point(800, 20),
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular)
            };
            _closeButton.Click += CloseButton_Click;

            // Таймер для обновления прогресса
            _timer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            _timer.Tick += Timer_Tick;

            // Добавляем контролы
            _controlsPanel.Controls.AddRange(new Control[] 
            {
                _playButton, _pauseButton, _stopButton, _progressBar, _timeLabel, _closeButton
            });

            _mainPanel.Controls.AddRange(new Control[] 
            {
                _videoPanel, _titleLabel, _controlsPanel
            });

            this.Controls.Add(_mainPanel);

            // Загружаем демонстрационное видео
            LoadDemoVideo();
        }

        private void LoadDemoVideo()
        {
            // Создаем демонстрационное содержимое
            var demoContent = new Label
            {
                Text = "🎬\n\nДемонстрационный ролик\nаквапарка \"Водный мир\"\n\n" +
                      "Здесь будет показан\nвидеоролик с аттракционами,\n" +
                      "развлечениями и услугами\nнашего аквапарка\n\n" +
                      "Нажмите \"Воспроизвести\"\nдля начала просмотра",
                Font = new Font("SF Pro Display", 16F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(50, 50),
                Size = new Size(800, 400),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _videoPanel.Controls.Add(demoContent);
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            _isPlaying = true;
            _playButton.Enabled = false;
            _pauseButton.Enabled = true;
            _stopButton.Enabled = true;
            _timer.Start();

            // Имитация воспроизведения
            var demoLabel = _videoPanel.Controls[0] as Label;
            if (demoLabel != null)
            {
                demoLabel.Text = "🎬\n\n▶️ Воспроизведение...\n\n" +
                               "Демонстрация водных горок,\n" +
                               "бассейнов, СПА-зоны\n" +
                               "и других развлечений\n\n" +
                               "⏱️ 00:15 / 02:30";
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            _isPlaying = false;
            _playButton.Enabled = true;
            _pauseButton.Enabled = false;
            _timer.Stop();

            var demoLabel = _videoPanel.Controls[0] as Label;
            if (demoLabel != null)
            {
                demoLabel.Text = "🎬\n\n⏸️ Пауза\n\n" +
                               "Демонстрация водных горок,\n" +
                               "бассейнов, СПА-зоны\n" +
                               "и других развлечений\n\n" +
                               "⏱️ 00:15 / 02:30";
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _isPlaying = false;
            _playButton.Enabled = true;
            _pauseButton.Enabled = false;
            _stopButton.Enabled = false;
            _timer.Stop();
            _progressBar.Value = 0;
            _timeLabel.Text = "00:00 / 02:30";

            LoadDemoVideo();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            this.Close();
        }

        private void ProgressBar_ValueChanged(object sender, EventArgs e)
        {
            var currentTime = TimeSpan.FromSeconds(_progressBar.Value * 150 / 100); // 2.5 минуты = 150 секунд
            var totalTime = TimeSpan.FromSeconds(150);
            _timeLabel.Text = $"{currentTime:mm\\:ss} / {totalTime:mm\\:ss}";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isPlaying && _progressBar.Value < _progressBar.Maximum)
            {
                _progressBar.Value += 1;
            }
            else if (_progressBar.Value >= _progressBar.Maximum)
            {
                StopButton_Click(sender, e);
            }
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
