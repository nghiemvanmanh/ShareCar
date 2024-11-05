using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShareCar.Models.Customer.UserModel
{
    public class AccountModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        
    }
}