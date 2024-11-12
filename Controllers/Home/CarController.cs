using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShareCar.Data;
using ShareCar.Models;
using ShareCar.Models.Home;
using ShareCar.Models.Home.CarModel;

namespace ShareCar.Controllers.Home
{

    public class CarController : Controller
    {
        private readonly ShareCarDBContext _car;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
        {
            _hostingEnvironment = hostingEnvironment;
            _car = car;
        }
        string newcarshare = "~/Views/Home/Car/CarShareNew.cshtml";
        string newcarsell= "~/Views/Home/Car/CarSellNew.cshtml";
        string carsharedetail = "~/Views/Home/Car/CarShareDetail.cshtml";
        string carselldetail = "~/Views/Home/Car/CarSellDetail.cshtml";
        string carsearch = "~/Views/Home/Car/CarSearch.cshtml";
        string addvancedsearch = "~/Views/Home/Car/AdvancedSearch.cshtml";
        string carsell = "~/Views/Home/Car/CarSellAll.cshtml";
        string carshare = "~/Views/Home/Car/CarShareAll.cshtml";
        [HttpGet("Car/CarShareNew")]
        public IActionResult CarShareNew()
        {
            return View(newcarshare);
        }
        [HttpGet("Car/CarSellNew")]
        public IActionResult CarSellNew()
        {
            return View(newcarsell);
        }

        [HttpGet("Car/CarShareAll")]
        public IActionResult CarShareAll()
        {
            var carShare = _car.tbl_CarShare.ToList(); // Lấy danh sách xe cho thuê
            int countcarshare = _car.tbl_CarShare.Count();
            ViewBag.CountCarShare = countcarshare; // Số lượng xe thuê
            return View(carshare, carShare);
        }

        [HttpGet("Car/CarSellAll")]
        public IActionResult CarSellAll()
        {
            var carShare = _car.tbl_CarSell.ToList(); // Lấy danh sách xe cho thuê
            int countcarsell = _car.tbl_CarSell.Count();
            ViewBag.CountCarSell = countcarsell; // Số lượng xe bán

            return View(carsell, carShare);
        }

