using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareCar.Data;
using ShareCar.Models;

namespace ShareCar.Controllers;

public class HomeController : Controller
{
    private readonly ShareCarDBContext _car;

        public HomeController(ShareCarDBContext car){
            _car = car;
        }

        public IActionResult Index(){
            
            var model = new CarHomeModel
            {
                CarShare = _car.tbl_Cars.ToList(),       // Danh sách xe cho thuê
                CarSell = _car.tbl_CarSell.ToList()      // Danh sách xe bán
            };
            return View(model);
        }

        [HttpGet("CarShare")]
        public IActionResult CarShare()
        {
            var carShare = _car.tbl_Cars.ToList(); // Lấy danh sách xe cho thuê
            return PartialView("CarShare", carShare);  // Trả về PartialView "_CarRental" với dữ liệu
        }

        [HttpGet("CarSell")]
        public IActionResult CarSell()
        {
            var carSell = _car.tbl_CarSell.ToList(); // Lấy danh sách xe cho thuê
            return PartialView("CarSell", carSell);  // Trả về PartialView "_CarRental" với dữ liệu
        }

        [HttpGet("Home/CarSearch")]
        public async Task<IActionResult> CarSearch(string query, string Status, string VehicleRegistration,string Days, string Option) {
            var shareCar = await _car.tbl_Cars.ToListAsync();
            var sellCar = await _car.tbl_CarSell.ToListAsync();

            // Kiểm tra tùy chọn "Thuê Xe/Mua Xe"
            if (Option == "Thuê Xe" || Option == "Tất cả") {
                if (!string.IsNullOrEmpty(query))
                {
                    shareCar = shareCar
                        .Where(c => c.Model.ToLower().Contains(query.ToLower()) || c.Brand.ToLower().Contains(query.ToLower()) || c.CarID.ToString().ToLower().Contains(query.ToLower()))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(Status) && Status != "Tất cả trạng thái") {
                    shareCar = shareCar.Where(c => c.Status == Status).ToList();
                }

                if (!string.IsNullOrEmpty(Days) && Days != "Tất cả thời gian") {
                    DateTime now = DateTime.Now;
                    switch (Days) {
                        case "Hôm nay":
                                // Kiểm tra các bài đăng trong ngày hiện tại
                                shareCar = shareCar.Where(c => c.Day.Date == now.Date).ToList();
                                break;
                            case "1 ngày trước":
                                shareCar = shareCar.Where(c => (now - c.Day).TotalDays <= 1).ToList();
                                break;
                            case "3 ngày trước":
                                shareCar = shareCar.Where(c => (now - c.Day).TotalDays <= 3).ToList();
                                break;
                            case "7 ngày trước":
                                shareCar = shareCar.Where(c => (now - c.Day).TotalDays <= 7).ToList();
                                break;
                            case "1 tháng trước":
                                shareCar = shareCar.Where(c => (now - c.Day).TotalDays <= 30).ToList();
                                break;
                            case "1 năm trước":
                                shareCar = shareCar.Where(c => (now - c.Day).TotalDays <= 365).ToList();
                                break;
                    }
                }
            }

            if (Option == "Mua Xe" || Option == "Tất cả") {
                if (!string.IsNullOrEmpty(query))
                {
                    sellCar = sellCar
                        .Where(c => c.Model.ToLower().Contains(query.ToLower()) || c.Brand.ToLower().Contains(query.ToLower()) || c.CarID.ToString().ToLower().Contains(query.ToLower()))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(VehicleRegistration) && VehicleRegistration != "Tất cả") {
                    sellCar = sellCar.Where(c => c.VehicleRegistration == VehicleRegistration).ToList();
                }

                if (!string.IsNullOrEmpty(Days) && Days != "Tất cả thời gian") {
                    DateTime now = DateTime.Now;
                    switch (Days) {
                        case "Hôm nay":
                                // Kiểm tra các bài đăng trong ngày hiện tại
                                sellCar = sellCar.Where(c => c.Day.Date == now.Date).ToList();
                                break;
                            case "1 ngày trước":
                                sellCar = sellCar.Where(c => (now - c.Day).TotalDays <= 1).ToList();
                                break;
                            case "3 ngày trước":
                                sellCar = sellCar.Where(c => (now - c.Day).TotalDays <= 3).ToList();
                                break;
                            case "7 ngày trước":
                                sellCar = sellCar.Where(c => (now - c.Day).TotalDays <= 7).ToList();
                                break;
                            case "1 tháng trước":
                                sellCar = sellCar.Where(c => (now - c.Day).TotalDays <= 30).ToList();
                                break;
                            case "1 năm trước":
                                sellCar = sellCar.Where(c => (now - c.Day).TotalDays <= 365).ToList();
                                break;
                    }
                }
            }
            HttpContext.Session.SetString("Option",Option);
            var model = new CarHomeModel
            {
                CarShare = shareCar,
                CarSell = sellCar
            };

            return View("CarSearch", model);
        }


}
