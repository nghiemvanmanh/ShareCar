Create table tblUser(
	Id INT Primary key identity(1,1),
	UserName NVARCHAR(255) Not null unique,
	PassWord NVARCHAR(255),
	Email NVARCHAR(255) check ( Email LIKE '%_@__%.__%' ), 
	SDT Varchar(10) CHECK (SDT LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	FullName Nvarchar(max) not null,
	Address Nvarchar(max) not null,
)

Insert into tblUser(UserName,PassWord,Email,SDT,FullName, Address) values ('admin','2003','admincar@admin.com','0999999999',N'Nghiêm Văn Mạnh',N'Hà Nội')

Create table tbl_Cars(
	CarID Int primary key identity(1,1),
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
use ProjectCar
drop table tbl_Cars

insert into tbl_Cars(Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values (N'Quản trị viên','30E-99999','Mercedes-Benz',N'Mercedes G63 AMG','White',N'Chưa cho thuê',N'Bộ la-zăng thể thao 5 chấu, 20 inch. Cửa sổ lớn, kính màu cách nhiệt. Cặp ống xả thể thao AMG ở sườn bên trái.','2024-11-02',10000000,N'Đại từ, Hà Nội','0999999999','https://giaxemercedes.vn/wp-content/uploads/2021/09/Mercedes-g63-amg-giaxemercedes-vn-12.jpg','4.5')

insert into tbl_Cars(Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values (N'Quản trị viên','29E-30174','Mercedes-Benz',N'Maybach S680 4Matic','Black',N'Chưa cho thuê',N'Mercedes-Maybach S 680 4Matic 2024 là sản phẩm đẳng cấp nhất của Mercedes toàn cầu, được thiết kế để trở nên độc quyền và đỉnh cao công nghệ hơn cả mẫu Maybach S-Class tiêu chuẩn.','2024-11-02',5000000,N'Ngã Tư Sở, Hà Nội','0999999999','https://static-images.vnncdn.net/files/publish/2022/12/1/danh-gia-mercedes-maybach-s680-vietnamnet-16-1919.jpg','4')
insert into tbl_Cars(Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values (N'Quản trị viên','30E-04953','Toyota',N'Camry 2.0Q','Red',N'Đã cho thuê',N'Bộ vành 18 inch cùng với hai ống xả đặt cân đối phía đuôi xe trên bản 2.0Q tạo nên cảm giác như các dòng xe thể thao thường thấy.','2024-10-12',2000000,N'Láng Hạ, Hà Nội','0999999999','https://banxemoi.com.vn/wp-content/uploads/2019/04/Toyota-Camry-2019_04.jpg','4')
insert into tbl_Cars(Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values (N'Quản trị viên','30E-12345','Honda',N'CR-V 2.0L e:HEV RS','White',N'Chưa cho thuê',N'Cảm tác từ sự mạnh mẽ của chiếc xe SUV đô thị đẳng cấp, Honda CR-V 2024 sở hữu kiểu dáng thể thao cao cấp và tinh tế hoàn toàn mới, khơi dậy khí chất uy phong của chủ nhân và mang đến cảm giác mãn nhãn đầy cuốn hút.','2024-09-15',1000000,N'Cầu Giấy, Hà Nội','0999999999','https://drive.gianhangvn.com/image/99lbvbw-2524733j32511.jpg','5')
insert into tbl_Cars(Poster,LicensePlate,Brand,Model,Color,Status,Description,Day,RentalPrice,Address,SDT,Image,AverageRating) values (N'Quản trị viên','33R5-7774','Honda',N' Wave Alpha','Black',N'Đã cho thuê',N'Xe mình mua từ năm 2023, hoạt động êm ru. Do nhu cầu sử dụng ít nên mình cho thuê lại.','2024-11-04',150000,N'Quận 2, TP HCM','0999999999','https://files01.danhgiaxe.com/2Pvn-ctu4_52I0jFqqqJoYqOi-k=/fit-in/1280x0/20230107/honda-wave-alpha-110-3-201701.jpg','5')