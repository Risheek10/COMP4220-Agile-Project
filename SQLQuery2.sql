-- 1. Category
CREATE TABLE [dbo].[Category] (
    [CategoryID]  INT           NOT NULL,
    [Name]        VARCHAR(80)   NULL,
    [Description] VARCHAR(255)  NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);

-- 2. Supplier
CREATE TABLE [dbo].[Supplier] (
    [SupplierId] INT          NOT NULL,
    [Name]       VARCHAR(80)  NOT NULL,
    CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([SupplierId] ASC)
);

-- 3. UserData
CREATE TABLE [dbo].[UserData] (
    [UserID]   INT           NOT NULL,
    [UserName] VARCHAR(20)   NOT NULL,
    [Password] VARCHAR(25)   NOT NULL,
    [Type]     CHAR(2)       NOT NULL,
    [Manager]  BIT           NOT NULL,
    [FullName] NVARCHAR(50)  NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

-- 4. Orders
CREATE TABLE [dbo].[Orders] (
    [OrderID]         INT IDENTITY(1, 1) NOT NULL,
    [UserID]          INT NOT NULL,
    [OrderDate]       DATETIME CONSTRAINT [DF_Orders_OrderDate] DEFAULT (GETDATE()) NOT NULL,
    [Status]          CHAR(1) CONSTRAINT [DF_Orders_StatusCode] DEFAULT ('P') NOT NULL,
    [DiscountPercent] INT DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_Orders_Employee] FOREIGN KEY ([UserID]) REFERENCES [dbo].[UserData] ([UserID])
);

-- 5. BookData
CREATE TABLE [dbo].[BookData] (
    [ISBN]       CHAR(10)        NOT NULL,
    [CategoryID] INT             NOT NULL,
    [Title]      VARCHAR(80)     NULL,
    [Author]     VARCHAR(255)    NULL,
    [Price]      DECIMAL(10, 2)  NULL,
    [SupplierId] INT             NULL,
    [Year]       NCHAR(4)        NULL,
    [Edition]    NCHAR(2)        NOT NULL,
    [Publisher]  NVARCHAR(50)    NULL,
    [InStock]    INT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ISBN] ASC),
    CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID]),
    CONSTRAINT [FK_Product_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Supplier] ([SupplierId])
);

-- 6. OrderItem
CREATE TABLE [dbo].[OrderItem] (
    [OrderID]  INT       NOT NULL,
    [ISBN]     CHAR(10)  NOT NULL,
    [Quantity] INT       NOT NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([OrderID] ASC, [ISBN] ASC),
    CONSTRAINT [FK_OrderItem_Orders] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Orders] ([OrderID]),
    CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY ([ISBN]) REFERENCES [dbo].[BookData] ([ISBN])
);

-- 7. DiscountData
CREATE TABLE [dbo].[DiscountData] (
    [Ccode]        VARCHAR(10)    NOT NULL,
    [discount]     DECIMAL(18, 2) NOT NULL,
    [DiscountDesc] VARCHAR(50)    NULL,
    PRIMARY KEY CLUSTERED ([Ccode] ASC)
);