-- Insert sample data for aquapark database
-- UTF-8 encoding

-- Insert clients data
INSERT INTO clients (client_id, full_name, phone, email, birth_date, registration_date) VALUES
(1, 'Ivanov Aleksandr Sergeevich', '+79161234567', 'ivanov.alex@mail.ru', '1990-05-15', '2024-01-10'),
(2, 'Petrova Ekaterina Viktorovna', '+79261234568', 'petrova.ekaterina@mail.ru', '1985-12-20', '2024-01-11'),
(3, 'Sidorov Dmitry Nikolaevich', '+79361234569', 'sidorov.dmitry@mail.ru', '1995-08-03', '2024-01-12'),
(4, 'Kuznetsova Olga Ivanovna', '+79461234570', 'kuznetsova.olga@mail.ru', '1988-03-25', '2024-01-13'),
(5, 'Vasiliev Mikhail Petrovich', '+79561234571', 'vasiliev.mikhail@mail.ru', '1992-11-30', '2024-01-14'),
(6, 'Nikolaeva Anna Dmitrievna', '+79661234572', 'nikolaeva.anna@mail.ru', '1993-07-18', '2024-01-15'),
(7, 'Fedorov Sergey Vladimirovich', '+79761234573', 'fedorov.sergey@mail.ru', '1987-02-14', '2024-01-16'),
(8, 'Aleksandrova Maria Igorevna', '+79861234574', 'aleksandrova.maria@mail.ru', '1991-09-22', '2024-01-17'),
(9, 'Pavlov Andrey Konstantinovich', '+79961234575', 'pavlov.andrey@mail.ru', '1989-06-08', '2024-01-18'),
(10, 'Semenova Elena Vasilievna', '+79031234576', 'semenova.elena@mail.ru', '1994-04-17', '2024-01-19');

-- Insert zones data
INSERT INTO zones (zone_id, zone_name, description, capacity) VALUES
(1, 'Children Pool', 'Shallow pool for children under 7 years', 30),
(2, 'Adult Pool', 'Olympic pool 50 meters', 50),
(3, 'Waterfall Slide', 'High slide with steep turns', 20),
(4, 'Rainbow Slide', 'Winding slide for the whole family', 25),
(5, 'Jacuzzi', 'Zone with hydromassage baths', 15),
(6, 'Sauna', 'Finnish sauna for 10 people', 10),
(7, 'Bathhouse', 'Russian bath with brooms', 12),
(8, 'Wave Cafe', 'Main cafe with hot meals', 40),
(9, 'Aqua Bar', 'Bar with refreshing drinks', 20),
(10, 'Children Playground', 'Play area for children', 25);

-- Insert employees data
INSERT INTO employees (employee_id, full_name, position, phone, hire_date, zone_id) VALUES
(1, 'Smirnov Aleksey Vladimirovich', 'Administrator', '+79161111111', '2023-01-15', 8),
(2, 'Ivanova Mariya Petrovna', 'Cashier', '+79162222222', '2023-02-20', 8),
(3, 'Petrov Dmitry Sergeevich', 'Lifeguard', '+79163333333', '2023-03-10', 2),
(4, 'Sidorova Elena Viktorovna', 'Lifeguard', '+79164444444', '2023-04-05', 1),
(5, 'Kuznetsov Andrey Nikolaevich', 'Instructor', '+79165555555', '2023-05-12', 3);

-- Insert services data
INSERT INTO services (service_id, name, description, price) VALUES
(1, 'Entry Ticket', 'One-time visit to the aquapark', 1500.00),
(2, 'Monthly Pass', 'Unlimited visits for a month', 8000.00),
(3, 'Family Ticket', 'Visit for 2 adults and 2 children', 4000.00),
(4, 'Children Ticket', 'For children under 12 years', 800.00),
(5, 'Senior Ticket', 'For seniors with ID', 1000.00),
(6, 'Locker Rental', 'Individual locker for belongings', 300.00),
(7, 'Towel Rental', 'Large terry towel', 200.00),
(8, 'Bathrobe Rental', 'Terry bathrobe', 500.00),
(9, 'Full Meal', 'First course, main course, drink and dessert', 600.00),
(10, 'Business Lunch', 'Soup, salad, main dish', 450.00);

-- Insert tickets data
INSERT INTO tickets (ticket_id, client_id, ticket_type, price, purchase_date, valid_until) VALUES
(1, 1, 'Entry Ticket', 1500.00, '2024-03-01', '2024-03-01'),
(2, 2, 'Monthly Pass', 8000.00, '2024-03-01', '2024-03-31'),
(3, 3, 'Family Ticket', 4000.00, '2024-03-02', '2024-03-02'),
(4, 4, 'Children Ticket', 800.00, '2024-03-02', '2024-03-02'),
(5, 5, 'Senior Ticket', 1000.00, '2024-03-03', '2024-03-03');

-- Insert inventory data
INSERT INTO inventory (inventory_id, name, quantity, status, zone_id, responsible_employee_id) VALUES
(1, 'Children Life Vest', 50, 'working', 1, 4),
(2, 'Adult Life Vest', 40, 'working', 2, 3),
(3, 'Children Inflatable Ring', 30, 'working', 1, 4),
(4, 'Adult Inflatable Ring', 25, 'working', 2, 3),
(5, 'Fins', 20, 'working', 2, 3),
(6, 'Swimming Mask', 15, 'working', 2, 3),
(7, 'Swimming Tube', 10, 'repair', 2, 3),
(8, 'Swimming Board', 12, 'working', 2, 3),
(9, 'Children Arm Bands', 35, 'working', 1, 4),
(10, 'Swimming Goggles', 18, 'working', 2, 3);

-- Update sequences
SELECT setval('clients_client_id_seq', (SELECT MAX(client_id) FROM clients));
SELECT setval('zones_zone_id_seq', (SELECT MAX(zone_id) FROM zones));
SELECT setval('employees_employee_id_seq', (SELECT MAX(employee_id) FROM employees));
SELECT setval('services_service_id_seq', (SELECT MAX(service_id) FROM services));
SELECT setval('tickets_ticket_id_seq', (SELECT MAX(ticket_id) FROM tickets));
SELECT setval('inventory_inventory_id_seq', (SELECT MAX(inventory_id) FROM inventory));
