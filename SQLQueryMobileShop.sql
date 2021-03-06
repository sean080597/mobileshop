create database DtMobileShop
go
use DtMobileShop
go
create table Roles(
	RoleId int identity(1,1),
	RoleName nvarchar(50) not null,
	constraint PK_Roles primary key (RoleId)
)
go
create table FeeShip(
	PlaceName nvarchar(50) not null,
	Fee decimal (18,0) check (Fee >=0),
	constraint PK_FeeShip primary key (PlaceName)
)
go
create table ProductType(
	TypeId int identity (1,1),
	TypeName nvarchar(50) not null,
	constraint PK_ProductType primary key (TypeId)	
)
go
create table Brand(
	BrandId int identity (1,1),
	BrandName nvarchar(50) not null,
	constraint PK_Brand primary key (BrandId)
)
go
create table Users(
	UserId int identity(1,1),
	UserName nvarchar(50) not null,
	UserAccount varchar(50) unique,
	UserPassword varchar(50) not null,
	UserAddress nvarchar(200),
	NPhone varchar(20),
	Email varchar(50) not null,
	CMND varchar(20),
	IdRole int,
	constraint PK_Users	primary key (UserId),
	constraint FK_Users foreign key	(IdRole) references Roles (RoleId)
)
go
create table Product(
	ProductId int identity(1,1),
	ProductName nvarchar(50) not null,
	Detail ntext,
	Sale ntext,
	ImageProduct varchar(50),
	Screen nvarchar(200),
	CPU nvarchar(200),
	UpdateDate datetime,
	Quantity int,
	TypeId int,
	BrandId int,
	constraint PK_Product primary key (ProductId),
	constraint FK_ProductType foreign key	(TypeId) references ProductType (TypeId),
	constraint FK_Brand foreign key	(BrandId) references Brand (BrandId)
)
go
create table ProductColor(
	ColorId int identity (1,1),
	ColorName nvarchar(50) not null,
	Price decimal (18,0) check (Price >=0),
	ImageColor nvarchar(200),
	ProductId int,
	constraint PK_ProductColor primary key (ColorId),
	constraint FK_ProductColor foreign key	(ProductId) references Product (ProductId)
)
go
create table OrderProduct(
	OrderingId int identity(1,1),
	CheckOut bit,
	OrderDay datetime,
	DeliveryDay datetime,
	PlaceName nvarchar(50),
	UserId int,
	constraint PK_OrderProduct primary key (OrderingId),
	constraint FK_FeeShip foreign key(PlaceName) references FeeShip (PlaceName),
	constraint FK_Users1 foreign key(UserId) references Users (UserId),
)
go
create table LineItem(
	OrderingId int not null,
	ColorId int not null,
	ProductId int not null,
	quantity int,
	Price decimal(18,0),
	Total decimal(18,0),
	primary key(OrderingId, ColorId),
	Foreign key (OrderingId) references OrderProduct(OrderingId),
	Foreign key (ColorId) references ProductColor(ColorId),
	Foreign key (ProductId) references Product(ProductId)
)
go
Insert into Roles values ('Admin');
Insert into Roles values ('Customer');
Insert into FeeShip values (N'hello', 1);
Insert into ProductType values ('Phone');
Insert into ProductType values ('Tablet');
Insert into Brand values ('Iphone');
Insert into Brand values ('Xiaomi');
Insert into Users values ('Trương Quốc Cẩm','camcam1132000','quoccam1993', 'HCM', '0101010101','cam@gmail.com','090909090',1);
Insert into Users values ('Trương Quốc Lâm','lamlam1132000','quoclam199̉', 'HCM', '0101010101','lam@gmail.com','090909090',2);
Insert into Product values ('Iphone 6','none','none','none','none','none','03/17/2018',15,1,1);
Insert into Product values ('Iphone 6 plus','none','none','none','none','none','03/17/2018',10,1,1);
Insert into ProductColor values ('Đen',10000000,'none',1);
Insert into ProductColor values ('Trắng',10000000,'none',1);
Insert into OrderProduct values (1,'03/17/2018','03/18/2018',N'Hà Nội', 2);
Insert into LineItem values (1,1,1,11000000,11000000);
