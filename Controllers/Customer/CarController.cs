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

namespace ShareCar.Controllers.Customer
{
    public class CarController : Controller
    {
       
        string index =  "~/Views/Customer/Car/Index.cshtml";
        string listcar=  "~/Views/Customer/Car/CarList.cshtml";
       private readonly ShareCarDBContext _car;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
        {
            _hostingEnvironment = hostingEnvironment;
            _car = car;
        }


        [HttpGet("Car/CarList")]
        public async Task<IActionResult> CarList()
        {
            var userid = HttpContext.Session.GetInt32("UserID");
        

            // Lấy danh sách xe của người dùng
            var cars = await _car.tbl_CarShare.Where(u => u.UserID == userid).ToListAsync();

            if (cars == null || !cars.Any())
            {
                return View(listcar, cars);
            }

            return View(listcar, cars);
        }

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

    }
}