﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareCar.Data;
using ShareCar.Models;
using ShareCar.Models.Home.CarModel;

namespace ShareCar.Controllers.Home;

public class HomeController : Controller
{
    private readonly ShareCarDBContext _car;

    public HomeController(ShareCarDBContext car)
    {
        _car = car;
    }

    string carsell = "~/Views/Home/CarSell.cshtml";
    string carshare = "~/Views/Home/CarShare.cshtml";

    //Hiển thị view trang chủ
    public IActionResult Index()
    {
         int ?id = HttpContext.Session.GetInt32("UserID");
        var model = new CarHomeModel
        {
            CarShare = _car.tbl_CarShare.ToList(),       // Danh sách xe cho thuê
            CarSell = _car.tbl_CarSell.ToList()      // Danh sách xe bán
        };
        if (HttpContext.Session.GetString("UserName") != "admin")
        {
           
            var carShareQueue = _car.tbl_CarShareQueue.Where(c => c.CarID == id).ToList();
            var carSellQueue = _car.tbl_CarSellQueue.Where(c => c.CarID == id).ToList();
            ViewBag.count = carShareQueue.Count + carSellQueue.Count;
            Console.WriteLine("ID la : " + id);
            return View(model);
        } 
        Console.WriteLine("ID la : " + id );
        ViewBag.count = _car.tbl_CarShareQueue.Count() + _car.tbl_CarSellQueue.Count();
        return View(model);
    }

    //Lấy danh sách xe thuê
    [HttpGet("Home/CarShare")]
    public IActionResult CarShare()
    {
        var carShare = _car.tbl_CarShare.ToList(); // Lấy danh sách xe cho thuê
        return PartialView(carshare, carShare);
    }

    //Lấy danh sách xe bán
    [HttpGet("Home/CarSell")]
    public IActionResult CarSell()
    {
        var carSell = _car.tbl_CarSell.ToList(); // Lấy danh sách xe cho thuê
        return PartialView(carsell, carSell);  // Trả về PartialView "_CarRental" với dữ liệu
    }


    //Hiển thị banner
    [HttpGet("Home/CarBanner")]
    public IActionResult CarBanner()
    {
        return View("~/Views/Home/CarBanner.cshtml");
    }





}
