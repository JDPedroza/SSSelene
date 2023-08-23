-- Crear la base de datos DB_Selene si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DB_Selene')
BEGIN
    CREATE DATABASE DB_Selene;
END
GO

-- Usar la base de datos DB_Selene
USE DB_Selene;
GO

-- Tabla de Usuarios
CREATE TABLE users (
    user_id TINYINT IDENTITY(1, 1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL,
    user_type VARCHAR(20) NOT NULL
);

-- Tabla de Proveedores
CREATE TABLE providers (
    provider_id TINYINT IDENTITY(1, 1) PRIMARY KEY,
    provider_name VARCHAR(100) NOT NULL,
    nit VARCHAR(20) NOT NULL,
    phone VARCHAR(20),
    mobile VARCHAR(20),
    email VARCHAR(100)
);

-- Tabla de Productos
CREATE TABLE products (
    product_id TINYINT IDENTITY(1, 1) PRIMARY KEY,
    product_name VARCHAR(100) NOT NULL,
    product_code VARCHAR(20) NOT NULL,
    value DECIMAL(10, 2) NOT NULL,
    creation_date DATE,
    provider_id TINYINT,
    photo VARBINARY(MAX),
    FOREIGN KEY (provider_id) REFERENCES providers(provider_id)
);

-- Tabla de Ventas
CREATE TABLE sales (
    sale_id TINYINT IDENTITY(1, 1) PRIMARY KEY,
    product_id TINYINT,
    quantity INT NOT NULL,
    tax DECIMAL(5, 2) NOT NULL,
    sale_date DATE,
    seller_id TINYINT,
    final_price DECIMAL(10, 2),
    FOREIGN KEY (product_id) REFERENCES products(product_id),
    FOREIGN KEY (seller_id) REFERENCES users(user_id)
);