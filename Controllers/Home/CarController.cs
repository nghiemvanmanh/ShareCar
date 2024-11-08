using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShareCar.Data;
using ShareCar.Models.Home;
using ShareCar.Models.Home.HomeModel;

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
         string newcar =  "~/Views/Home/Car/CarShareNew.cshtml";

        [HttpGet("Car/CarShareNew")]
        public IActionResult CarShareNew(){
            return View(newcar);
        }

        //Thêm mới xe
        [HttpPost("Car/CarShareNew")]
        public async Task<IActionResult> CarShareNew(CarShareModel car,IFormFile imageFile)
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
                _car.tbl_CarShare.Add(car);
                await _car.SaveChangesAsync();

                return RedirectToAction("Index","Home");
            }

            return View(newcar,car);
        }
    }
}