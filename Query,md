# Library
Project

CREATE TABLE LoginData(
	Id INT IDENTITY PRIMARY KEY,
	[Email] NVARCHAR(max) not null CHECK([Email] != ''),
	[Login] NVARCHAR(max),
	[Password] NVARCHAR(max) not null CHECK([Password] != '')
)

CREATE TABLE SubscriptionsNames(
	Id INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(50) not null CHECK([Name] != '')
)

CREATE TABLE Subscriptions(
	Id INT IDENTITY PRIMARY KEY,
	[SubscriptionsNameId] INT not null FOREIGN KEY REFERENCES SubscriptionsNames(Id),
	[ValidUntil] SMALLDATETIME not null
)

CREATE TABLE Persons(
	Id INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(50) not null CHECK([Name] != ''),
	[Surname] NVARCHAR(50) not null CHECK([Surname] != '')	
)

CREATE TABLE Users(
	Id INT IDENTITY PRIMARY KEY,
	[PersonId] INT not null FOREIGN KEY REFERENCES Persons(Id),
	[LoginDataId] INT not null FOREIGN KEY REFERENCES LoginData(Id),
	[SubscriptionsId] INT not null FOREIGN KEY REFERENCES Subscriptions(Id),
	[Image] IMAGE,
	[Address] NVARCHAR(max) not null,
	[Phone] CHAR(10) CHECK([Phone] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	[Money] MONEY not null,
	[AdvancedAccess] BIT NULL DEFAULT(0)
)

CREATE TABLE Authors(
	Id INT IDENTITY PRIMARY KEY,
	[PersonId] INT not null FOREIGN KEY REFERENCES Persons(Id)
)

CREATE TABLE BookCategories(
	Id INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(50) not null
)

CREATE TABLE Books(
	Id INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(50) not null CHECK([Name] != ''),
	[Image] IMAGE not null,
	[AuthorId] INT not null FOREIGN KEY REFERENCES Authors(Id),
	[BookCategoryId] INT not null FOREIGN KEY REFERENCES BookCategories(Id),
	[YearEdition] INT not null CHECK([YearEdition] > 0 and [YearEdition] < 2021)
)

CREATE TABLE LastAddedBook(
	Id INT IDENTITY NOT NULL,
	[BooksId] INT not null FOREIGN KEY REFERENCES Books(Id),
	[ShortDescription] NVARCHAR(max) NOT NULL
)

CREATE TABLE ReceivedBooks(
	Id INT IDENTITY PRIMARY KEY,
	[BookId] INT not null FOREIGN KEY REFERENCES Books(Id),
	[UserId] INT not null FOREIGN KEY REFERENCES Users(Id),
	[ReceivingDate] SMALLDATETIME not null,
	[DeliveryDate] SMALLDATETIME not null
)

INSERT SubscriptionsNames([Name]) VALUES
	('Default'),('Silver'),('Gold'),('Diamond')

INSERT BookCategories([Name]) VALUES
	('Horror'),('Fantasy')



  CREATE PROC SelectUserByLoginDataEmail @Email NVARCHAR(max),@Password NVARCHAR(max) AS 
  SELECT Users.Id FROM Users INNER JOIN  LoginData ON LoginData.Id = Users.LoginDataId 
  WHERE LoginData.Email = @Email and LoginData.Password = @Password


  CREATE PROC SelectUserByLoginDataLogin @Login NVARCHAR(max),@Password NVARCHAR(max) AS 
  SELECT Users.Id FROM Users INNER JOIN  LoginData ON LoginData.Id = Users.LoginDataId 
  WHERE LoginData.Login = @Login and LoginData.Password = @Password



  execute SelectUserByLoginDataEmail
