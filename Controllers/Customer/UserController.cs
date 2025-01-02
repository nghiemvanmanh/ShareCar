using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareCar.Data;
using ShareCar.Models.Customer.UserModel;

namespace ShareCar.Controllers.Customer
{

    public class UserController : Controller
    {
        private readonly ShareCarDBContext user;

        public UserController(ShareCarDBContext user){
            this.user = user;
        }

        string log = "~/Views/Customer/User/Login.cshtml";
        string reg = "~/Views/Customer/User/Register.cshtml";
        string edit = "~/Views/Customer/User/Edit.cshtml";
        string profile = "~/Views/Customer/User/Profile.cshtml";

        [HttpGet("Register")]
        public IActionResult Register(){
            return View(reg);
        }

        
       [HttpGet("Login")]
        public IActionResult Login(){
            return View(log);
        }

        [HttpPost("Register")]
        public IActionResult Register(AccountModel register){
            
            //Phòng chống XSS (loại bỏ các ký tự nguy hiểm trong dữ liệu đầu vào)
            register.UserName = HttpUtility.HtmlEncode(register.UserName);
            register.Email = HttpUtility.HtmlEncode(register.Email);
            register.FullName = HttpUtility.HtmlEncode(register.FullName);
            register.SDT = HttpUtility.HtmlEncode(register.SDT);
            if(ModelState.IsValid){
                if(user.tblUser.Any(u=> u.UserName.ToLower() == register.UserName.ToLower())){
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    return View(reg, register);
                }
                if( register.PassWord.Length < 4){
                    ModelState.AddModelError("PassWord", "Mật khẩu phải có ít nhất 4 ký tự");
                    return View(reg, register);
                }
                if(user.tblUser.Any(u=> u.Email.ToLower() == register.Email.ToLower())){
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(reg, register);
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(register.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("Email", "Email không đúng định dạng");
                    return View(reg, register);
                }

                // Kiểm tra định dạng số điện thoại (chỉ cho phép các chữ số và độ dài 10-11 ký tự)
                if (!System.Text.RegularExpressions.Regex.IsMatch(register.SDT, @"^0[0-9]{9}$"))
                {
                    ModelState.AddModelError("SDT", "Số điện thoại không hợp lệ");
                    return View(reg, register);
                }
                user.tblUser.Add(register);
                user.SaveChanges();
                TempData["SuccessMessage"] = "Đăng ký thành công!";
                return RedirectToAction("Login","User");
                
            }
            return View(reg, register);

            
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel login){
            var timeLock = HttpContext.Session.GetString("TimeLock");
            if (!string.IsNullOrEmpty(timeLock) && DateTime.Now < DateTime.Parse(timeLock))
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau 30 phút.");
                return View(log,login);
            }
            if(ModelState.IsValid){
                var _user = user.tblUser.FirstOrDefault(u => u.UserName.ToLower() == login.UserName.ToLower() && u.PassWord == login.PassWord);
                if (_user == null)
                {
                    var errorLogin = HttpContext.Session.GetInt32("ErrorLogin") ?? 0;
                    errorLogin++;
                    HttpContext.Session.SetInt32("ErrorLogin", errorLogin);
                    if (errorLogin >= 3)
                    {   
                        
                        HttpContext.Session.SetString("TimeLock", DateTime.Now.AddMinutes(30).ToString());
                        ModelState.AddModelError("", "Tài khoản bị khóa do đăng nhập sai quá nhiều lần. Vui lòng thử lại sau 30 phút.");
                        HttpContext.Session.SetString("UserLock", login.UserName);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                    }
                    return View(log, login);
                }

                // Kiểm tra mật khẩu
                if (_user != null)
                {
                    HttpContext.Session.SetString("UserName", _user.UserName);
                    HttpContext.Session.SetString("FullName", _user.FullName);
                    HttpContext.Session.SetString("SDT", _user.SDT);
                    HttpContext.Session.SetInt32("UserID", _user.Id);
                    HttpContext.Session.SetInt32("ErrorLogin", 0);
                    HttpContext.Session.Remove("TimeLock");
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(log, login);
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName"); // Xóa username khỏi Session
            HttpContext.Session.Remove("FullName");
            HttpContext.Session.Remove("SDT");
            HttpContext.Session.Remove("UserID");
            return RedirectToAction("Index", "Home"); // Chuyển hướng về trang đăng nhập
        }

        //Hiển thị form Profile

        [HttpGet("User/Profile")]
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("UserName"); // Lấy tên người dùng từ session
            if (username == null)
            {
                return RedirectToAction("Login","User"); // Chuyển hướng đến trang đăng nhập nếu không có session
            }

            // Giả sử bạn có phương thức để lấy người dùng theo username
            var users = user.tblUser.FirstOrDefault(u => u.UserName == username);
            if (users == null)
            {
                return NotFound(); // Nếu không tìm thấy người dùng
            }

            var model = new ProfileModel
            {
                UserName = users.UserName,
                PassWord = users.PassWord,
                FullName = users.FullName,
                Email = users.Email,
                SDT = users.SDT,
                Address = users.Address,
                VerifyKey = users.VerifyKey
            };

            return View(profile,model); // Trả về view với model chứa thông tin người dùng
        }

        [HttpGet("User/Edit")]
        public IActionResult Edit()
        {
            var username = HttpContext.Session.GetString("UserName"); // Lấy tên người dùng từ session
            if (username == null)
            {
                return RedirectToAction("Login","User"); // Chuyển hướng đến trang đăng nhập nếu không có session
            }

            // Lấy người dùng từ database
            var users = user.tblUser.FirstOrDefault(u => u.UserName == username);
            if (users == null)
            {
                return NotFound(); // Nếu không tìm thấy người dùng
            }

            var model = new ProfileModel
            {
                UserName = users.UserName,
                PassWord = users.PassWord,
                FullName = users.FullName,
                Email = users.Email,
                SDT = users.SDT,
                Address = users.Address,
                VerifyKey = users.VerifyKey
            };

            return View(edit,model); // Trả về view để người dùng chỉnh sửa thông tin
        }


        [HttpPost("User/Edit")]
        public async Task<IActionResult> Edit(ProfileModel model)
        {
            var existingUserByEmail = await user.tblUser
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Id != model.Id);

                if (existingUserByEmail != null) {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                }   
            if (ModelState.IsValid)
            {
                var userEntity = await user.tblUser.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (userEntity == null)
                {
                    return NotFound(); // Nếu không tìm thấy người dùng
                }

                // Cập nhật thông tin người dùng
                userEntity.FullName = model.FullName;
                userEntity.PassWord = model.PassWord;
                userEntity.Email = model.Email;
                userEntity.SDT = model.SDT;
                userEntity.Address = model.Address;
                userEntity.VerifyKey = model.VerifyKey;
                // Lưu thay đổi vào database
                await user.SaveChangesAsync(); 
                TempData["Success"] = "Cập nhật thông tin thành công!"; // Thêm thông báo thành công
                return RedirectToAction("Profile"); // Chuyển hướng về trang profile
            }
        
            return View(edit,model); // Nếu có lỗi, trả về form để sửa
        }

    }
}