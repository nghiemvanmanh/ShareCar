using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShareCar.Models;
using ShareCar.Models.Customer.UserModel;


namespace ShareCar.Data
{
    public class ShareCarDBContext : DbContext
    {
        public  ShareCarDBContext(DbContextOptions<ShareCarDBContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<AccountModel>()
            .HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarShareModel>()
            .HasKey(p => p.CarID);
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<AccountModel> tblUser { get; set; }
        public DbSet<CarShareModel> tbl_Cars { get; set; }
    }
}