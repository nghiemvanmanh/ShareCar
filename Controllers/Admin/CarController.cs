using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareCar.Data;

namespace ShareCar.Controllers.Admin
{

    public class CarController : Controller
    {
        string carmanager = "~/Views/Admin/Car/CarManager.cshtml";
        private readonly IWebHostEnvironment _hostingEnvironment;
          private readonly ShareCarDBContext _car;
            public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
            {
                _hostingEnvironment = hostingEnvironment;
                _car = car;
            }

            [HttpGet("Car/CarManager")]
            public async Task<IActionResult> CarManager(string searchString) {
                var cars = await _car.tbl_Cars.ToListAsync(); // Lấy danh sách tất cả xe

                // Kiểm tra xem có từ khóa tìm kiếm hay không
                if (!string.IsNullOrEmpty(searchString))
                {
                    // Nếu searchString có thể chuyển đổi sang int, tìm kiếm theo ID
                    if (int.TryParse(searchString, out int carId))
                    {
                        cars = cars.Where(c => c.CarID == carId).ToList();
                    }
                    else
                    {
                        // Tìm kiếm theo Model
                        cars = cars.Where(c => c.Model.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                }
                return View(carmanager, cars); 
                }
        
    }
}