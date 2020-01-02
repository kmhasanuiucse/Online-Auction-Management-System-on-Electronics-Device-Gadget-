﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Database;
using Auction.Entities;

namespace Auction.Services.User
{
    public class UserProductService
    {
        #region Singleton

        public static UserProductService Instance
        {
            get
            {
                if (PrivateInstance == null)
                    PrivateInstance = new UserProductService();
                return PrivateInstance;
            }
        }
        private static UserProductService PrivateInstance { get; set; }

        private UserProductService()
        {

        }

        #endregion

        public bool AddNewPost(Product model)
        {
            using (var context=new AuctionDbContext())
            {
                context.Products.Add(model);
                return context.SaveChanges() > 0;

            }
        }

        public List<Product> GetAdByUserId(int userId)
        {
            using (var context = new AuctionDbContext())
            {
                return context.Products.Where(x => x.UserId == userId).ToList();
            }
        }

        public List<Product> GetLatestProductPosts(int numberOfProducts)
        {

            using (var context = new AuctionDbContext())
            {
                return context.Products.
                    Where(x => x.Category.IsActive && x.IsActive && x.User.IsActive).
                    OrderByDescending(x => x.Id).Take(numberOfProducts).
                    Include(x => x.Category).
                    ToList();

            }

        }

        public List<Product> GetEightProducts(int numberOfProducts)
        {

            using (var context = new AuctionDbContext())
            {
                return context.Products.
                    Where(x => x.Category.IsActive && x.IsActive && x.User.IsActive).
                    OrderByDescending(x => x.Id).Take(numberOfProducts).

                    Include(x => x.Category).
                    ToList();
            }

        }

        public List<Product> GetProductsByCategory(int categoryId, int numberOfProducts)
        {

            using (var context = new AuctionDbContext())
            {
                return context.Products.
                    Where(x => x.Category.Id == categoryId && x.Category.IsActive && x.User.IsActive).
                    OrderByDescending(x => x.Id).
                    Take(numberOfProducts).
                    Include(x => x.Category).
                    ToList();

            }

        }

        public int GetMaximumPrice()
        {
            using (var context = new AuctionDbContext())
            {
                return (int)(context.Products.Where(x => x.Category.IsActive && x.IsActive && x.User.IsActive).Max(x => x.BasePrice));
            }
        }

        public int GetMinimumPrice()
        {
            using (var context = new AuctionDbContext())
            {
                return (int)(context.Products.Where(x => x.Category.IsActive && x.IsActive && x.User.IsActive).Min(x => x.BasePrice));
            }
        }

        public List<Product> SearchProduct(string searchTxt, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy, int pageNo, int pageSize)
        {
            using (var context = new AuctionDbContext())
            {
                var products = context.Products.Where(x => x.Category.IsActive && x.IsActive && x.User.IsActive).ToList();

                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.Category.Id == categoryID.Value).ToList();
                }

                if (!string.IsNullOrEmpty(searchTxt))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTxt.ToLower())).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.BasePrice >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.BasePrice <= maximumPrice.Value).ToList();
                }



                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderBy(x => x.BasePrice).ToList();
                            break;
                        case 3:
                            products = products.OrderByDescending(x => x.BasePrice).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.StarDateTime).ToList();
                            break;
                        case 5:
                            products = products.OrderBy(x => x.StarDateTime).ToList();
                            break;
                        default:
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;
                    }
                }

                return products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public int SearchProductCount(string searchTxt, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy)
        {
            using (var context = new AuctionDbContext())
            {
                var products = context.Products.Where(x => x.Category.IsActive && x.IsActive && x.User.IsActive).ToList();

                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.Category.Id == categoryID.Value).ToList();
                }

                if (!string.IsNullOrEmpty(searchTxt))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTxt.ToLower())).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.BasePrice >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.BasePrice <= maximumPrice.Value).ToList();
                }



                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderBy(x => x.BasePrice).ToList();
                            break;
                        case 3:
                            products = products.OrderByDescending(x => x.BasePrice).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.StarDateTime).ToList();
                            break;
                        case 5:
                            products = products.OrderBy(x => x.StarDateTime).ToList();
                            break;
                        default:
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;
                    }
                }

                return products.Count;
            }
        }

        public bool BidNow(Bidder model)
        {
            using (var context=new AuctionDbContext())
            {
                //set current price to product
                var product = context.Products.Find(model.ProductId);
                product.CurrentBidPrice = model.BidPrice;
                context.SaveChanges();

                context.Bidders.Add(model);
                return context.SaveChanges() > 0;
            }
        }

        public List<Bidder> Bidders(int productId)
        {
            using (var context=new AuctionDbContext())
            {
                return context.Bidders.Where(x => x.ProductId == productId).Include(x=>x.User).OrderByDescending(x=>x.Id).ToList();
            }
        }
    }
}
