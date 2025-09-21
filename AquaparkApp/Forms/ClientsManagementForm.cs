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
    public partial class ClientsManagementForm : Form
    {
        private Panel _mainPanel;
        private Panel _headerPanel;
        private Panel _contentPanel;
        private DataGridView _clientsGrid;
        private MacOSButton _addButton;
        private MacOSButton _editButton;
        private MacOSButton _deleteButton;
        private MacOSButton _refreshButton;
        private TextBox _searchTextBox;
        private ClientRepository _clientRepository;
        private List<Client> _clients;

        public ClientsManagementForm()
        {
            InitializeComponent();
            InitializeRepository();
            SetupUI();
            LoadClients();
        }


        private void InitializeRepository()
        {
            _clientRepository = new ClientRepository();
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
                Height = 100,
                BackColor = Color.FromArgb(0, 122, 255)
            };

            // Заголовок
            var titleLabel = new Label
            {
                Text = "👥 Управление клиентами",
                Font = new Font("SF Pro Display", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Поиск
            var searchLabel = new Label
            {
                Text = "Поиск:",
                Font = new Font("SF Pro Display", 12F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(30, 55),
                AutoSize = true
            };

            _searchTextBox = new TextBox
            {
                Size = new Size(200, 25),
                Location = new Point(80, 53),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;

            // Кнопки
            _addButton = new MacOSButton
            {
                Text = "➕ Добавить",
                Size = new Size(100, 30),
                Location = new Point(300, 50),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _addButton.Click += AddButton_Click;

            _editButton = new MacOSButton
            {
                Text = "✏️ Изменить",
                Size = new Size(100, 30),
                Location = new Point(410, 50),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                Enabled = false
            };
            _editButton.Click += EditButton_Click;

            _deleteButton = new MacOSButton
            {
                Text = "🗑️ Удалить",
                Size = new Size(100, 30),
                Location = new Point(520, 50),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular),
                Enabled = false
            };
            _deleteButton.Click += DeleteButton_Click;

            _refreshButton = new MacOSButton
            {
                Text = "🔄 Обновить",
                Size = new Size(100, 30),
                Location = new Point(630, 50),
                Font = new Font("SF Pro Display", 11F, FontStyle.Regular)
            };
            _refreshButton.Click += RefreshButton_Click;

            _headerPanel.Controls.AddRange(new Control[] 
            {
                titleLabel, searchLabel, _searchTextBox, _addButton, _editButton, _deleteButton, _refreshButton
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

            // Таблица клиентов
            _clientsGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
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
            _clientsGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 255);
            _clientsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _clientsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("SF Pro Display", 11F, FontStyle.Bold);
            _clientsGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            _clientsGrid.SelectionChanged += ClientsGrid_SelectionChanged;

            _contentPanel.Controls.Add(_clientsGrid);
        }

        private async void LoadClients()
        {
            try
            {
                _clients = (await _clientRepository.GetAllAsync()).ToList();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGrid()
        {
            var displayData = _clients.Select(c => new
            {
                ID = c.ClientId,
                ФИО = c.FullName,
                Телефон = c.Phone,
                Email = c.Email ?? "Не указан",
                Дата_рождения = c.BirthDate.ToString("dd.MM.yyyy"),
                Возраст = c.Age,
                Дата_регистрации = c.RegistrationDate.ToString("dd.MM.yyyy")
            }).ToList();

            _clientsGrid.DataSource = displayData;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            var searchTerm = _searchTextBox.Text.ToLower();
            if (string.IsNullOrEmpty(searchTerm))
            {
                RefreshGrid();
                return;
            }

            var filteredClients = _clients.Where(c => 
                c.FullName.ToLower().Contains(searchTerm) ||
                c.Phone.Contains(searchTerm) ||
                (c.Email?.ToLower().Contains(searchTerm) ?? false)).ToList();

            var displayData = filteredClients.Select(c => new
            {
                ID = c.ClientId,
                ФИО = c.FullName,
                Телефон = c.Phone,
                Email = c.Email ?? "Не указан",
                Дата_рождения = c.BirthDate.ToString("dd.MM.yyyy"),
                Возраст = c.Age,
                Дата_регистрации = c.RegistrationDate.ToString("dd.MM.yyyy")
            }).ToList();

            _clientsGrid.DataSource = displayData;
        }

        private void ClientsGrid_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = _clientsGrid.SelectedRows.Count > 0;
            _editButton.Enabled = hasSelection;
            _deleteButton.Enabled = hasSelection;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // Здесь можно открыть форму добавления клиента
            MessageBox.Show("Функция добавления клиента будет реализована", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (_clientsGrid.SelectedRows.Count > 0)
            {
                var selectedRow = _clientsGrid.SelectedRows[0];
                var clientId = (int)selectedRow.Cells["ID"].Value;
                var client = _clients.FirstOrDefault(c => c.ClientId == clientId);
                
                if (client != null)
                {
                    // Здесь можно открыть форму редактирования клиента
                    MessageBox.Show($"Редактирование клиента: {client.FullName}", "Информация", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_clientsGrid.SelectedRows.Count > 0)
            {
                var selectedRow = _clientsGrid.SelectedRows[0];
                var clientId = (int)selectedRow.Cells["ID"].Value;
                var client = _clients.FirstOrDefault(c => c.ClientId == clientId);
                
                if (client != null)
                {
                    var result = MessageBox.Show($"Вы уверены, что хотите удалить клиента {client.FullName}?", 
                        "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            await _clientRepository.DeleteAsync(clientId);
                            _clients.Remove(client);
                            RefreshGrid();
                            MessageBox.Show("Клиент успешно удален", "Успех", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка удаления клиента: {ex.Message}", "Ошибка", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadClients();
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
