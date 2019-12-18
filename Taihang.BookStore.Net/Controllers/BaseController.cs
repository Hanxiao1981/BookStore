using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taihang.BookStore.Net.Models;

namespace Taihang.BookStore.Net.Controllers
{
    public class BaseController : Controller
    {
        public const string CartSessionKey = "CartID";

        private ApplicationDbContext _dbContext;
        private ApplicationUserManager _userManager;

        public ApplicationDbContext AppDbContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().GetUserManager<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }
        }

        public ApplicationUserManager AppUserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public List<CartItem> ShoppingCart
        {
            get
            {
                string cartID = GetCartId();

                return AppDbContext.Cart.Include("Book").Where(c => c.CartId == cartID).ToList();
            }
        }

        // 获取购物车ID
        public string GetCartId()
        {
            if (Session[CartSessionKey] == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser currentUser = AppUserManager.FindByNameAsync(User.Identity.Name).Result;

                    Session[CartSessionKey] = currentUser.Id;
                }
                else
                {
                    Session[CartSessionKey] = Guid.NewGuid().ToString();
                }
            }

            return Session[CartSessionKey].ToString();
        }

        // 购物车物品数量
        public int CartUnits
        {
            get
            {
                int unit = 0;

                if (ShoppingCart != null)
                {
                    unit = ShoppingCart.Count;
                }

                return unit;
            }
        }

        // 购物车总金额
        public decimal CartTotalPrice
        {
            get
            {
                return ShoppingCart.Sum(item => item.Book.Price * item.Count);
            }
        }
    }
}