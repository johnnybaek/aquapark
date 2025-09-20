-- Создание таблиц для аквапарка
\c aquapark_db;

-- Таблица пользователей
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Phone VARCHAR(20),
    DateOfBirth DATE,
    IsAdmin BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    LastLoginAt TIMESTAMP,
    IsActive BOOLEAN DEFAULT TRUE
);

-- Таблица аттракционов
CREATE TABLE Attractions (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(10,2) NOT NULL,
    MinAge INTEGER DEFAULT 0,
    MaxAge INTEGER DEFAULT 100,
    MinHeight INTEGER DEFAULT 0, -- в см
    MaxHeight INTEGER DEFAULT 300, -- в см
    Capacity INTEGER DEFAULT 1, -- максимальная вместимость
    Duration INTEGER DEFAULT 30, -- продолжительность в минутах
    ImagePath VARCHAR(255),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP,
    Category VARCHAR(50) DEFAULT 'Общие',
    DifficultyLevel VARCHAR(20) DEFAULT 'Средний' -- Легкий, Средний, Сложный
);

-- Таблица заказов
CREATE TABLE Orders (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id) ON DELETE CASCADE,
    OrderNumber VARCHAR(20) UNIQUE NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    DiscountAmount DECIMAL(10,2) DEFAULT 0,
    FinalAmount DECIMAL(10,2) NOT NULL,
    Status VARCHAR(20) DEFAULT 'Pending', -- Pending, Paid, Completed, Cancelled
    PaymentMethod VARCHAR(20), -- Cash, Card, Online
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PaidAt TIMESTAMP,
    CompletedAt TIMESTAMP,
    Notes TEXT
);

-- Таблица билетов
CREATE TABLE Tickets (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id) ON DELETE CASCADE,
    AttractionId INTEGER REFERENCES Attractions(Id) ON DELETE CASCADE,
    OrderId INTEGER REFERENCES Orders(Id) ON DELETE SET NULL,
    VisitDate DATE NOT NULL,
    VisitTime TIME NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Quantity INTEGER DEFAULT 1,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Status VARCHAR(20) DEFAULT 'Pending', -- Pending, Confirmed, Used, Cancelled
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ConfirmedAt TIMESTAMP,
    UsedAt TIMESTAMP,
    QrCode VARCHAR(100) UNIQUE,
    Notes TEXT
);

-- Таблица акций/промокодов
CREATE TABLE Promotions (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    DiscountPercent DECIMAL(5,2) DEFAULT 0,
    FixedDiscountAmount DECIMAL(10,2),
    MinOrderAmount DECIMAL(10,2) DEFAULT 0,
    StartDate TIMESTAMP NOT NULL,
    EndDate TIMESTAMP NOT NULL,
    PromoCode VARCHAR(20) UNIQUE,
    UsageLimit INTEGER DEFAULT 0, -- 0 = без ограничений
    UsedCount INTEGER DEFAULT 0,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ImagePath VARCHAR(255)
);

-- Таблица корзины покупок
CREATE TABLE ShoppingCarts (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id) ON DELETE CASCADE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблица элементов корзины
CREATE TABLE CartItems (
    Id SERIAL PRIMARY KEY,
    CartId INTEGER REFERENCES ShoppingCarts(Id) ON DELETE CASCADE,
    AttractionId INTEGER REFERENCES Attractions(Id) ON DELETE CASCADE,
    VisitDate DATE NOT NULL,
    VisitTime TIME NOT NULL,
    Quantity INTEGER DEFAULT 1,
    UnitPrice DECIMAL(10,2) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблица для хранения сессий пользователей
CREATE TABLE UserSessions (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id) ON DELETE CASCADE,
    SessionToken VARCHAR(255) UNIQUE NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ExpiresAt TIMESTAMP NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    IpAddress VARCHAR(45),
    UserAgent TEXT
);

-- Таблица для логирования действий пользователей
CREATE TABLE UserActivityLogs (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id) ON DELETE SET NULL,
    Action VARCHAR(100) NOT NULL,
    Description TEXT,
    IpAddress VARCHAR(45),
    UserAgent TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблица для настроек системы
CREATE TABLE SystemSettings (
    Id SERIAL PRIMARY KEY,
    SettingKey VARCHAR(100) UNIQUE NOT NULL,
    SettingValue TEXT,
    Description TEXT,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedBy INTEGER REFERENCES Users(Id)
);

-- Таблица для уведомлений
CREATE TABLE Notifications (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id) ON DELETE CASCADE,
    Title VARCHAR(200) NOT NULL,
    Message TEXT NOT NULL,
    Type VARCHAR(50) DEFAULT 'Info', -- Info, Warning, Error, Success
    IsRead BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ReadAt TIMESTAMP
);

-- Таблица для отзывов и рейтингов
CREATE TABLE Reviews (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id) ON DELETE CASCADE,
    AttractionId INTEGER REFERENCES Attractions(Id) ON DELETE CASCADE,
    Rating INTEGER CHECK (Rating >= 1 AND Rating <= 5),
    Comment TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsApproved BOOLEAN DEFAULT FALSE
);

-- Обновление таблицы Users с дополнительными полями
ALTER TABLE Users ADD COLUMN IF NOT EXISTS TotalOrders INTEGER DEFAULT 0;
ALTER TABLE Users ADD COLUMN IF NOT EXISTS TotalSpent DECIMAL(10,2) DEFAULT 0;
ALTER TABLE Users ADD COLUMN IF NOT EXISTS AvatarPath VARCHAR(255);
ALTER TABLE Users ADD COLUMN IF NOT EXISTS Address TEXT;
ALTER TABLE Users ADD COLUMN IF NOT EXISTS Gender VARCHAR(10);
ALTER TABLE Users ADD COLUMN IF NOT EXISTS PreferredLanguage VARCHAR(5) DEFAULT 'ru';

-- Создание индексов для оптимизации
CREATE INDEX idx_users_username ON Users(Username);
CREATE INDEX idx_users_email ON Users(Email);
CREATE INDEX idx_attractions_category ON Attractions(Category);
CREATE INDEX idx_attractions_active ON Attractions(IsActive);
CREATE INDEX idx_orders_user_id ON Orders(UserId);
CREATE INDEX idx_orders_created_at ON Orders(CreatedAt);
CREATE INDEX idx_tickets_user_id ON Tickets(UserId);
CREATE INDEX idx_tickets_attraction_id ON Tickets(AttractionId);
CREATE INDEX idx_tickets_visit_date ON Tickets(VisitDate);
CREATE INDEX idx_promotions_promo_code ON Promotions(PromoCode);
CREATE INDEX idx_promotions_active ON Promotions(IsActive, StartDate, EndDate);
CREATE INDEX idx_cart_items_cart_id ON CartItems(CartId);
CREATE INDEX idx_cart_items_attraction_id ON CartItems(AttractionId);
CREATE INDEX idx_user_sessions_user_id ON UserSessions(UserId);
CREATE INDEX idx_user_sessions_token ON UserSessions(SessionToken);
CREATE INDEX idx_user_sessions_expires ON UserSessions(ExpiresAt);
CREATE INDEX idx_activity_logs_user_id ON UserActivityLogs(UserId);
CREATE INDEX idx_activity_logs_created_at ON UserActivityLogs(CreatedAt);
CREATE INDEX idx_notifications_user_id ON Notifications(UserId);
CREATE INDEX idx_notifications_is_read ON Notifications(IsRead);
CREATE INDEX idx_reviews_attraction_id ON Reviews(AttractionId);
CREATE INDEX idx_reviews_user_id ON Reviews(UserId);
