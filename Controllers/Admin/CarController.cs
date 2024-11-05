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

namespace ShareCar.Controllers.Admin
{

    public class CarController : Controller
    {
        string carmanager = "~/Views/Admin/Car/CarManager.cshtml";
        string caredit = "~/Views/Admin/Car/CarEditManager.cshtml";
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ShareCarDBContext _car;
        public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
        {
            _hostingEnvironment = hostingEnvironment;
            _car = car;
        }

        [HttpGet("Car/CarManager")]
        public async Task<IActionResult> CarManager(string searchString, string status, string days) {
            var cars = await _car.tbl_Cars.ToListAsync(); // Lấy danh sách tất cả xe

            // Tìm kiếm theo ID hoặc Brand
            if (!string.IsNullOrEmpty(searchString))
            {   
                cars = cars.Where(c => c.CarID.ToString().ToLower().Contains(searchString.ToLower()) || c.Brand.ToLower().Contains(searchString.ToLower()) || c.Model.ToLower().Contains(searchString.ToLower())).ToList();
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status) && status != "Tất cả trạng thái")
            {
                cars = cars.Where(c => c.Status == status).ToList();
            }

            // Lọc theo số ngày chênh lệch (Days)
            if (!string.IsNullOrEmpty(days) && days != "Tất cả thời gian")
            {
                int dayDifference = 0;
                switch (days)
                {
                    case "Hôm nay":
                        dayDifference = 0;
                        break;
                    case "1 ngày trước":
                        dayDifference = 1;
                        break;
                    case "3 ngày trước":
                        dayDifference = 3;
                        break;
                    case "7 ngày trước":
                        dayDifference = 7;
                        break;
                    case "1 tháng trước":
                        dayDifference = 30;
                        break;
                    case "1 năm trước":
                        dayDifference = 365;
                        break;
                }
                
                // Lọc xe dựa trên số ngày chênh lệch
                cars = cars.Where(c => (DateTime.Now - c.Day).TotalDays <= dayDifference).ToList();
            }
            return View(carmanager, cars); 
        }

        // Action để hiển thị form chỉnh sửa
        [HttpGet("Car/CarEditManager/{id}")]
        public IActionResult CarEditManager(int id)
        {
            var car = _car.tbl_Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(caredit,car); // Truyền model vào view
        }

        // Action để xử lý cập nhật
        [HttpPost("Car/CarEditManager")]
        public async Task<IActionResult> CarEdit(CarShareModel models)
        {
            if (!ModelState.IsValid) {
                // Ghi log lỗi hoặc kiểm tra thông báo lỗi
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors) {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            if (ModelState.IsValid) {
                Console.WriteLine($"Car ID: {models.CarID}");
                // Kiểm tra nếu tồn tại sản phẩm trong cơ sở dữ liệu
                _car.Update(models);
                await _car.SaveChangesAsync();
                TempData["SuccessCarManager"] = "Cập nhật thành công!";
                return RedirectToAction("CarManager","Car");
            }
            
            return View(caredit,models); // Nếu không hợp lệ, quay lại view với dữ liệu đã nhập
        }

        [HttpDelete]
        public IActionResult Delete(int id) {
            var car = _car.tbl_Cars.Find(id);
            if (car != null) {
                _car.tbl_Cars.Remove(car);
                _car.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}