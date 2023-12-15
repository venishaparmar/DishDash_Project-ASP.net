-- Create User table
CREATE TABLE Users (
    id INT PRIMARY KEY IDENTITY(1, 1),
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL,
    password NVARCHAR(255) NOT NULL,
    create_at DATETIME NOT NULL
);

-- Create Product table
CREATE TABLE Product (
    id INT PRIMARY KEY IDENTITY(1, 1),
    active BIT NOT NULL,
    title NVARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    image VARBINARY(MAX), -- VARBINARY(MAX) for image data
    create_at DATETIME NOT NULL,
    update_at DATETIME
);

-- Create Category table
CREATE TABLE Category (
    id INT PRIMARY KEY IDENTITY(1, 1),
    active BIT,
    image VARBINARY(MAX),
    title VARCHAR(255) NOT NULL,
    create_at DATETIME NOT NULL,
    update_at DATETIME
);

-- Add category_id column to Product table
ALTER TABLE Product
ADD category_id INT,
    FOREIGN KEY (category_id) REFERENCES Category(id);

-- Create Payment table
CREATE TABLE Payment (
    id INT PRIMARY KEY IDENTITY(1, 1),
    amount DECIMAL(10, 2),
    method VARCHAR(50),
    create_at DATETIME
);

-- Create Orders table
CREATE TABLE Orders (
    id INT PRIMARY KEY IDENTITY(1, 1),
    no_of_items INT,
    total_amount DECIMAL(10, 2),
    payment_id INT, -- Added payment_id as a foreign key
    FOREIGN KEY (payment_id) REFERENCES Payment(id)
);



-- Create Receipt table
CREATE TABLE Receipt (
    id INT PRIMARY KEY IDENTITY(1, 1),
	ordered_id INT,
    customer_id INT,
    payment_id INT,
    create_at DATETIME,
    FOREIGN KEY (ordered_id) REFERENCES Orders(id),
    FOREIGN KEY (customer_id) REFERENCES Customer(id),
    FOREIGN KEY (payment_id) REFERENCES Payment(id)
);

-- Create Admin table
CREATE TABLE Admin (
    id INT PRIMARY KEY IDENTITY(1, 1),
    password VARCHAR(255) NOT NULL,
    username VARCHAR(50) NOT NULL,
    name VARCHAR(255) NOT NULL
);

-- Create Customer table
CREATE TABLE Customer (
    id INT PRIMARY KEY IDENTITY(1, 1),
    user_id INT,
    order_id INT,
    address VARCHAR(255),
    contact VARCHAR(20),
    FOREIGN KEY (user_id) REFERENCES Users(id),
    FOREIGN KEY (order_id) REFERENCES Orders(id)
);

-- Create Ordered_Items table
CREATE TABLE Ordered_Items (
    id INT PRIMARY KEY IDENTITY(1, 1),
    product_id INT,
    quantity INT,
    order_id INT,
    price DECIMAL(10, 2),
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (order_id) REFERENCES Orders(id)
);

-- Create AddToCart table
CREATE TABLE AddToCart (
    id INT PRIMARY KEY IDENTITY(1, 1),
    user_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    added_at DATETIME NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id),
    FOREIGN KEY (product_id) REFERENCES Product(id)
);

ALTER TABLE Customer
ADD name VARCHAR(100);

ALTER TABLE Customer
DROP CONSTRAINT FK__Customer__order___33D4B598;
ALTER TABLE Customer
DROP COLUMN order_id;

ALTER TABLE Payment
ADD customer_id INT, 
    FOREIGN KEY (customer_id) REFERENCES Customer(id);

SELECT * FROM Receipt;
SP_HELP [User]