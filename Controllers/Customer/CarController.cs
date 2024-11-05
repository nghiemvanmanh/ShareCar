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
        string searchcar =  "~/Views/Customer/Car/CarSearch.cshtml";
        string newcar =  "~/Views/Customer/Car/NewCar.cshtml";
        string detailcar =  "~/Views/Customer/Car/CarDetail.cshtml";
        string index =  "~/Views/Customer/Car/Index.cshtml";
       private readonly ShareCarDBContext _car;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public CarController(IWebHostEnvironment hostingEnvironment, ShareCarDBContext car)
        {
            _hostingEnvironment = hostingEnvironment;
            _car = car;
        }

        [HttpGet("Car/Index")]
        public IActionResult Index(){
            var cars = _car.tbl_Cars.ToList();
            return View(index,cars);
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

                return RedirectToAction("Index");
            }

            return View(newcar,car);
        }

        [HttpGet("CarSearch")]
        public IActionResult CarSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction("Index"); // Nếu không có truy vấn, quay lại trang danh sách xe
            }

            var cars = _car.tbl_Cars
                .Where(c => c.Model.Contains(query) || c.Brand.Contains(query))
                .ToList(); // Tìm kiếm theo Model hoặc Brand
            HttpContext.Session.SetString("Key", query);
            return View(searchcar, cars); // Trả về view với danh sách xe tìm được
        }
    }
}