using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShareCar.Models.Customer.CarModel;
using ShareCar.Models.Home.CarModel;

namespace ShareCar.Models
{
    public class CarHomeModel
    {   
        public IEnumerable<CarShareModel> CarShare { get; set; }  // Danh sách xe cho thuê
        public IEnumerable<CarSellModel> CarSell { get; set; }    // Danh sách xe bán

        public IEnumerable<CarShareModel> CarShareAll { get; set; }  // Danh sách xe cho thuê
        public IEnumerable<CarSellModel> CarSellAll { get; set; }

        public IEnumerable<CarShareModel> CarShareManager { get; set; }  // Danh sách xe cho thuê
        public IEnumerable<CarSellModel> CarSellManager { get; set; }    // Danh sách xe bán
        public IEnumerable<CarShareModel> CarShareUser { get; set; }  // Danh sách xe cho thuê
        public IEnumerable<CarSellModel> CarSellUser { get; set; }    // Danh sách xe bán

        public IEnumerable<CarShareQueue> carShareQueues { get; set; } // Danh sách xe cho thuê duyệt
        public IEnumerable<CarSellQueue> carSellQueues { get; set; } // Danh sách xe đăng bán duyệt

        public IEnumerable<CarShareFavorite> carShareFavorites { get; set; } //Danh sách xe cho thuê yêu thích
        public IEnumerable<CarSellFavorite> carSellFavorites { get; set; } //Danh sách xe đăng bán yêu thích
    }
}