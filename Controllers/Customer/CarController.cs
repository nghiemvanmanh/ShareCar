using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareCar.Data;
using ShareCar.Models;
using ShareCar.Models.Customer.CarModel;
using ShareCar.Models.Home.CarModel;

namespace ShareCar.Controllers.Customer
{
    public class CarController : Controller
    {


        string listcar = "~/Views/Customer/Car/CarUser.cshtml";
        string carshareuser = "~/Views/Customer/Car/CarShareUser.cshtml";
        string carselluser = "~/Views/Customer/Car/CarSellUser.cshtml";

        string carshareuseredit = "~/Views/Customer/Car/CarUserShareEdit.cshtml";
        string carselluseredit = "~/Views/Customer/Car/CarUserSellEdit.cshtml";

        string carsharefavorite = "~/Views/Customer/Car/CarShareFavorite.cshtml";
        string carsellfavorite = "~/Views/Customer/Car/CarSellFavorite.cshtml";
        string carfavorite = "~/Views/Customer/Car/CarFavorite.cshtml";
        private readonly ShareCarDBContext _car;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
        {
            _hostingEnvironment = hostingEnvironment;
            _car = car;
        }

        //Hiển thi dách sách xe User cho Thuê
        [HttpGet("Car/CarShareUser")]
        public IActionResult CarShareUser()
        {
            var carShare = _car.tbl_CarShare.ToList(); // Lấy danh sách xe cho thuê
            return PartialView(carshareuser, carShare);
        }


        //Hiển thị danh sách xe User đăng bán
        [HttpGet("Car/CarSellUser")]
        public IActionResult CarSellUser()
        {
            var carSell = _car.tbl_CarSell.ToList(); // Lấy danh sách xe đăng bán
            return PartialView(carselluser, carSell);
        }

        //Hiển thị tất cả danh sách xe User
        [HttpGet("Car/CarUser")]
        public async Task<IActionResult> CarUser()
        {
            var userid = HttpContext.Session.GetInt32("UserID");
            Console.WriteLine("ID: " + userid);

            // Lấy danh sách xe của người dùng
            var carshare = await _car.tbl_CarShare.Where(u => u.UserID == userid).ToListAsync();
            var carsell = await _car.tbl_CarSell.Where(u => u.UserID == userid).ToListAsync();

            var model = new CarHomeModel
            {
                CarShareUser = carshare,
                CarSellUser = carsell
            };

            return View(listcar, model);
        }

