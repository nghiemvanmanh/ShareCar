using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareCar.Data;
using ShareCar.Models.Customer.UserModel;

namespace ShareCar.Controllers.Admin
{
    public class UserController : Controller
    {   
        string usermanager = "~/Views/Admin/User/UserManager.cshtml";
        string editmanager = "~/Views/Admin/User/EditManager.cshtml";
        private readonly ShareCarDBContext _user;

        public UserController(ShareCarDBContext user){
            this._user = user;
        }

        [HttpGet("User/UserManager")]
        public IActionResult UserManager(){
            var user = _user.tblUser.ToList();
            return View(usermanager, user);
        }
        
        //Hiển thị form sửa
        [HttpGet("User/EditManager/{id}")]
        public IActionResult Edit(int id)
        {
            var users = _user.tblUser.Find(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(editmanager,users);
        }

        // Cập nhật thông tin sửa
        [HttpPost("User/EditManager")]
        public async Task<IActionResult> Edit(AccountModel model) {
             var existingUserByEmail = await _user.tblUser
            .FirstOrDefaultAsync(u => u.Email == model.Email && u.UserName != model.UserName);

            if (existingUserByEmail != null) {
            ModelState.AddModelError("Email", "Email này đã được sử dụng.");
            }
            if (ModelState.IsValid) {
                
                _user.Update(model);
                await _user.SaveChangesAsync();
                TempData["SuccessUpdate"] = "Cập nhật thành công!";
                return RedirectToAction("UserManager","User");
            }
            return View(editmanager,model);
        }
        
        //Xóa người dùng
        [HttpDelete]
        public IActionResult Delete(int id) {
            var users = _user.tblUser.Find(id);
            if (users != null) {
                _user.tblUser.Remove(users);
                _user.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}