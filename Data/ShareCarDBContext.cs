using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShareCar.Models;
using ShareCar.Models.Customer.CarModel;
using ShareCar.Models.Customer.UserModel;
using ShareCar.Models.Home.CarModel;


namespace ShareCar.Data
{
    public class ShareCarDBContext : DbContext
    {
        public ShareCarDBContext(DbContextOptions<ShareCarDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>()
            .HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarShareModel>()
            .HasKey(p => p.CarID);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarSellModel>()
            .HasKey(p => p.CarID);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarShareQueue>()
            .HasKey(p => p.CarID);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarSellQueue>()
            .HasKey(p => p.CarID);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CommentModel>()
            .HasOne(c => c.carShareModel)
            .WithMany(p => p.CommentShare)
            .HasForeignKey(c => c.CarID);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccountModel> tblUser { get; set; }
        public DbSet<CarShareModel> tbl_CarShare { get; set; }
        public DbSet<CarSellModel> tbl_CarSell { get; set; }

        public DbSet<CarShareQueue> tbl_CarShareQueue { get; set; }

        public DbSet<CarSellQueue> tbl_CarSellQueue { get; set; }

        public DbSet<CommentModel> tbl_Comment { get; set; }
    }
}