        //Xóa xe Thuê User
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var car = _car.tbl_CarShare.Find(id);
            if (car != null)
            {
                _car.tbl_CarShare.Remove(car);
                _car.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        //Xóa xe bán User
        [HttpDelete]
        public IActionResult DeleteCarSell(int id)
        {
            var car = _car.tbl_CarSell.Find(id);
            if (car != null)
            {
                _car.tbl_CarSell.Remove(car);
                _car.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        // Action để hiển thị form chỉnh sửa xe thuê
        [HttpGet("Car/CarUserShareEdit/{id}")]
        public IActionResult CarUserShareEdit(int id)
        {
            var car = _car.tbl_CarShare.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(carshareuseredit, car); // Truyền model vào view
        }

        // Action để xử lý cập nhật xe thuê
        [HttpPost]
        public async Task<IActionResult> CarUserShareEdit(CarShareModel models)
        {
            Console.WriteLine($"CarID: {models.CarID}");  // Kiểm tra giá trị của CarID
            if (!ModelState.IsValid)
            {
                // Ghi log lỗi hoặc kiểm tra thông báo lỗi
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine("Error :" + error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Car ID: {models.CarID}");
                // Kiểm tra nếu tồn tại sản phẩm trong cơ sở dữ liệu
                _car.Update(models);
                await _car.SaveChangesAsync();
                TempData["SuccessCarUser"] = "Cập nhật thành công!";
                return RedirectToAction("CarUser", "Car");
            }
            return View(carshareuseredit, models); // Nếu không hợp lệ, quay lại view với dữ liệu đã nhập
        }

        // Action để hiển thị form chỉnh sửa carSell
        [HttpGet("Car/CarUserSellEdit/{id}")]
        public IActionResult CarUserSellEdit(int id)
        {
            var car = _car.tbl_CarSell.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(carselluseredit, car); // Truyền model vào view
        }

        // Action để xử lý cập nhật carSell
        [HttpPost]
        public async Task<IActionResult> CarUserSellEdit(CarSellModel models)
        {
            if (!ModelState.IsValid)
            {
                // Ghi log lỗi hoặc kiểm tra thông báo lỗi
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Car ID: {models.CarID}");
                // Kiểm tra nếu tồn tại sản phẩm trong cơ sở dữ liệu
                _car.Update(models);
                await _car.SaveChangesAsync();
                TempData["SuccessCarUser"] = "Cập nhật thành công!";
                return RedirectToAction("CarUser", "Car");
            }

            return View(carselluseredit, models); // Nếu không hợp lệ, quay lại view với dữ liệu đã nhập
        }

        [HttpGet("Car/CarShareFavorite")]
        public IActionResult CarShareFavorite()
        {
            var carShareFavorite = _car.tbl_CarShareFavorite.ToList();
            return PartialView(carsharefavorite, carShareFavorite);
        }

        [HttpGet("Car/CarSellFavorite")]
        public IActionResult CarSellFavorite()
        {
            var carSellFavorite = _car.tbl_CarShareFavorite.ToList();
            return PartialView(carsellfavorite, carSellFavorite);
        }

        //Thêm Xe thuê yêu thích
        [HttpPost]
        public async Task<ActionResult> AddToFavorite(int carId)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            var model = _car.tbl_CarShare.FirstOrDefault(car => car.CarID == carId);
            Console.WriteLine("CarID la:" + carId);
            if (userId == null)
            {
                 return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            // Kiểm tra dữ liệu đầu vào
            if (model == null)
            {
                
                return Json(new { success = false, message = "Không tìm thấy xe!" });
                
            }

            // Kiểm tra nếu xe đã được thêm vào danh sách yêu thích
            var existingFavorite = _car.tbl_CarShareFavorite.FirstOrDefault(x => x.UserID == userId && x.CarID == model.CarID);

            if (existingFavorite != null)
            {
                return Json(new { success = false, message = "Xe đã có trong mục yêu thích." });
            }

            // Thêm xe vào danh sách yêu thích
            var newFavorite = new CarShareFavorite
            {
                CarID = model.CarID,
                UserID = userId.Value,
                LicensePlate = model.LicensePlate,
                Brand = model.Brand,
                Model = model.Model,
                Color = model.Color,
                Status = model.Status,
                Poster = model.Poster,
                Day = model.Day,
                Description = model.Description,
                SDT = model.SDT,
                Address = model.Address,
                RentalPrice = model.RentalPrice,
                Image = model.Image,
                AverageRating = model.AverageRating,
                Logo = model.Logo
            };
            _car.tbl_CarShareFavorite.Add(newFavorite);
            await _car.SaveChangesAsync();

            return Json(new { success = true, message = "Đã thêm vào mục yêu thích!" });
        }

        [HttpPost]
        public async Task<ActionResult> AddCarSellFavorite(int carId)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            var model = _car.tbl_CarSell.FirstOrDefault(car => car.CarID == carId);
            Console.WriteLine("CarID la:" + carId);
            if (userId == null)
            {
                 return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            // Kiểm tra dữ liệu đầu vào
            if (model == null)
            {
                
                return Json(new { success = false, message = "Không tìm thấy xe!" });
                
            }

            // Kiểm tra nếu xe đã được thêm vào danh sách yêu thích
            var existingFavorite = _car.tbl_CarSellFavorite.FirstOrDefault(x => x.UserID == userId && x.CarID == model.CarID);

            if (existingFavorite != null)
            {
                return Json(new { success = false, message = "Xe đã có trong mục yêu thích." });
            }

            // Thêm xe vào danh sách yêu thích
            var newFavorite = new CarSellFavorite
            {
                CarID = model.CarID,
                UserID = userId.Value,
                LicensePlate = model.LicensePlate,
                Brand = model.Brand,
                Model = model.Model,
                Color = model.Color,
                VehicleRegistration = model.VehicleRegistration,
                Poster = model.Poster,
                Day = model.Day,
                Description = model.Description,
                SDT = model.SDT,
                Address = model.Address,
                SellPrice = model.SellPrice,
                Image = model.Image,
                Logo = model.Logo
            };
            _car.tbl_CarSellFavorite.Add(newFavorite);
            await _car.SaveChangesAsync();

            return Json(new { success = true, message = "Đã thêm vào mục yêu thích!" });
        }
        
        [HttpGet("Car/CarFavorite")]
        public IActionResult CarFavorite()
        {
            int? id = HttpContext.Session.GetInt32("UserID");
            var carShareFavorite = _car.tbl_CarShareFavorite.Where(x => x.UserID == id).ToList();
            var carSellFavorite = _car.tbl_CarSellFavorite.Where(x => x.UserID == id).ToList();

            var model = new CarHomeModel
            {
                carShareFavorites = carShareFavorite,       // Danh sách xe cho thuê
                carSellFavorites = carSellFavorite    // Danh sách xe bán
            };

            return View(carfavorite, model);
        }
    }
}