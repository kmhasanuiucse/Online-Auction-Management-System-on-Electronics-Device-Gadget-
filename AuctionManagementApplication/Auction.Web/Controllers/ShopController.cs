﻿using System;
using System.Linq;
using System.Web.Helpers;
using Auction.Web.ViewModels;
using System.Web.Mvc;
using Auction.Entities;
using Auction.Services.User;
using Auction.Services.UserAccountService;

namespace Auction.Web.Controllers
{
    public class ShopController : Controller
    {
        public ActionResult Index(string searchTxt, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            int pageSize = 9;
            ShopViewModel model = new ShopViewModel
            {
                FeaturedCategories = UserCategoryService.Instance.GetFeaturedNotNullItemCategory(),
                MaximumPrice = UserProductService.Instance.GetMaximumPrice(),
                MinimumPrice = UserProductService.Instance.GetMinimumPrice()
            };

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            model.SortBy = sortBy;
            model.CategoryID = categoryID;

            int totalCount = UserProductService.Instance.SearchProductCount(searchTxt, minimumPrice, maximumPrice, categoryID, sortBy);
            model.Products = UserProductService.Instance.SearchProduct(searchTxt, minimumPrice, maximumPrice, categoryID, sortBy, pageNo.Value, pageSize);

            model.Pager = new Pager(totalCount, pageNo, pageSize);

            return View(model);

        }
        public ActionResult FilterProduct(string searchTxt, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            int pageSize = 9;
            FilterProductsViewModel model = new FilterProductsViewModel();

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            model.SortBy = sortBy;
            model.CategoryID = categoryID;

            int totalCount = UserProductService.Instance.SearchProductCount(searchTxt, minimumPrice, maximumPrice, categoryID, sortBy);
            model.Products = UserProductService.Instance.SearchProduct(searchTxt, minimumPrice, maximumPrice, categoryID, sortBy, pageNo.Value, pageSize);

            model.Pager = new Pager(totalCount, pageNo, pageSize);
            return PartialView(model);

        }

        public ActionResult BidNow(Bidder model)
        {
            if (Session["UserData"] == null) { return RedirectToAction("Login", "User"); }
            var userData = Session["UserData"] as User;
            model.UserId = userData.Id;
            model.DateTime=DateTime.Now;
            JsonResult json=new JsonResult
            {
                Data=UserProductService.Instance.BidNow(model) ? new { Success = true} : new { Success = false}

            };
            return json;
        }

    }
}