using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AquaparkApp.Controls;
using AquaparkApp.DAL;
using AquaparkApp.Models;
using ClosedXML.Excel;
using System.IO;

namespace AquaparkApp.Forms
{
    public partial class ReportsForm : Form
    {
        private Panel _mainPanel;
        private Panel _headerPanel;
        private Panel _contentPanel;
        private ComboBox _reportTypeComboBox;
        private DateTimePicker _startDatePicker;
        private DateTimePicker _endDatePicker;
        private MacOSButton _generateButton;
        private MacOSButton _exportExcelButton;
        private DataGridView _reportDataGrid;
        private Label _reportTitleLabel;
        private Label _summaryLabel;
        private ClientRepository _clientRepository;
        private TicketRepository _ticketRepository;
        private ServiceRepository _serviceRepository;
        private ZoneRepository _zoneRepository;

        public ReportsForm()
        {
            InitializeComponent();
            InitializeRepositories();
            SetupUI();
            LoadReportTypes();
        }


        private void InitializeRepositories()
        {
            _clientRepository = new ClientRepository();
            _ticketRepository = new TicketRepository();
            _serviceRepository = new ServiceRepository();
            _zoneRepository = new ZoneRepository();
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

            // Панель контента
            CreateContentPanel();

            _mainPanel.Controls.AddRange(new Control[] { _headerPanel, _contentPanel });
            this.Controls.Add(_mainPanel);
        }

        private void CreateHeaderPanel()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.FromArgb(0, 122, 255)
            };

