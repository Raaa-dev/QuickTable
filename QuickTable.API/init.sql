-- =============================================
-- QuickTable Database Schema Initialization
-- =============================================

-- 1. Users
CREATE TABLE IF NOT EXISTS "user" (
    id SERIAL PRIMARY KEY,
    user_name VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL,
    refresh_token VARCHAR(500),
    is_active BOOLEAN DEFAULT TRUE
);

-- 2. Menu Categories
CREATE TABLE IF NOT EXISTS menu_categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE
);

-- 3. Menu Items
CREATE TABLE IF NOT EXISTS menu_items (
    id SERIAL PRIMARY KEY,
    category_id INT NOT NULL REFERENCES menu_categories(id),
    name VARCHAR(100) NOT NULL,
    price NUMERIC NOT NULL,
    image_url VARCHAR(500),
    is_active BOOLEAN DEFAULT TRUE
);

-- 4. Tables
CREATE TABLE IF NOT EXISTS tables (
    id SERIAL PRIMARY KEY,
    table_number VARCHAR(20) UNIQUE NOT NULL,
    capacity INT,
    is_active BOOLEAN DEFAULT TRUE
);

-- 5. Table QR Codes
CREATE TABLE IF NOT EXISTS table_qr_codes (
    id SERIAL PRIMARY KEY,
    table_id INT NOT NULL REFERENCES tables(id),
    qr_token VARCHAR(500) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE
);

-- 6. Table Sessions
CREATE TABLE IF NOT EXISTS table_sessions (
    id SERIAL PRIMARY KEY,
    table_id INT NOT NULL REFERENCES tables(id),
    session_code VARCHAR(100),
    started_at TIMESTAMP,
    end_at TIMESTAMP,
    status VARCHAR(50)
);

-- 7. Orders
CREATE TABLE IF NOT EXISTS orders (
    id SERIAL PRIMARY KEY,
    table_session_id INT NOT NULL REFERENCES table_sessions(id),
    order_number VARCHAR(100),
    total_amount NUMERIC,
    status VARCHAR(50)
);

-- 8. Order Items
CREATE TABLE IF NOT EXISTS order_items (
    id SERIAL PRIMARY KEY,
    order_id INT NOT NULL REFERENCES orders(id),
    menu_item_id INT NOT NULL REFERENCES menu_items(id),
    quantity INT NOT NULL,
    price NUMERIC NOT NULL,
    subtotal NUMERIC NOT NULL
);

-- 9. Notification Logs
CREATE TABLE IF NOT EXISTS notification_logs (
    id SERIAL PRIMARY KEY,
    order_id INT NOT NULL REFERENCES orders(id),
    message VARCHAR(200),
    status VARCHAR(50),
    send_at TIMESTAMP
);
