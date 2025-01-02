Create table tblUser(
	Id INT Primary key identity(1,1),
	UserName NVARCHAR(255) Not null unique,
	PassWord NVARCHAR(255),
	Email NVARCHAR(255) check ( Email LIKE '%_@__%.__%' ), 
	SDT Varchar(10) CHECK (SDT LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	FullName Nvarchar(max) not null,
	Address Nvarchar(max) not null,
)

alter table tblUser add VerifyKey nvarchar(max) null

Insert into tblUser(UserName,PassWord,Email,SDT,FullName, Address) values ('admin','2003','admincar@admin.com','0999999999',N'Nghiêm Văn Mạnh',N'Hà Nội')

Create table tbl_CarShare(
	CarID Int primary key identity(1,1),
	UserID Int not null,
	Poster nvarchar(max) ,
	LicensePlate Nvarchar(20) not null ,
	Brand Nvarchar(255) not null,
	Model nvarchar(255) not null,
	Color nvarchar(20) not null,
	Status nvarchar(50) not null,
	Description Nvarchar(max) ,
	Day Datetime ,
	Address Nvarchar(max),
	SDT nvarchar(12) not null,
	RentalPrice Float not null,
	Image nvarchar(max),
	AverageRating Float,
	Logo nvarchar(max)
)

insert into tbl_CarShare(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values ('/images/carlogo/mercedes.jpg',1,N'Quản trị viên','30E-99999','Mercedes',N'G63 AMG','White',N'Chưa cho thuê',N'Bộ la-zăng thể thao 5 chấu, 20 inch. Cửa sổ lớn, kính màu cách nhiệt. Cặp ống xả thể thao AMG ở sườn bên trái.','2024-11-02',10000000,N'Đại từ, Hà Nội','0999999999','https://giaxemercedes.vn/wp-content/uploads/2021/09/Mercedes-g63-amg-giaxemercedes-vn-12.jpg','4.5')
insert into tbl_CarShare(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values ('/images/carlogo/mercedes.jpg',1,N'Quản trị viên','29E-30174','Mercedes',N'Maybach S680 4Matic','Black',N'Chưa cho thuê',N'Mercedes-Maybach S 680 4Matic 2024 là sản phẩm đẳng cấp nhất của Mercedes toàn cầu, được thiết kế để trở nên độc quyền và đỉnh cao công nghệ hơn cả mẫu Maybach S-Class tiêu chuẩn.','2024-11-02',5000000,N'Ngã Tư Sở, Hà Nội','0999999999','https://static-images.vnncdn.net/files/publish/2022/12/1/danh-gia-mercedes-maybach-s680-vietnamnet-16-1919.jpg','4')
insert into tbl_CarShare(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values ('/images/carlogo/toyota.jpg',1,N'Quản trị viên','30E-04953','Toyota',N'Camry 2.0Q','Red',N'Đã cho thuê',N'Bộ vành 18 inch cùng với hai ống xả đặt cân đối phía đuôi xe trên bản 2.0Q tạo nên cảm giác như các dòng xe thể thao thường thấy.','2024-10-12',2000000,N'Láng Hạ, Hà Nội','0999999999','https://banxemoi.com.vn/wp-content/uploads/2019/04/Toyota-Camry-2019_04.jpg','4')
insert into tbl_CarShare(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values ('/images/carlogo/honda.jpg',1,N'Quản trị viên','30E-12345','Honda',N'CR-V 2.0L e:HEV RS','White',N'Chưa cho thuê',N'Cảm tác từ sự mạnh mẽ của chiếc xe SUV đô thị đẳng cấp, Honda CR-V 2024 sở hữu kiểu dáng thể thao cao cấp và tinh tế hoàn toàn mới, khơi dậy khí chất uy phong của chủ nhân và mang đến cảm giác mãn nhãn đầy cuốn hút.','2024-09-15',1000000,N'Cầu Giấy, Hà Nội','0999999999','https://drive.gianhangvn.com/image/99lbvbw-2524733j32511.jpg','5')
insert into tbl_CarShare(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values ('/images/carlogo/honda.jpg',1,N'Quản trị viên','33R5-7774','Honda',N' Wave Alpha','Black',N'Đã cho thuê',N'Xe mình mua từ năm 2023, hoạt động êm ru. Do nhu cầu sử dụng ít nên mình cho thuê lại.','2024-11-04',150000,N'Quận 2, TP HCM','0999999999','https://files01.danhgiaxe.com/2Pvn-ctu4_52I0jFqqqJoYqOi-k=/fit-in/1280x0/20230107/honda-wave-alpha-110-3-201701.jpg','5')


Create table tbl_CarSell(
	CarID Int primary key identity(1,1),
	UserID Int not null,
	Poster nvarchar(max) ,
	LicensePlate Nvarchar(20) not null ,
	Brand Nvarchar(255) not null,
	Model nvarchar(255) not null,
	Color nvarchar(20) not null,
	VehicleRegistration Nvarchar(max) not null,
	Description Nvarchar(max) ,
	Day Datetime ,
	Address Nvarchar(max),
	SDT nvarchar(12) not null,
	SellPrice Float not null,
	Image nvarchar(max),
	Logo nvarchar(max),
)


insert into tbl_CarSell(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,VehicleRegistration,Description,Day,SellPrice,Address,SDT,Image ) values ('/images/carlogo/mitsubishi.jpg',1,N'Quản trị viên','30E-23459','Mitsubishi',N'Xpander 2023',N'Trắng',N'Đầy đủ',N'Xpander Cross 2023. LH:0999999999
- Camera toàn cảnh 360
- Camera hành trình
- Cảm biến lùi
- Gạt mưa tự động, đèn tự động có điều chỉnh độ cao đèn
- Có trang bị chức năng cruise control (ga tự động thích ứng)
- Vietmap live cảnh báo tốc độ, chỉ đường
- Hệ thống giải tri: apple car play, androi auto, bluetooth
- Xe gia đình sạch sẽ, rỗng rãi cho 7 người lớn
- Trang bi thêm sấy gương khi đi trời mưa
- Có đầy đủ bảo hiểm thân vỏ, bảo hiểm người ngồi trên xe, bảo hiểm TNDS
- Có thu phí không dừng VETC
- Giao nhận xe 24/7','2024-11-02',350000000,N'Đại từ, Hà Nội','0999999999','https://xehoithongminh.vn/wp-content/uploads/2022/06/danh-gia-Mitsubishi-Xpander-2022.jpg' )

insert into tbl_CarSell(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,VehicleRegistration,Description,Day,SellPrice,Address,SDT,Image ) values ('/images/carlogo/toyota.jpg',1,N'Quản trị viên','51K-46413','Toyota ',N'Veloz Cross 2022',N'Trắng',N'Đầy đủ',N'Veloz Cross 2022. LH:0999999999
- Camera toàn cảnh 360
- Camera hành trình
- Cảm biến lùi
- Gạt mưa tự động, đèn tự động có điều chỉnh độ cao đèn
- Có trang bị chức năng cruise control (ga tự động thích ứng)
- Vietmap live cảnh báo tốc độ, chỉ đường
- Hệ thống giải tri: apple car play, androi auto, bluetooth
- Xe gia đình sạch sẽ, rỗng rãi cho 7 người lớn
- Trang bi thêm sấy gương khi đi trời mưa
- Có đầy đủ bảo hiểm thân vỏ, bảo hiểm người ngồi trên xe, bảo hiểm TNDS
- Có thu phí không dừng VETC
- Giao nhận xe 24/7','2024-11-04',250000000,N'Cầu Giấy, Hà Nội','0999999999','https://i1-vnexpress.vnecdn.net/2022/03/23/DSCF15692JPG-1648011323.jpg?w=750&h=450&q=100&dpr=1&fit=crop&s=UJzJzAVEG3-zcgaVf2dSAQ' )

insert into tbl_CarSell(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,VehicleRegistration,Description,Day,SellPrice,Address,SDT,Image ) values ('/images/carlogo/vinfast.jpg',1,N'Quản trị viên','71A-11829','Vinfast',N'Fadil 2021',N'Đỏ',N'Đầy đủ',N'Vinfast Fadil 2022-25.000 km. Xe gia đình.
Đã lên đèn Bi led đi đêm. Màn hình giải trí Bravigo. Có ghế em bé hiệu Combi','2024-10-17',200000000,N'Bến Tre','0999999999','https://bizweb.dktcdn.net/100/446/720/products/z4226766302440-c2a94e4818dd00ee3ada4a04176a45a2.jpg?v=1682407353683' )

insert into tbl_CarSell(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,VehicleRegistration,Description,Day,SellPrice,Address,SDT,Image ) values ('/images/carlogo/kia.jpg',1,N'Quản trị viên','51L-01673','KIA Morning ',N'CarenS 2023',N'Đen',N'Đầy đủ',N'KIA CarenS. LH:0999999999
- Camera toàn cảnh 360
- Camera hành trình
- Cảm biến lùi
- Gạt mưa tự động, đèn tự động có điều chỉnh độ cao đèn
- Có trang bị chức năng cruise control (ga tự động thích ứng)
- Vietmap live cảnh báo tốc độ, chỉ đường
- Xe gia đình sạch sẽ, rỗng rãi cho 7 người lớn
- Có đầy đủ bảo hiểm thân vỏ, bảo hiểm người ngồi trên xe, bảo hiểm TNDS
- Có thu phí không dừng VETC
- Giao nhận xe 24/7','2024-10-28',280000000,N'Bình Thạnh, TP.Hồ Chí Minh','0999999999','https://cdn.tuoitrethudo.vn/stores/news_dataimages/2023/062023/27/22/xehay-carens-240623-2-result20230627223349.0025740.jpg' )

insert into tbl_CarSell(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,VehicleRegistration,Description,Day,SellPrice,Address,SDT,Image ) values ('/images/carlogo/Honda.jpg',1,N'Quản trị viên','51H-85817','Honda ',N'Brio 2021',N'Đỏ',N'Đầy đủ',N'Brio 2021. LH:0999999999
- Camera toàn cảnh 360
- Camera hành trình
- Cảm biến lùi
- Gạt mưa tự động, đèn tự động có điều chỉnh độ cao đèn
- Có trang bị chức năng cruise control (ga tự động thích ứng)
- Vietmap live cảnh báo tốc độ, chỉ đường
- Xe gia đình sạch sẽ, rỗng rãi cho 7 người lớn
- Trang bi thêm sấy gương khi đi trời mưa
- Có đầy đủ bảo hiểm thân vỏ, bảo hiểm người ngồi trên xe, bảo hiểm TNDS
- Giao nhận xe 24/7','2024-11-05',400000000,N'Bình Thạnh, TP.Hồ Chí Minh','0999999999','https://cdn.24h.com.vn/upload/3-2021/images/2021-09-04/Gia-xe-Honda-Brio-lan-banh-thang-9-2021-15-1630769510-539-width660height440.jpg' )

insert into tbl_CarSell(Logo,UserID,Poster,LicensePlate,Brand,Model,Color,VehicleRegistration,Description,Day,SellPrice,Address,SDT,Image ) 
values ('/images/carlogo/honda.jpg',1,N'Quản trị viên','29Y7-09361','Honda',N'Air Blade 160 ABS',N'Đen',N'Không đầy đủ',N'Xe mình mua năm 2022 bị mất giấy tờ xe. Mình không còn nhu cầu sử dụng nên bán rẻ lại','2024-10-17',15000000,N'Tây Hồ, Hà Nội','0999999999'
,'https://giadinh.mediacdn.vn/296230595582509056/2024/4/1/gia-xe-honda-air-blade-5-1702742479-1711933692317-171193369245196920274.jpg' )


Create table tbl_CarShareQueue(
	CarID Int primary key identity(1,1),
	UserID Int not null,
	Poster nvarchar(max) ,
	LicensePlate Nvarchar(20) not null ,
	Brand Nvarchar(255) not null,
	Model nvarchar(255) not null,
	Color nvarchar(20) not null,
	Status nvarchar(50) not null,
	Description Nvarchar(max) ,
	Day Datetime ,
	Address Nvarchar(max),
	SDT nvarchar(12) not null,
	RentalPrice Float not null,
	Image nvarchar(max),
	AverageRating Float
)

Create table tbl_CarSellQueue(
	CarID Int primary key identity(1,1),
	UserID Int not null,
	Poster nvarchar(max) ,
	LicensePlate Nvarchar(20) not null ,
	Brand Nvarchar(255) not null,
	Model nvarchar(255) not null,
	Color nvarchar(20) not null,
	VehicleRegistration Nvarchar(max) not null,
	Description Nvarchar(max) ,
	Day Datetime ,
	Address Nvarchar(max),
	SDT nvarchar(12) not null,
	SellPrice Float not null,
	Image nvarchar(max),
)

Create table tbl_Comment(
	CommentID int  primary key identity(1,1),
	CarID int not null,
	PosterName nvarchar(max) not null,
	DateUp Datetime,
	CommentText nvarchar(max),
	NameComment nvarchar(max),
	FOREIGN KEY (CarID) REFERENCES tbl_CarShare(CarID)
)


use ProjectCar

Create table tbl_CarShareFavorite(
	ID_Share int primary key identity(1,1),
	CarID Int not null,
	UserID Int not null,
	Poster nvarchar(max) ,
	LicensePlate Nvarchar(20) not null ,
	Brand Nvarchar(255) not null,
	Model nvarchar(255) not null,
	Color nvarchar(20) not null,
	Status nvarchar(50) not null,
	Description Nvarchar(max) ,
	Day Datetime ,
	Address Nvarchar(max),
	SDT nvarchar(12) not null,
	RentalPrice Float not null,
	Image nvarchar(max),
	AverageRating Float,
	Logo nvarchar(max)
)

Create table tbl_CarSellFavorite(
	ID_Sell  Int primary key identity(1,1),
	CarID Int not null,
	UserID Int not null,
	Poster nvarchar(max) ,
	LicensePlate Nvarchar(20) not null ,
	Brand Nvarchar(255) not null,
	Model nvarchar(255) not null,
	Color nvarchar(20) not null,
	VehicleRegistration Nvarchar(max) not null,
	Description Nvarchar(max) ,
	Day Datetime ,
	Address Nvarchar(max),
	SDT nvarchar(12) not null,
	SellPrice Float not null,
	Image nvarchar(max),
	Logo nvarchar(max),
)