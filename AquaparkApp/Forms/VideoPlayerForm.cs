using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms.Integration;
using System.Windows.Controls;
using WF = System.Windows.Forms;
using AquaparkApp.Controls;

namespace AquaparkApp.Forms
{
    public partial class VideoPlayerForm : Form
    {
        private WF.Panel _mainPanel;
        private GlassPanel _videoPanel;
        private WF.Panel _controlsPanel;
        private MacOSButton _playButton;
        private MacOSButton _pauseButton;
        private MacOSButton _closeButton;
        private WF.Label _titleLabel;
        private WF.TrackBar _progressBar;
        private WF.Label _timeLabel;
        private WF.Timer _timer;
        private bool _isPlaying = false;
        private string _videoPath;
        private MediaElement _mediaElement;
        private ElementHost _host;
        private TimeSpan _totalDuration = TimeSpan.FromSeconds(150); // default 2.5 minutes

        public VideoPlayerForm(string videoPath = null)
        {
            if (videoPath == null)
            {
                videoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Videos", "demo.mp4");
            }
            _videoPath = videoPath;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SetupUI();
        }


        private void SetupUI()
        {
            // Создаем главную панель
            _mainPanel = new WF.Panel
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

            _host = new ElementHost();
            _host.Dock = DockStyle.Fill;
            _videoPanel.Controls.Add(_host);

            _mediaElement = new MediaElement();
            _mediaElement.LoadedBehavior = MediaState.Manual;
            _mediaElement.UnloadedBehavior = MediaState.Manual;
            _mediaElement.MediaOpened += MediaElement_MediaOpened;
            _host.Child = _mediaElement;

            // Заголовок
            _titleLabel = new WF.Label
            {
                Text = "🎬 Демонстрационный ролик аквапарка",
                Font = new Font("SF Pro Display", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 20),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Панель управления
            _controlsPanel = new WF.Panel
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

            // Прогресс-бар
            _progressBar = new WF.TrackBar
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
            _timeLabel = new WF.Label
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
            _timer = new WF.Timer
            {
                Interval = 1000
            };
            _timer.Tick += Timer_Tick;

            // Добавляем контролы
            _controlsPanel.Controls.AddRange(new WF.Control[]
            {
                _playButton, _pauseButton, _progressBar, _timeLabel, _closeButton
            });

            _mainPanel.Controls.AddRange(new WF.Control[]
            {
                _videoPanel, _titleLabel, _controlsPanel
            });

            this.Controls.Add(_mainPanel);

            // Загружаем демонстрационное видео
            LoadDemoVideo();

            if (_mediaElement.Source == null)
            {
                _timeLabel.Text = $"00:00 / {_totalDuration:mm\\:ss}";
            }
        }

        private void LoadDemoVideo()
        {
            if (!string.IsNullOrEmpty(_videoPath) && File.Exists(_videoPath))
            {
                _mediaElement.Source = new Uri("file://" + _videoPath);
                // Do not auto-play, let user control
            }
            else
            {
                // Создаем демонстрационное содержимое
                var demoContent = new WF.Label
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
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (_mediaElement.Source != null)
            {
                _mediaElement.Play();
                _isPlaying = true;
                _playButton.Enabled = false;
                _pauseButton.Enabled = true;
                _timer.Start();
            }
            else
            {
                _isPlaying = true;
                _playButton.Enabled = false;
                _pauseButton.Enabled = true;
                _timer.Start();

                // Имитация воспроизведения
                var demoLabel = _videoPanel.Controls[0] as WF.Label;
                if (demoLabel != null)
                {
                    var currentTime = TimeSpan.FromSeconds(_progressBar.Value / 100.0 * _totalDuration.TotalSeconds);
                    demoLabel.Text = "🎬\n\n▶️ Воспроизведение...\n\n" +
                                   "Демонстрация водных горок,\n" +
                                   "бассейнов, СПА-зоны\n" +
                                   "и других развлечений\n\n" +
                                   $"⏱️ {currentTime:mm\\:ss} / {_totalDuration:mm\\:ss}";
                }
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (_mediaElement.Source != null)
            {
                _mediaElement.Pause();
                _isPlaying = false;
                _playButton.Enabled = true;
                _pauseButton.Enabled = false;
                _timer.Stop();
            }
            else
            {
                _isPlaying = false;
                _playButton.Enabled = true;
                _pauseButton.Enabled = false;
                _timer.Stop();

                var demoLabel = _videoPanel.Controls[0] as WF.Label;
                if (demoLabel != null)
                {
                    var currentTime = TimeSpan.FromSeconds(_progressBar.Value / 100.0 * _totalDuration.TotalSeconds);
                    demoLabel.Text = "🎬\n\n⏸️ Пауза\n\n" +
                                   "Демонстрация водных горок,\n" +
                                   "бассейнов, СПА-зоны\n" +
                                   "и других развлечений\n\n" +
                                   $"⏱️ {currentTime:mm\\:ss} / {_totalDuration:mm\\:ss}";
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            _mediaElement.Stop();
            _timer.Stop();
            this.Close();
        }

        private void MediaElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_mediaElement.NaturalDuration.HasTimeSpan)
            {
                _totalDuration = _mediaElement.NaturalDuration.TimeSpan;
            }
        }

        private void ProgressBar_ValueChanged(object sender, EventArgs e)
        {
            if (_mediaElement.Source != null && _mediaElement.NaturalDuration.HasTimeSpan)
            {
                var total = _mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                var current = _progressBar.Value / 100.0 * total;
                _mediaElement.Position = TimeSpan.FromSeconds(current);
                var currentTime = TimeSpan.FromSeconds(current);
                _timeLabel.Text = $"{currentTime:mm\\:ss} / {_mediaElement.NaturalDuration.TimeSpan:mm\\:ss}";
            }
            else
            {
                var currentTime = TimeSpan.FromSeconds(_progressBar.Value / 100.0 * _totalDuration.TotalSeconds);
                _timeLabel.Text = $"{currentTime:mm\\:ss} / {_totalDuration:mm\\:ss}";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_mediaElement.Source != null && _mediaElement.NaturalDuration.HasTimeSpan)
            {
                var total = _mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                var current = _mediaElement.Position.TotalSeconds;
                _progressBar.Value = (int)(current / total * 100);
                var currentTime = TimeSpan.FromSeconds(current);
                _timeLabel.Text = $"{currentTime:mm\\:ss} / {_mediaElement.NaturalDuration.TimeSpan:mm\\:ss}";
                if (current >= total)
                {
                    _mediaElement.Stop();
                    _mediaElement.Position = TimeSpan.Zero;
                    _isPlaying = false;
                    _playButton.Enabled = true;
                    _pauseButton.Enabled = false;
                    _timer.Stop();
                    _progressBar.Value = 0;
                    _timeLabel.Text = "00:00 / 00:00";
                }
            }
            else
            {
                if (_isPlaying && _progressBar.Value < _progressBar.Maximum)
                {
                    _progressBar.Value += 1;
                    var currentTime = TimeSpan.FromSeconds(_progressBar.Value / 100.0 * _totalDuration.TotalSeconds);
                    _timeLabel.Text = $"{currentTime:mm\\:ss} / {_totalDuration:mm\\:ss}";
                }
                else if (_progressBar.Value >= _progressBar.Maximum)
                {
                    _isPlaying = false;
                    _playButton.Enabled = true;
                    _pauseButton.Enabled = false;
                    _timer.Stop();
                    _progressBar.Value = 0;
                    _timeLabel.Text = $"00:00 / {_totalDuration:mm\\:ss}";

                    LoadDemoVideo();
                }
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