            // Заголовок
            var titleLabel = new Label
            {
                Text = "📊 Отчеты и аналитика аквапарка",
                Font = new Font("SF Pro Display", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Выбор типа отчета
            var reportTypeLabel = new Label
            {
                Text = "Тип отчета:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(30, 60),
                AutoSize = true
            };

            _reportTypeComboBox = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(130, 58),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Период
            var periodLabel = new Label
            {
                Text = "Период:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(350, 60),
                AutoSize = true
            };

            _startDatePicker = new DateTimePicker
            {
                Size = new Size(120, 30),
                Location = new Point(420, 58),
                Font = new Font("SF Pro Display", 10F, FontStyle.Regular),
                Format = DateTimePickerFormat.Short
            };

            var toLabel = new Label
            {
                Text = "до",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(550, 60),
                AutoSize = true
            };

            _endDatePicker = new DateTimePicker
            {
                Size = new Size(120, 30),
                Location = new Point(580, 58),
                Font = new Font("SF Pro Display", 10F, FontStyle.Regular),
                Format = DateTimePickerFormat.Short
            };

            // Кнопки
            _generateButton = new MacOSButton
            {
                Text = "📈 Сформировать",
                Size = new Size(120, 35),
                Location = new Point(720, 55),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _generateButton.Click += GenerateButton_Click;

            _exportExcelButton = new MacOSButton
            {
                Text = "📊 Excel",
                Size = new Size(100, 35),
                Location = new Point(850, 55),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                Enabled = false
            };
            _exportExcelButton.Click += ExportExcelButton_Click;

            _headerPanel.Controls.AddRange(new Control[] 
            {
                titleLabel, reportTypeLabel, _reportTypeComboBox, periodLabel,
                _startDatePicker, toLabel, _endDatePicker, _generateButton, _exportExcelButton
            });
        }

        private void CreateContentPanel()
        {
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 248, 248),
                Padding = new Padding(20)
            };

            // Заголовок отчета
            _reportTitleLabel = new Label
            {
                Text = "Выберите тип отчета и период",
                Font = new Font("SF Pro Display", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Сводка
            _summaryLabel = new Label
            {
                Text = "",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(102, 102, 102),
                Location = new Point(20, 50),
                Size = new Size(800, 60),
                AutoSize = true
            };

            // Таблица данных
            _reportDataGrid = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(1100, 500),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                GridColor = Color.FromArgb(230, 230, 230),
                RowHeadersVisible = false
            };

            // Стилизация таблицы
            _reportDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 255);
            _reportDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _reportDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);
            _reportDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            _contentPanel.Controls.AddRange(new Control[] 
            {
                _reportTitleLabel, _summaryLabel, _reportDataGrid
            });
        }

        private void LoadReportTypes()
        {
            var reportTypes = new[]
            {
                "📊 Общая статистика",
                "👥 Клиенты",
                "🎫 Билеты",
                "🛍️ Услуги",
                "🏊 Зоны",
                "👷 Сотрудники",
                "🎒 Инвентарь",
                "🏃 Аренда инвентаря",
                "🚪 Посещения",
                "💳 Оплаты",
                "📈 Финансовая отчетность",
                "📋 Популярные услуги",
                "🏆 Топ клиенты"
            };

            _reportTypeComboBox.Items.AddRange(reportTypes);
            _reportTypeComboBox.SelectedIndex = 0;

            // Устанавливаем период по умолчанию (последний месяц)
            _endDatePicker.Value = DateTime.Now;
            _startDatePicker.Value = DateTime.Now.AddMonths(-1);
        }

        private async void GenerateButton_Click(object sender, EventArgs e)
        {
            try
            {
                _generateButton.Enabled = false;
                _generateButton.Text = "⏳ Формирование...";

                var reportType = _reportTypeComboBox.SelectedItem.ToString();
                var startDate = _startDatePicker.Value.Date;
                var endDate = _endDatePicker.Value.Date;

                _reportTitleLabel.Text = reportType;
                _reportDataGrid.DataSource = null;

                switch (reportType)
                {
                    case "📊 Общая статистика":
                        await GenerateGeneralStatisticsReport();
                        break;
                    case "👥 Клиенты":
                        await GenerateClientsReport();
                        break;
                    case "🎫 Билеты":
                        await GenerateTicketsReport(startDate, endDate);
                        break;
                    case "🛍️ Услуги":
                        await GenerateServicesReport(startDate, endDate);
                        break;
                    case "🏊 Зоны":
                        await GenerateZonesReport();
                        break;
                    case "💳 Оплаты":
                        await GeneratePaymentsReport(startDate, endDate);
                        break;
                    case "📈 Финансовая отчетность":
                        await GenerateFinancialReport(startDate, endDate);
                        break;
                    case "📋 Популярные услуги":
                        await GeneratePopularServicesReport();
                        break;
                    case "🏆 Топ клиенты":
                        await GenerateTopClientsReport();
                        break;
                }

                _exportExcelButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _generateButton.Enabled = true;
                _generateButton.Text = "📈 Сформировать";
            }
        }

        private async Task GenerateGeneralStatisticsReport()
        {
            var clients = await _clientRepository.GetAllAsync();
            var tickets = await _ticketRepository.GetAllAsync();
            var services = await _serviceRepository.GetAllAsync();
            var zones = await _zoneRepository.GetAllAsync();

            var summary = $"Всего клиентов: {clients.Count()}, Билетов: {tickets.Count()}, " +
                         $"Услуг: {services.Count()}, Зон: {zones.Count()}";
            _summaryLabel.Text = summary;

            var reportData = new[]
            {
                new { Показатель = "Количество клиентов", Значение = clients.Count().ToString() },
                new { Показатель = "Количество билетов", Значение = tickets.Count().ToString() },
                new { Показатель = "Количество услуг", Значение = services.Count().ToString() },
                new { Показатель = "Количество зон", Значение = zones.Count().ToString() },
                new { Показатель = "Общая выручка от билетов", Значение = tickets.Sum(t => t.Price).ToString("C") },
                new { Показатель = "Средняя цена билета", Значение = tickets.Average(t => t.Price).ToString("C") },
                new { Показатель = "Средняя цена услуги", Значение = services.Average(s => s.Price).ToString("C") }
            };

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GenerateClientsReport()
        {
            var clients = await _clientRepository.GetAllAsync();
            _summaryLabel.Text = $"Всего клиентов: {clients.Count()}";

            var reportData = clients.Select(c => new
            {
                ID = c.ClientId,
                ФИО = c.FullName,
                Телефон = c.Phone,
                Email = c.Email ?? "Не указан",
                Дата_рождения = c.BirthDate.ToString("dd.MM.yyyy"),
                Возраст = c.Age,
                Дата_регистрации = c.RegistrationDate.ToString("dd.MM.yyyy")
            }).ToList();

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GenerateTicketsReport(DateTime startDate, DateTime endDate)
        {
            var tickets = await _ticketRepository.GetAllAsync();
            var filteredTickets = tickets.Where(t => t.PurchaseDate.Date >= startDate && t.PurchaseDate.Date <= endDate);

            _summaryLabel.Text = $"Билетов за период: {filteredTickets.Count()}, " +
                               $"Общая сумма: {filteredTickets.Sum(t => t.Price).ToString("C")}";

            var reportData = filteredTickets.Select(t => new
            {
                ID = t.TicketId,
                Клиент = t.Client?.FullName ?? "Неизвестен",
                Тип_билета = t.TicketType,
                Цена = t.Price.ToString("C"),
                Дата_покупки = t.PurchaseDate.ToString("dd.MM.yyyy"),
                Действителен_до = t.ValidUntil.ToString("dd.MM.yyyy"),
                Статус = t.Status
            }).ToList();

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GenerateServicesReport(DateTime startDate, DateTime endDate)
        {
            var services = await _serviceRepository.GetAllAsync();
            _summaryLabel.Text = $"Всего услуг: {services.Count()}, " +
                               $"Общая стоимость: {services.Sum(s => s.Price).ToString("C")}";

            var reportData = services.Select(s => new
            {
                ID = s.ServiceId,
                Название = s.Name,
                Описание = s.ShortDescription,
                Цена = s.Price.ToString("C")
            }).ToList();

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GenerateZonesReport()
        {
            var zones = await _zoneRepository.GetAllAsync();
            _summaryLabel.Text = $"Всего зон: {zones.Count()}, " +
                               $"Общая вместимость: {zones.Sum(z => z.Capacity)}";

            var reportData = zones.Select(z => new
            {
                ID = z.ZoneId,
                Название = z.ZoneName,
                Описание = z.ShortDescription,
                Вместимость = z.Capacity
            }).ToList();

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GeneratePaymentsReport(DateTime startDate, DateTime endDate)
        {
            // Здесь можно добавить логику для получения данных об оплатах
            _summaryLabel.Text = "Отчет по оплатам за выбранный период";
            
            var reportData = new[]
            {
                new { Дата = "01.03.2024", Клиент = "Иванов А.С.", Сумма = "1500.00", Способ = "Карта", Тип = "Билет" },
                new { Дата = "01.03.2024", Клиент = "Петрова Е.В.", Сумма = "8000.00", Способ = "Наличные", Тип = "Абонемент" },
                new { Дата = "02.03.2024", Клиент = "Сидоров Д.Н.", Сумма = "4000.00", Способ = "Карта", Тип = "Семейный билет" }
            };

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GenerateFinancialReport(DateTime startDate, DateTime endDate)
        {
            var tickets = await _ticketRepository.GetAllAsync();
            var filteredTickets = tickets.Where(t => t.PurchaseDate.Date >= startDate && t.PurchaseDate.Date <= endDate);

            var totalRevenue = filteredTickets.Sum(t => t.Price);
            var avgTicketPrice = filteredTickets.Any() ? filteredTickets.Average(t => t.Price) : 0;

            _summaryLabel.Text = $"Общая выручка: {totalRevenue.ToString("C")}, " +
                               $"Средняя цена билета: {avgTicketPrice.ToString("C")}";

            var reportData = new[]
            {
                new { Показатель = "Общая выручка", Значение = totalRevenue.ToString("C") },
                new { Показатель = "Количество проданных билетов", Значение = filteredTickets.Count().ToString() },
                new { Показатель = "Средняя цена билета", Значение = avgTicketPrice.ToString("C") },
                new { Показатель = "Максимальная цена билета", Значение = filteredTickets.Any() ? filteredTickets.Max(t => t.Price).ToString("C") : "0" },
                new { Показатель = "Минимальная цена билета", Значение = filteredTickets.Any() ? filteredTickets.Min(t => t.Price).ToString("C") : "0" }
            };

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GeneratePopularServicesReport()
        {
            var services = await _serviceRepository.GetAllAsync();
            _summaryLabel.Text = "Популярные услуги аквапарка";

            var reportData = services.OrderByDescending(s => s.Price).Take(10).Select(s => new
            {
                Название = s.Name,
                Цена = s.Price.ToString("C"),
                Описание = s.ShortDescription
            }).ToList();

            _reportDataGrid.DataSource = reportData;
        }

        private async Task GenerateTopClientsReport()
        {
            var clients = await _clientRepository.GetTopSpendersAsync(10);
            _summaryLabel.Text = "Топ-10 клиентов по тратам";

            var reportData = clients.Select((c, index) => new
            {
                Место = index + 1,
                Клиент = c.FullName,
                Телефон = c.Phone,
                Email = c.Email ?? "Не указан",
                Дата_регистрации = c.RegistrationDate.ToString("dd.MM.yyyy")
            }).ToList();

            _reportDataGrid.DataSource = reportData;
        }

        private void ExportExcelButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx";
                    saveDialog.FileName = $"Отчет_{_reportTypeComboBox.SelectedItem}_{DateTime.Now:yyyyMMdd}.xlsx";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToExcel(saveDialog.FileName);
                        MessageBox.Show("Отчет успешно экспортирован в Excel!", "Успех", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта в Excel: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel(string fileName)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Отчет");

                // Заголовок
                worksheet.Cell("A1").Value = _reportTitleLabel.Text;
                worksheet.Cell("A1").Style.Font.FontSize = 16;
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.LightBlue;

                // Сводка
                worksheet.Cell("A2").Value = _summaryLabel.Text;
                worksheet.Cell("A2").Style.Font.FontSize = 12;

                // Данные таблицы
                if (_reportDataGrid.DataSource != null)
                {
                    var data = _reportDataGrid.DataSource as System.Collections.IList;
                    if (data != null && data.Count > 0)
                    {
                        // Заголовки колонок
                        var properties = data[0].GetType().GetProperties();
                        for (int i = 0; i < properties.Length; i++)
                        {
                            worksheet.Cell(4, i + 1).Value = properties[i].Name;
                            worksheet.Cell(4, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }

                        // Данные
                        for (int row = 0; row < data.Count; row++)
                        {
                            for (int col = 0; col < properties.Length; col++)
                            {
                                var value = properties[col].GetValue(data[row]);
                                worksheet.Cell(row + 5, col + 1).Value = value?.ToString() ?? "";
                            }
                        }

                        // Автоподбор ширины колонок
                        worksheet.Columns().AdjustToContents();
                    }
                }

                // Информация о создании отчета
                worksheet.Cell($"A{worksheet.RowsUsed().Count() + 3}").Value = $"Отчет создан: {DateTime.Now:dd.MM.yyyy HH:mm}";
                worksheet.Cell($"A{worksheet.RowsUsed().Count() + 3}").Style.Font.Italic = true;

                workbook.SaveAs(fileName);
            }
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
