-------------------------------------------------------
-- 1. CATEGORY
-------------------------------------------------------
INSERT INTO [dbo].[Category] ([CategoryID], [Name], [Description]) VALUES (1, N'Programming Languages', N'Programming Language');
INSERT INTO [dbo].[Category] ([CategoryID], [Name], [Description]) VALUES (2, N'Software Development', N'Project Management');
INSERT INTO [dbo].[Category] ([CategoryID], [Name], [Description]) VALUES (3, N'New Books in Fall 2018', N'Fall 2018');
INSERT INTO [dbo].[Category] ([CategoryID], [Name], [Description]) VALUES (4, N'Textbooks', N'Used in class');

-------------------------------------------------------
-- 2. SUPPLIER
-------------------------------------------------------
INSERT INTO [dbo].[Supplier] ([SupplierId], [Name]) VALUES (1, N'amazon.ca');
INSERT INTO [dbo].[Supplier] ([SupplierId], [Name]) VALUES (2, N'Chapter Bookstore');
INSERT INTO [dbo].[Supplier] ([SupplierId], [Name]) VALUES (3, N'Springer');

-------------------------------------------------------
-- 3. USERDATA
-------------------------------------------------------
INSERT INTO [dbo].[UserData] ([UserID], [UserName], [Password], [Type], [Manager], [FullName]) VALUES (1, N'dclark', N'dc1234', N'SA', 0, N'Donald Clark');
INSERT INTO [dbo].[UserData] ([UserID], [UserName], [Password], [Type], [Manager], [FullName]) VALUES (2, N'jsmith', N'js1234', N'SA', 0, N'Jone Smith');
INSERT INTO [dbo].[UserData] ([UserID], [UserName], [Password], [Type], [Manager], [FullName]) VALUES (3, N'mjones', N'mj1234', N'SA', 1, N'Mary Jones');
INSERT INTO [dbo].[UserData] ([UserID], [UserName], [Password], [Type], [Manager], [FullName]) VALUES (4, N'klink', N'kl1234', N'RG', 0, N'King Link');
INSERT INTO [dbo].[UserData] ([UserID], [UserName], [Password], [Type], [Manager], [FullName]) VALUES (5, N'sone', N'so1234', N'RG', 0, N'Some One');
INSERT INTO [dbo].[UserData] ([UserID], [UserName], [Password], [Type], [Manager], [FullName]) VALUES (6, N'selse', N'se1234', N'RG', 0, N'Someone Else');

-------------------------------------------------------
-- 4. DISCOUNTDATA
-------------------------------------------------------
INSERT INTO [dbo].[DiscountData] ([Ccode], [discount], [DiscountDesc]) VALUES (N'CP10', CAST(0.10 AS Decimal(18, 2)), N'10% discount');
INSERT INTO [dbo].[DiscountData] ([Ccode], [discount], [DiscountDesc]) VALUES (N'CP20', CAST(0.20 AS Decimal(18, 2)), N'20% discount');
INSERT INTO [dbo].[DiscountData] ([Ccode], [discount], [DiscountDesc]) VALUES (N'CP50', CAST(0.50 AS Decimal(18, 2)), N'half price');
INSERT INTO [dbo].[DiscountData] ([Ccode], [discount], [DiscountDesc]) VALUES (N'CP60', CAST(0.60 AS Decimal(18, 2)), N'60% discount');