        //Thêm mới xe
        [HttpPost("Car/CarShareNew")]
        public async Task<IActionResult> CarShareNew(CarShareModel car, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {

                if (imageFile != null && imageFile.Length > 0)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine("Model Error: " + error.ErrorMessage);
                            Console.WriteLine("Model Error: " + imageFile.Length);
                            Console.WriteLine("Model Error: " + Guid.NewGuid().ToString() + "_" + imageFile.FileName);
                        }
                    }
                    return View(newcarshare, car);
                }

            }
            if (ModelState.IsValid)
            {
                // car.Poster = HttpContext.Session.GetString("FullName");
                // car.SDT = HttpContext.Session.GetString("SDT");

                car.Day = DateTime.Now;
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lấy đường dẫn thư mục wwwroot/image
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/carshare");

                    // Tạo tên file duy nhất để tránh trùng lặp
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu file vào thư mục
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    car.Image = "/images/carshare/" + uniqueFileName;
                    Console.WriteLine("Url Image : " + car.Image);
                }
                Console.WriteLine("Url Image : " + car.Image);
                // Thêm car vào cơ sở dữ liệu
                _car.tbl_CarShare.Add(car);
                await _car.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(newcarshare, car);
        }

        [HttpPost("Car/CarSellNew")]
        public async Task<IActionResult> CarSellNew(CarSellModel carsell, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {

                if (imageFile != null && imageFile.Length > 0)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine("Model Error: " + error.ErrorMessage);
                            Console.WriteLine("Model Error: " + imageFile.Length);
                            Console.WriteLine("Model Error: " + Guid.NewGuid().ToString() + "_" + imageFile.FileName);
                        }
                    }
                    return View(newcarsell, carsell);
                }

            }
            if (ModelState.IsValid)
            {
                // car.Poster = HttpContext.Session.GetString("FullName");
                // car.SDT = HttpContext.Session.GetString("SDT");

                carsell.Day = DateTime.Now;
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lấy đường dẫn thư mục wwwroot/image
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/carsell");

                    // Tạo tên file duy nhất để tránh trùng lặp
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu file vào thư mục
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    carsell.Image = "/images/carsell/" + uniqueFileName;
                    Console.WriteLine("Url Image : " + carsell.Image);
                }
                Console.WriteLine("Url Image : " + carsell.Image);
                // Thêm car vào cơ sở dữ liệu
                _car.tbl_CarSell.Add(carsell);
                await _car.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(newcarsell, carsell);
        }

        [HttpGet("Car/CarSearch")]
        public async Task<IActionResult> CarSearch(string query)
        {
            var shareCar = await _car.tbl_CarShare.ToListAsync();
            var sellCar = await _car.tbl_CarSell.ToListAsync();
            if (!string.IsNullOrEmpty(query))
            {
                sellCar = sellCar
                    .Where(c => c.Model.ToLower().Contains(query.ToLower()) || c.Brand.ToLower().Contains(query.ToLower()))
                    .ToList();
                shareCar = shareCar
                    .Where(c => c.Model.ToLower().Contains(query.ToLower()) || c.Brand.ToLower().Contains(query.ToLower()))
                    .ToList();
            }
            var model = new CarHomeModel
            {
                CarShareAll = shareCar,       // Danh sách xe cho thuê
                CarSellAll = sellCar     // Danh sách xe bán
            };
            ViewBag.check = true;
            ViewBag.key = query;
            int countcarsell = sellCar.Count();
            ViewBag.CountCarSell = countcarsell; // Số lượng xe bán
            int countcarshare = shareCar.Count();
            ViewBag.CountCarShare = countcarshare; // Số lượng xe thuê
            return View(carsearch, model);
        }

        [HttpGet("Car/CarShareDetail/{id}")]
        public IActionResult CarShareDetail(int id)
        {
            var cars = _car.tbl_CarShare.FirstOrDefault(c => c.CarID == id);
            if (cars == null)
            {
                return NotFound();
            }
            return View(carsharedetail, cars);
        }

        [HttpGet("Car/CarSellDetail/{id}")]
        public IActionResult CarSellDetail(int id)
        {
            var cars = _car.tbl_CarSell.FirstOrDefault(c => c.CarID == id);
            if (cars == null)
            {
                return NotFound();
            }
            return View(carselldetail, cars);
        }

        //Bộ lọc nâng cao
        [HttpGet("Car/AdvancedSearch")]
        public async Task<IActionResult> AddvancedSearch(string Option, string query, string Status, string Daysell, string Days, string RentalPrice, string VehicleRegistration, string SellPrice)
        {

            var shareCar = await _car.tbl_CarShare.ToListAsync();
            var sellCar = await _car.tbl_CarSell.ToListAsync();
            if (Option == "Thuê xe")
            {
                sellCar = null;
                if (!string.IsNullOrEmpty(query))
                {
                    shareCar = shareCar
                        .Where(c => c.Model.ToLower().Contains(query.ToLower()) || c.Brand.ToLower().Contains(query.ToLower()))
                        .ToList();
                }
                // Tìm theo trạng thái thuê
                if (!string.IsNullOrEmpty(Status) && Status != "Tất cả trạng thái")
                {
                    shareCar = shareCar
                        .Where(c => c.Status == Status)
                        .ToList();
                }
                if (!string.IsNullOrEmpty(Days) && Days != "Tất cả thời gian")
                {
                    DateTime compareDate = Days switch
                    {
                        "Hôm nay" => DateTime.Today,
                        "1 ngày trước" => DateTime.Today.AddDays(-1),
                        "3 ngày trước" => DateTime.Today.AddDays(-3),
                        "7 ngày trước" => DateTime.Today.AddDays(-7),
                        "1 tháng trước" => DateTime.Today.AddMonths(-1),
                        "1 năm trước" => DateTime.Today.AddYears(-1),
                        _ => DateTime.MinValue
                    };

                    shareCar = shareCar
                        .Where(c => c.Day >= compareDate)
                        .ToList();
                }

                // Tìm theo giá thuê
                if (!string.IsNullOrEmpty(RentalPrice) && RentalPrice != "Tất cả")
                {
                    // Xử lý giá thuê dựa trên giá trị của RentalPrice
                    shareCar = RentalPrice switch
                    {
                        "Dưới 500.000" => shareCar.Where(c => c.RentalPrice < 500_000).ToList(),
                        "500.000 - 1.000.000" => shareCar.Where(c => c.RentalPrice >= 500_000 && c.RentalPrice <= 1_000_000).ToList(),
                        "1.000.000 - 2.000.000" => shareCar.Where(c => c.RentalPrice > 1_000_000 && c.RentalPrice <= 2_000_000).ToList(),
                        "Trên 2.000.000" => shareCar.Where(c => c.RentalPrice > 2_000_000).ToList(),
                        _ => shareCar
                    };
                }


            }
            if (Option == "Mua xe")
            {
                shareCar = null;
                // Tìm theo từ khóa
                if (!string.IsNullOrEmpty(query))
                {
                    sellCar = sellCar
                        .Where(c => c.Model.ToLower().Contains(query.ToLower()) || c.Brand.ToLower().Contains(query.ToLower()))
                        .ToList();
                }

                // Tìm theo tình trạng giấy tờ
                if (!string.IsNullOrEmpty(VehicleRegistration) && VehicleRegistration != "Tất cả")
                {
                    sellCar = sellCar
                        .Where(c => c.VehicleRegistration == VehicleRegistration)
                        .ToList();
                }

                // Tìm theo giá bán
                if (!string.IsNullOrEmpty(SellPrice) && SellPrice != "Tất cả")
                {
                    // Xử lý giá bán dựa trên giá trị của SellPrice
                    sellCar = SellPrice switch
                    {
                        "Dưới 200 triệu" => sellCar.Where(c => c.SellPrice < 200_000_000).ToList(),
                        "200 - 500 triệu" => sellCar.Where(c => c.SellPrice >= 200_000_000 && c.SellPrice <= 500_000_000).ToList(),
                        "500 triệu - 1 tỷ" => sellCar.Where(c => c.SellPrice > 500_000_000 && c.SellPrice <= 1_000_000_000).ToList(),
                        "Trên 1 tỷ" => sellCar.Where(c => c.SellPrice > 1_000_000_000).ToList(),
                        _ => sellCar
                    };
                }

                // Tìm theo khoảng thời gian
                if (!string.IsNullOrEmpty(Daysell) && Daysell != "Tất cả thời gian")
                {
                    DateTime compareDate = Daysell switch
                    {
                        "Hôm nay" => DateTime.Today,
                        "1 ngày trước" => DateTime.Today.AddDays(-1),
                        "3 ngày trước" => DateTime.Today.AddDays(-3),
                        "7 ngày trước" => DateTime.Today.AddDays(-7),
                        "1 tháng trước" => DateTime.Today.AddMonths(-1),
                        "1 năm trước" => DateTime.Today.AddYears(-1),
                        _ => DateTime.MinValue
                    };
                    sellCar = sellCar
                        .Where(c => c.Day >= compareDate)
                        .ToList();
                }

            }

            // Kiểm tra xem có kết quả nào cho loại xe cho thuê
            if (Option == "Thuê xe" && shareCar.Any())
            {
                int countcarshares = shareCar.Count();
                ViewBag.CountCarShare = countcarshares;
                var models = new CarHomeModel
                {
                    CarShareAll = shareCar      // Danh sách xe cho thuê           
                };
                return View(addvancedsearch, models);
            }

            // Kiểm tra xem có kết quả nào cho loại xe bán
            if (Option == "Mua xe" && sellCar.Any())
            {
                int countcarsells = sellCar.Count();
                ViewBag.CountCarSell = countcarsells;
                var modela = new CarHomeModel
                {
                    CarSellAll = sellCar    // Danh sách xe bán
                };
                return View(addvancedsearch, modela);
            }
            int countcarsell = sellCar.Count();
            ViewBag.CountCarSell = countcarsell; // Số lượng xe bán
            int countcarshare = shareCar.Count();
            ViewBag.CountCarShare = countcarshare; // Số lượng xe thuê
            var model = new CarHomeModel
            {

                CarShareAll = shareCar,       // Danh sách xe cho thuê
                CarSellAll = sellCar    // Danh sách xe bán
            };
            return View(addvancedsearch, model);
        }
    }
}