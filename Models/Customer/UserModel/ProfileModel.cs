using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShareCar.Models.Customer.UserModel
{
    public class ProfileModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^.{5,}[0-9]$", ErrorMessage = "VerifyKey phải có ít nhất 6 ký tự và kết thúc bằng một chữ số.")]
        // [MinLength(6, ErrorMessage = "VerifyKey phải có ít nhất 6 ký tự.")]
        // [RegularExpression(@".*\d$", ErrorMessage = "VerifyKey phải kết thúc bằng một chữ số.")]
        public string? VerifyKey { get; set; }
    }
}