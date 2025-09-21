-- Скрипт создания базы данных аквапарка
-- Выполните этот скрипт в PostgreSQL для создания базы данных и таблиц

-- Создание базы данных (выполните от имени суперпользователя)
-- CREATE DATABASE aquapark_db;

-- Подключение к базе данных aquapark_db
-- \c aquapark_db;

-- 1. Таблица Клиенты
CREATE TABLE IF NOT EXISTS clients (
    client_id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    email VARCHAR(100),
    birth_date DATE NOT NULL,
    registration_date DATE NOT NULL DEFAULT CURRENT_DATE
);

-- 2. Таблица Билеты
CREATE TABLE IF NOT EXISTS tickets (
    ticket_id SERIAL PRIMARY KEY,
    client_id INT NOT NULL,
    ticket_type VARCHAR(50) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    purchase_date DATE NOT NULL DEFAULT CURRENT_DATE,
    valid_until DATE NOT NULL,
    FOREIGN KEY (client_id) REFERENCES clients(client_id)
);

-- 3. Таблица Услуги
CREATE TABLE IF NOT EXISTS services (
    service_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL
);

-- 4. Таблица Зоны аквапарка
CREATE TABLE IF NOT EXISTS zones (
    zone_id SERIAL PRIMARY KEY,
    zone_name VARCHAR(100) NOT NULL,
    description TEXT,
    capacity INT NOT NULL
);

-- 5. Таблица Сотрудники
CREATE TABLE IF NOT EXISTS employees (
    employee_id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    position VARCHAR(50) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    hire_date DATE NOT NULL DEFAULT CURRENT_DATE,
    zone_id INT,
    FOREIGN KEY (zone_id) REFERENCES zones(zone_id)
);

-- 6. Таблица Расписание
CREATE TABLE IF NOT EXISTS schedule (
    schedule_id SERIAL PRIMARY KEY,
    employee_id INT NOT NULL,
    zone_id INT NOT NULL,
    work_date DATE NOT NULL,
    shift_start TIME NOT NULL,
    shift_end TIME NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id),
    FOREIGN KEY (zone_id) REFERENCES zones(zone_id)
);

-- 7. Таблица Инвентарь
CREATE TABLE IF NOT EXISTS inventory (
    inventory_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    quantity INT NOT NULL,
    status VARCHAR(50) NOT NULL,
    zone_id INT,
    responsible_employee_id INT,
    FOREIGN KEY (zone_id) REFERENCES zones(zone_id),
    FOREIGN KEY (responsible_employee_id) REFERENCES employees(employee_id)
);

-- 8. Таблица Аренда инвентаря
CREATE TABLE IF NOT EXISTS inventory_rentals (
    rental_id SERIAL PRIMARY KEY,
    client_id INT NOT NULL,
    inventory_id INT NOT NULL,
    employee_id INT NOT NULL,
    rental_date DATE NOT NULL DEFAULT CURRENT_DATE,
    return_date DATE,
    deposit_amount DECIMAL(10, 2),
    FOREIGN KEY (client_id) REFERENCES clients(client_id),
    FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id),
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id)
);

-- 9. Таблица Посещения
CREATE TABLE IF NOT EXISTS visits (
    visit_id SERIAL PRIMARY KEY,
    client_id INT NOT NULL,
    ticket_id INT NOT NULL,
    entry_time TIMESTAMP NOT NULL,
    exit_time TIMESTAMP,
    zone_id INT,
    FOREIGN KEY (client_id) REFERENCES clients(client_id),
    FOREIGN KEY (ticket_id) REFERENCES tickets(ticket_id),
    FOREIGN KEY (zone_id) REFERENCES zones(zone_id)
);

-- 10. Таблица Обслуживание клиентов
CREATE TABLE IF NOT EXISTS client_services (
    service_log_id SERIAL PRIMARY KEY,
    client_id INT NOT NULL,
    service_id INT NOT NULL,
    employee_id INT NOT NULL,
    service_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    quantity INT NOT NULL DEFAULT 1,
    FOREIGN KEY (client_id) REFERENCES clients(client_id),
    FOREIGN KEY (service_id) REFERENCES services(service_id),
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id)
);

-- 11. Таблица Оплаты
CREATE TABLE IF NOT EXISTS payments (
    payment_id SERIAL PRIMARY KEY,
    client_id INT NOT NULL,
    ticket_id INT,
    service_id INT,
    employee_id INT NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    payment_date DATE NOT NULL DEFAULT CURRENT_DATE,
    payment_method VARCHAR(20) NOT NULL,
    FOREIGN KEY (client_id) REFERENCES clients(client_id),
    FOREIGN KEY (ticket_id) REFERENCES tickets(ticket_id),
    FOREIGN KEY (service_id) REFERENCES services(service_id),
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id)
);

-- Создание индексов для улучшения производительности
CREATE INDEX IF NOT EXISTS idx_clients_email ON clients(email);
CREATE INDEX IF NOT EXISTS idx_clients_phone ON clients(phone);
CREATE INDEX IF NOT EXISTS idx_tickets_client_id ON tickets(client_id);
CREATE INDEX IF NOT EXISTS idx_visits_client_id ON visits(client_id);
CREATE INDEX IF NOT EXISTS idx_visits_entry_time ON visits(entry_time);
CREATE INDEX IF NOT EXISTS idx_payments_client_id ON payments(client_id);
CREATE INDEX IF NOT EXISTS idx_payments_date ON payments(payment_date);
CREATE INDEX IF NOT EXISTS idx_employees_zone_id ON employees(zone_id);
CREATE INDEX IF NOT EXISTS idx_inventory_rentals_client_id ON inventory_rentals(client_id);
CREATE INDEX IF NOT EXISTS idx_inventory_rentals_return_date ON inventory_rentals(return_date);

-- Комментарии к таблицам
COMMENT ON TABLE clients IS 'Таблица клиентов аквапарка';
COMMENT ON TABLE tickets IS 'Таблица билетов';
COMMENT ON TABLE services IS 'Таблица услуг';
COMMENT ON TABLE zones IS 'Таблица зон аквапарка';
COMMENT ON TABLE employees IS 'Таблица сотрудников';
COMMENT ON TABLE schedule IS 'Таблица расписания работы';
COMMENT ON TABLE inventory IS 'Таблица инвентаря';
COMMENT ON TABLE inventory_rentals IS 'Таблица аренды инвентаря';
COMMENT ON TABLE visits IS 'Таблица посещений';
COMMENT ON TABLE client_services IS 'Таблица обслуживания клиентов';
COMMENT ON TABLE payments IS 'Таблица оплат';
