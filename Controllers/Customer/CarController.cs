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
using ShareCar.Models.Home.CarModel;

namespace ShareCar.Controllers.Customer
{
    public class CarController : Controller
    {
       

        string listcar=  "~/Views/Customer/Car/CarUser.cshtml";
        string carshareuser=  "~/Views/Customer/Car/CarShareUser.cshtml";
        string carselluser=  "~/Views/Customer/Car/CarSellUser.cshtml";

        string carshareuseredit=  "~/Views/Customer/Car/CarUserShareEdit.cshtml";
        string carselluseredit=  "~/Views/Customer/Car/CarUserSellEdit.cshtml";
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

            var model = new CarHomeModel{
                CarShareUser = carshare,
                CarSellUser = carsell
            };

            return View(listcar, model);
        }

        //Xóa xe Thuê User
        [HttpDelete]
        public IActionResult Delete(int id) {
            var car = _car.tbl_CarShare.Find(id);
            if (car != null) {
                _car.tbl_CarShare.Remove(car);
                _car.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        //Xóa xe bán User
        [HttpDelete]
        public IActionResult DeleteCarSell(int id) {
            var car = _car.tbl_CarSell.Find(id);
            if (car != null) {
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
                    Console.WriteLine("Error :" +error.ErrorMessage);
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


    }
}