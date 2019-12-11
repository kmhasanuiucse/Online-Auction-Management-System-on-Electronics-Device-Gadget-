﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Database;

namespace Auction.Services.Admin
{
   public class DashboardService
    {
        #region Singleton

        public static DashboardService Instance
        {
            get
            {
                if (PrivateInstance == null)
                    PrivateInstance = new DashboardService();
                return PrivateInstance;
            }
        }
        private static DashboardService PrivateInstance { get; set; }

        private DashboardService()
        {

        }

        #endregion
        public int GetCategoryCount()
        {
            using (var context = new AuctionDbContext())
            {
                return context.Categories.Count();
            }
        }
        public int GetProductCount()
        {
            using (var context = new AuctionDbContext())
            {
                return context.Products.Count();
            }
        }
        public int GetDistrictCount()
        {
            using (var context = new AuctionDbContext())
            {
                return context.Districts.Count();
            }
        }
        public int GetThanaCount()
        {
            using (var context = new AuctionDbContext())
            {
                return context.Thanas.Count();
            }
        }

    }
}