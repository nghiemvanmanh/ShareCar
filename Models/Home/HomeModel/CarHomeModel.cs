using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShareCar.Models.Home.HomeModel;

namespace ShareCar.Models
{
    public class CarHomeModel
    {   
        public IEnumerable<CarShareModel> CarShare { get; set; }  // Danh sách xe cho thuê
        public IEnumerable<CarSellModel> CarSell { get; set; }    // Danh sách xe bán
    }
}