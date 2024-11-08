using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace ShareCar.Models.Home.HomeModel
{
    public class CarShareModel
    {
        [Key]
        public int CarID {get; set; } 
		public int UserID {get; set; }
		public string Poster {get; set; }
	    public string LicensePlate {get; set; }
     	public string Brand {get;set;}
	    public string Model {get; set; } 
    	public string Color {get; set; }
	    public string Status {get; set; }
        public string Description {get; set; }
        public DateTime Day {get; set; }
		public string Address {get; set; }
		public string SDT {get; set; }
	    public double RentalPrice {get; set; } 
	    public string? Image {get; set; } 
	    public double? AverageRating {get; set; } 
    }
}