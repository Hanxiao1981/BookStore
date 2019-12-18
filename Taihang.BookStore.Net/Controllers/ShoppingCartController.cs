using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taihang.BookStore.Net.Models;

namespace Taihang.BookStore.Net.Controllers
{
    public class ShoppingCartController : BaseController
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ViewBag.ItemCount = CartUnits; // 物品数量
            ViewBag.TotalPrice = CartTotalPrice; // 购物车合计

            return View(ShoppingCart);
        }

        // 添加到购物车，id代表图书id
        public ActionResult AddToCart(int id)
        {
            string cartID = GetCartId();

            CartItem cartItem = AppDbContext.Cart.Where(it => it.CartId == cartID && it.BookID == id).SingleOrDefault();

            if (cartItem == null) // 未添加到购物车
            {
                Book book = AppDbContext.Book.Where(b => b.ID == id).SingleOrDefault();

                CartItem item = new CartItem { CartId = cartID, BookID = id, Count = 1, Book = book, CreateDate = DateTime.Now };

                AppDbContext.Cart.Add(item);
            }
            else // 已添加到购物车
            {
                cartItem.Count += 1;

                AppDbContext.Entry(cartItem).State = EntityState.Modified;
            }

            AppDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        // 从购物车中移除，id代表图书id
        public ActionResult RemoveFromCart(int id)
        {
            string cartID = GetCartId();

            CartItem cartItem = AppDbContext.Cart.Where(it => it.CartId == cartID && it.BookID == id).SingleOrDefault();

            if (cartItem != null)
            {
                AppDbContext.Cart.Remove(cartItem);
                AppDbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // 增加数量
        public ActionResult AddQuantity(int id)
        {
            string cartID = GetCartId();

            CartItem cartItem = AppDbContext.Cart.Where(it => it.CartId == cartID && it.BookID == id).SingleOrDefault();

            if (cartItem != null)
            {
                cartItem.Count += 1;
                AppDbContext.SaveChanges();
            }

            return Json(new { total_price = CartTotalPrice }, JsonRequestBehavior.AllowGet);
        }

        // 减少数量
        public ActionResult SubtractQuantity(int id)
        {
            string cartID = GetCartId();

            CartItem cartItem = AppDbContext.Cart.Where(it => it.CartId == cartID && it.BookID == id).SingleOrDefault();

            if (cartItem != null)
            {
                cartItem.Count -= 1;
                AppDbContext.SaveChanges();
            }

            return Json(new { total_price = CartTotalPrice }, JsonRequestBehavior.AllowGet);
        }

        //// GET: ShoppingCart/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ShoppingCart/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ShoppingCart/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ShoppingCart/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ShoppingCart/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ShoppingCart/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ShoppingCart/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
