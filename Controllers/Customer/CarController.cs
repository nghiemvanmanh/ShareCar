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
        string newcar =  "~/Views/Customer/Car/NewCar.cshtml";
        string detailcar =  "~/Views/Customer/Car/CarDetail.cshtml";
        string index =  "~/Views/Customer/Car/Index.cshtml";
        string listcar=  "~/Views/Customer/Car/CarList.cshtml";
       private readonly ShareCarDBContext _car;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
        {
            _hostingEnvironment = hostingEnvironment;
            _car = car;
        }

        [HttpGet("Car/CarDetail/{id}")]
        public IActionResult CarDetails(int id)
        {
            var cars = _car.tbl_Cars.FirstOrDefault(c => c.CarID == id);
            if (cars == null)
            {
                return NotFound();
            }
            return View(detailcar,cars);
        }

        //Trả về View Thêm xe
        [HttpGet("Car/NewCar")]
        public IActionResult NewCar()
        {
            return View(newcar);
        }

        //Thêm mới xe
        [HttpPost("Car/NewCar")]
        public async Task<IActionResult> NewCar(CarShareModel car,IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                
                if(imageFile != null && imageFile.Length > 0){
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine("Model Error: " + error.ErrorMessage);
                            Console.WriteLine("Model Error: " + imageFile.Length);
                            Console.WriteLine("Model Error: " + Guid.NewGuid().ToString() + "_" + imageFile.FileName);
                        }
                    }
                return View(newcar,car);
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
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    // Tạo tên file duy nhất để tránh trùng lặp
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu file vào thư mục
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    car.Image = "/images/" + uniqueFileName;
                    Console.WriteLine("Url Image : " + car.Image);
                }
                Console.WriteLine("Url Image : " + car.Image);
                // Thêm car vào cơ sở dữ liệu
                _car.tbl_Cars.Add(car);
                await _car.SaveChangesAsync();

                return RedirectToAction("Index","Home");
            }

            return View(newcar,car);
        }


        [HttpGet("Car/CarList")]
        public async Task<IActionResult> CarList()
        {
            var userid = HttpContext.Session.GetInt32("UserID");
        

            // Lấy danh sách xe của người dùng
            var cars = await _car.tbl_Cars.Where(u => u.UserID == userid).ToListAsync();

            if (cars == null || !cars.Any())
            {
                return View(listcar, cars);
            }

            return View(listcar, cars);
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