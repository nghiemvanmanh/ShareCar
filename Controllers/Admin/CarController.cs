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

namespace ShareCar.Controllers.Admin
{

    public class CarController : Controller
    {
        string carmanager = "~/Views/Admin/Car/CarManager.cshtml";
        string caresharedit = "~/Views/Admin/Car/CarShareEdit.cshtml";
        string careselldit = "~/Views/Admin/Car/CarSellEdit.cshtml";
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ShareCarDBContext _car;
        public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
        {
            _hostingEnvironment = hostingEnvironment;
            _car = car;
        }

        string carshare = "~/Views/Admin/Car/CarShareManager.cshtml";
        string carsell = "~/Views/Admin/Car/CarSellManager.cshtml";

        //Hiển thị danh sách xe thuê quản lý
        [HttpGet("Car/CarShareManager")]
        public IActionResult CarShareManager()
        {
            var carShare = _car.tbl_CarShare.ToList(); // Lấy danh sách xe cho thuê
            return PartialView(carshare, carShare);
        }


        //Hiển thị danh sách xe bán quản lý
        [HttpGet("Car/CarSellManager")]
        public IActionResult CarSellManager()
        {
            var carSell = _car.tbl_CarSell.ToList(); // Lấy danh sách xe cho thuê
            return PartialView(carsell, carSell);  // Trả về PartialView "_CarRental" với dữ liệu
        }

        //Hiển thị tất cả xe quản lý
        [HttpGet("Car/CarManager")]
        public async Task<IActionResult> CarManager(string Option, string searchString, string days)
        {
            var carShare = await _car.tbl_CarShare.ToListAsync(); // Lấy danh sách tất cả xe
            var carSell = await _car.tbl_CarSell.ToListAsync();

            var model = new CarHomeModel
            {
                CarShareManager = carShare,
                CarSellManager = carSell
            };
            return View(carmanager, model);
        }

        // Action để hiển thị form chỉnh sửa xe thuê
        [HttpGet("Car/CarShareEdit/{id}")]
        public IActionResult CarShareEdit(int id)
        {
            var car = _car.tbl_CarShare.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(caresharedit, car); // Truyền model vào view
        }

        // Action để xử lý cập nhật xe thuê
        [HttpPost]
        public async Task<IActionResult> CarShareEdit(CarShareModel models)
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
                // Kiểm tra nếu tồn tại sản phẩm trong cơ sở dữ liệu
                _car.Update(models);
                await _car.SaveChangesAsync();
                TempData["SuccessCarManager"] = "Cập nhật thành công!";
                return RedirectToAction("CarManager", "Car");
            }
            return View(caresharedit, models); // Nếu không hợp lệ, quay lại view với dữ liệu đã nhập
        }

        // Action để hiển thị form chỉnh sửa carSell
        [HttpGet("Car/CarSellEdit/{id}")]
        public IActionResult CarSellEdit(int id)
        {
            var car = _car.tbl_CarSell.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(careselldit, car); // Truyền model vào view
        }

        // Action để xử lý cập nhật carSell
        [HttpPost]
        public async Task<IActionResult> CarSellEdit(CarSellModel models)
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
                TempData["SuccessCarManager"] = "Cập nhật thành công!";
                return RedirectToAction("CarManager", "Car");
            }

            return View(careselldit, models); // Nếu không hợp lệ, quay lại view với dữ liệu đã nhập
        }

        //Xóa xe bán
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

        //Xóa xe cho thuê
        public IActionResult DeleteCarShare(int id)
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
    }
}