-------------------------------------------------------
-- 5. BOOKDATA
-------------------------------------------------------
INSERT INTO [dbo].[BookData] ([ISBN], [CategoryID], [Title], [Author], [Price], [SupplierId], [Year], [Edition], [Publisher], [InStock]) VALUES
(N'0135974445', 2, N'Agile Software Development, Principles, Patterns, and Practices', N'Robert C. Martin', 70.40, 1, N'2002', N'1 ', N'Pearson', 0),
(N'0321146530', 2, N'Test Driven Development: By Example', N'Kent Beck', 41.59, 1, N'2002', N'1 ', N'Addison-Wesley Professional', 0),
(N'0321278658', 2, N'Extreme Programming Explained: Embrace Change', N'Kent Beck and Cynthia Andres', 44.63, 1, N'2004', N'2 ', N'Addison-Wesley Professional', 99),
(N'073561993X', 2, N'Agile Project Management with Scrum', N'Ken Schwaber', 26.45, 1, N'2004', N'1 ', N'Microsoft Press', 97),
(N'0987      ', 3, N'A Good Book', N'Nice', 12.00, NULL, N'2020', N'  ', N'', 0),
(N'1118026241', 2, N'Agile Project Management For Dummies', N'Mark C. Layton', 26.99, 1, N'2012', N'1 ', N'For Dummies', 85),
(N'1234      ', 4, N'ABCD', N'Efg', 0.00, NULL, N'2018', N'  ', N'', 0),
(N'1234345   ', 4, N'Some Title', N'Someone', 0.01, NULL, N'2018', N'  ', N'Someone', 1),
(N'1234567890', 4, N'A No-Show Book As Not In Stock ', N'Some One', 10.00, 2, N'2011', N'  ', N'Somewhere', 0),
(N'1285096339', 1, N'Microsoft Visual C# 2012: An Introduction to Object-Oriented Programming', N'Joyce Farrell', 185.11, 1, N'2013', N'5 ', N'Course Technology', 97),
(N'1443452483', 3, N'Educated', N'Tara Westover', 11.49, 2, N'2018', N'  ', N'HarperCollins', 1),
(N'1491922834', 4, N'Learning Virtual Reality: Developing Immersive Experiences and Applications', N'Tony Parisi', 39.71, 1, N'2015', N'1 ', N'O''Reilly Media', 85),
(N'1554683831', 3, N'Becoming', N'Michelle Obama', 24.00, 2, N'2018', N'  ', N'Harper Collins', 100),
(N'1617290890', 1, N'The Art of Unit Testing: with examples in C#', N'Roy Osherove', 57.28, 1, N'2013', N'1 ', N'Manning Publications', 74),
(N'161729134X', 1, N'NULLC# in Depth', N'Jon Skeet', 41.22, 1, N'2013', N'3 ', N'Manning Publications', 100),
(N'161729232X', 1, N'Unity in Action: Multiplatform Game Development in C# with Unity 5', N'Joe Hocking', 47.54, 1, N'2015', N'1 ', N'Manning Publications', 66),
(N'1852333944', 1, N'Essential Java 3D Fast : Developing 3D Graphics Applications in Java', N'Ian Palmer', 89.99, 3, N'2001', N'  ', N'Springer-Verlag', 100),
(N'1884777902', 1, N'3D User Interfaces with Java 3D', N'Jon Barrilleaux', 100.00, 1, N'2000', N'  ', N'Manning Publications', 55),
(N'1941393101', 4, N'Virtual Reality Beginner''s Guide + Google Cardboard', N'Patrick Buckley and Frederic Lardinois', 20.95, 1, N'2014', N'  ', N'Regan Arts', 44),
(N'4567      ', 2, N'XYZ', N'Abc', 1.00, NULL, N'2019', N'  ', N'', 0),
(N'5678      ', 4, N'AA BB CC', N'Dd Ee', 21.00, NULL, N'2021', N'  ', N'', 0),
(N'xxxxxxxxxx', 4, N'Unity Virtual Reality Projects', N'Jonathan Linowes', 31.99, 1, N'2015', N'1 ', N'Packt Publishing', 33);

-------------------------------------------------------
-- 6. ORDERS
-------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Orders] ON;

INSERT INTO [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [Status], [DiscountPercent]) VALUES
(1, 4, N'2015-09-18 09:23:58', N'P', 0),
(4, 2, N'2015-09-18 10:03:00', N'P', 50),
(7, 2, N'2015-09-18 22:25:26', N'P', 10),
(8, 2, N'2015-09-18 22:29:01', N'P', 20),
(9, 4, N'2015-09-18 22:31:00', N'P', 30),
(11, 2, N'2017-09-27 15:55:32', N'P', 20),
(13, 2, N'2017-10-05 14:36:49', N'P', 20),
(14, 2, N'2017-12-20 22:46:44', N'P', 50),
(15, 6, N'2018-06-07 09:18:39', N'P', 10),
(16, 5, N'2018-06-07 16:48:05', N'P', 100),
(17, 5, N'2018-06-08 10:28:54', N'P', 10),
(18, 1, N'2018-09-11 14:48:23', N'P', 10),
(19, 1, N'2018-09-12 15:10:41', N'P', 10),
(20, 1, N'2018-11-03 09:40:03', N'P', 10),
(21, 5, N'2018-11-04 22:36:47', N'P', 10),
(22, 5, N'2018-11-06 10:25:20', N'P', 10),
(23, 4, N'2018-12-23 21:43:38', N'P', 100),
(24, 4, N'2018-12-23 21:54:05', N'P', 10),
(25, 4, N'2018-12-23 22:11:39', N'P', 10),
(26, 4, N'2018-12-23 22:16:02', N'P', 10),
(27, 2, N'2020-07-14 19:56:08', N'P', 100);

SET IDENTITY_INSERT [dbo].[Orders] OFF;

-------------------------------------------------------
-- 7. ORDERITEM
-------------------------------------------------------
INSERT INTO [dbo].[OrderItem] ([OrderID], [ISBN], [Quantity]) VALUES
(4, N'1617290890', 1),
(4, N'161729232X', 2),
(8, N'1852333944', 2),
(9, N'1554683831', 1),
(11, N'1852333944', 2),
(13, N'0321146530', 1),
(14, N'161729134X', 2),
(15, N'0321146530', 2),
(15, N'1617290890', 1),
(16, N'0135974445', 1),
(16, N'073561993X', 2),
(17, N'1285096339', 2),
(18, N'0321146530', 1),
(19, N'1285096339', 1),
(20, N'1491922834', 1),
(20, N'1617290890', 2),
(21, N'0321146530', 11),
(21, N'1118026241', 11),
(21, N'1234345   ', 11),
(22, N'1491922834', 1),
(23, N'1443452483', 1),
(24, N'1491922834', 1),
(25, N'1118026241', 1),
(26, N'1118026241', 1),
(27, N'073561993X', 1);