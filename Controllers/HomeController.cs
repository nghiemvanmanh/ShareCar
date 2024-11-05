using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShareCar.Data;
using ShareCar.Models;

namespace ShareCar.Controllers;

public class HomeController : Controller
{
    string detailcar =  "~/Views/Customer/Car/CarDetail.cshtml";
    private readonly ShareCarDBContext _car;

        public HomeController(ShareCarDBContext car){
            _car = car;
        }

    public IActionResult Index()
    {
        var cars = _car.tbl_Cars.ToList();
            return View(cars);
    }

}
