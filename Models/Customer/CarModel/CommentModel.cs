using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShareCar.Models.Home.CarModel;

namespace ShareCar.Models.Customer.CarModel
{
    public class CommentModel
    {
        [Key]
        public int CommentID { get; set; }
        public int CarID { get; set; }
        public string PosterName { get; set; }
        public DateTime DateUp { get; set; }
        public string CommentText { get; set; }
        
        public string NameComment { get; set; }
        public CarShareModel carShareModel { get; set; }
       
    }
}