﻿using System;
using System.Linq;
using System.Web.Helpers;
using Auction.Web.ViewModels;
using System.Web.Mvc;
using Auction.Entities;
using Auction.Services.Admin;
using Auction.Services.User;
using Auction.Services.UserAccountService;

namespace Auction.Web.Controllers
{
    public class AllAdController : Controller
    {
        public ActionResult Index(string searchTxt, DateTime? todayDateTime, int? districtId,int? thanaId, int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy, int? pageNo)
        {
            int pageSize = 9;
            ShopViewModel model = new ShopViewModel
            {
                SearchText = searchTxt,
                FeaturedCategories = UserCategoryService.Instance.GetFeaturedNotNullItemCategory(),
                Districts = DistrictService.Instance.GetAll(),
                MaximumPrice = UserProductService.Instance.GetMaximumPrice(),
                MinimumPrice = UserProductService.Instance.GetMinimumPrice()
            };

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            model.SortBy = sortBy;
            model.CategoryID = categoryId;
            model.DistrictId = districtId;

            //int totalCount = UserProductService.Instance.SearchProductCount(searchTxt, minimumPrice, maximumPrice, categoryId, sortBy);
            int totalCount = UserProductService.Instance.SearchProduct(searchTxt,todayDateTime, districtId,thanaId, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize).Count;
            model.Products = UserProductService.Instance.SearchProduct(searchTxt,todayDateTime, districtId,thanaId, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize);

            model.Pager = new Pager(totalCount, pageNo, pageSize);

            return View(model);

        }
        public ActionResult FilterProduct(string searchTxt, DateTime? todayDateTime, int? districtId,int? thanaId,int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy, int? pageNo)
        {
            int pageSize = 9;
            FilterProductsViewModel model = new FilterProductsViewModel();

            model.SearchText = searchTxt;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            model.SortBy = sortBy;
            model.CategoryID = categoryId;
            model.DistrictId = districtId;
            int totalCount = UserProductService.Instance.SearchProduct(searchTxt,todayDateTime, districtId,thanaId, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize).Count;
            model.Products = UserProductService.Instance.SearchProduct(searchTxt,todayDateTime,districtId,thanaId, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize);

            model.Pager = new Pager(totalCount, pageNo, pageSize);
            return PartialView(model);

        }

        public ActionResult BidNow(Bidder model,double depositAmount)
        {
            if (Session["UserData"] == null) { return RedirectToAction("Login", "User"); }
            var userData = Session["UserData"] as User;

            model.UserId = userData.Id;
            model.DateTime = DateTime.Now;

            var bidder = UserProductService.Instance.ExistBidder(model);
            
            if (bidder !=null)
            {
                JsonResult json = new JsonResult
                {
                    Data = UserProductService.Instance.UpdateBidNow(model) ? new { Success = true } : new { Success = false }

                };
                return json;
            }
            else
            {
                UserAccountService.Instance.DepositCreditForBidding(userData.Id, depositAmount);

                JsonResult json = new JsonResult
                {
                    Data = UserProductService.Instance.BidNow(model) ? new { Success = true } : new { Success = false }

                };
                return json;
            }


           
        }

    }
}