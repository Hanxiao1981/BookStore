using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Taihang.BookStore.Net.Models;

namespace Taihang.BookStore.Net.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            ApplicationUser user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;

            return View(AppDbContext.Order.Where(o => o.UserID == user.Id).OrderByDescending(o => o.ID).ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Order order = AppDbContext.Order.Find(id);
            Order order = AppDbContext.Order.Include("OrderItems").Where(o => o.ID == id).FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ApplicationUser user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;

            // 用户个人信息不存在，先填写个人信息
            if (user.Profile == null)
            {
                return RedirectToAction("Create","UserProfile");
            }
           
            ViewBag.Cart = ShoppingCart; // 购物车
            ViewBag.ItemCount = CartUnits; // 购物车物品数量
            ViewBag.TotalPrice = CartTotalPrice; // 购物车合计
            ViewBag.Profile = user.Profile; // 个人信息

            return View();
        }

        // POST: Order/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form)
        {
            ApplicationUser user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;
            UserProfile userProfile = user.Profile;

            #region 生成订单
            Order order = new Order(); // 订单信息

            order.UserID = user.Id;
            order.ReciverName = userProfile.RealName;
            order.ReciverPhone = userProfile.Phone;
            order.PostAddr = userProfile.Address;
            order.TotalSum = CartTotalPrice;
            order.OrderState = OrderStateEnum.已下单;
            order.StateDescribe = OrderStateEnum.已下单.ToString();
            order.OrderDate = DateTime.Now;         

            List<OrderItem> orderItems = new List<OrderItem>(); // 订单明细

            foreach (var cartItem in ShoppingCart)
            {
                OrderItem item = new OrderItem();

                item.Img = cartItem.Book.Img;
                item.Name = cartItem.Book.Name;
                item.Author = cartItem.Book.Author;
                item.Price = cartItem.Book.Price;
                item.Count = cartItem.Count;               

                item.BookID = cartItem.BookID;
                item.Book = cartItem.Book;

                orderItems.Add(item);
            }

            order.OrderItems = orderItems;

            AppDbContext.Order.Add(order);
            AppDbContext.SaveChanges();
            #endregion

            #region 清空购物车
            string cartID = GetCartId();
            List<CartItem> cartItems = AppDbContext.Cart.Where(it => it.CartId == cartID).ToList();

            AppDbContext.Cart.RemoveRange(cartItems);
            AppDbContext.SaveChanges();
            #endregion

            return RedirectToAction("Details", new { id = order.ID });
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = AppDbContext.Order.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            string nextStep = null;

            switch(order.OrderState)
            {
                case OrderStateEnum.已下单:
                    nextStep = "支付";
                    order.OrderState = OrderStateEnum.已付款;
                    break;
                case OrderStateEnum.已付款:
                    nextStep = "收货";
                    order.OrderState = OrderStateEnum.已完成;
                    break;
            }

            ViewBag.NextStep = nextStep;

            return View(order);
        }

        // POST: Order/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, OrderState")] Order order)
        {
            if (ModelState.IsValid)
            {
                Order dbOrder = AppDbContext.Order.Find(order.ID);
                dbOrder.OrderState = order.OrderState;
                dbOrder.StateDescribe = order.OrderState.ToString();

                AppDbContext.SaveChanges();                
            }

            return RedirectToAction("Index");
        }

        //// GET: Order/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = AppDbContext.Order.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Order/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Order order = AppDbContext.Order.Find(id);
        //    AppDbContext.Order.Remove(order);
        //    AppDbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AppDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
