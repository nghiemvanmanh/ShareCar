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
            var cars = _car.tbl_Cars ;
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
        public async Task<IActionResult> CarSearch(string query, string Status, string Days) {
            var car = await _car.tbl_Cars.ToListAsync(); // Lấy danh sách tất cả xe

                // Tìm kiếm theo ID hoặc Brand
                if (!string.IsNullOrEmpty(query))
                {   
                    Console.WriteLine("Status = " + Status);
                    car = car.Where(c => c.Model.Contains(query) || c.Brand.Contains(query)).ToList();
                }
                // Lọc theo trạng thái
                if (!string.IsNullOrEmpty(Status) && Status != "Tất cả trạng thái")
                {   
                    Console.WriteLine("Status = " + Status);
                    car = car.Where(c => c.Status == Status).ToList();
                }

                // Lọc theo số ngày chênh lệch (Days)
                if (!string.IsNullOrEmpty(Days) && Days != "Tất cả thời gian")
                {
                    if (!string.IsNullOrEmpty(Days) && Days != "Tất cả thời gian")
                    {
                        // Ngày hiện tại
                        DateTime now = DateTime.Now;
                        
                        switch (Days)
                        {
                            case "Hôm nay":
                                // Kiểm tra các bài đăng trong ngày hiện tại
                                car = car.Where(c => c.Day.Date == now.Date).ToList();
                                break;
                            case "1 ngày trước":
                                car = car.Where(c => (now - c.Day).TotalDays <= 1).ToList();
                                break;
                            case "3 ngày trước":
                                car = car.Where(c => (now - c.Day).TotalDays <= 3).ToList();
                                break;
                            case "7 ngày trước":
                                car = car.Where(c => (now - c.Day).TotalDays <= 7).ToList();
                                break;
                            case "1 tháng trước":
                                car = car.Where(c => (now - c.Day).TotalDays <= 30).ToList();
                                break;
                            case "1 năm trước":
                                car = car.Where(c => (now - c.Day).TotalDays <= 365).ToList();
                                break;
                        }
                    }
                }
            return View(searchcar, car); 
        }

    }
}