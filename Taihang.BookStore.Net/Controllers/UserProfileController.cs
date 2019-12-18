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
    public class UserProfileController : BaseController
    {
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserProfile
        public ActionResult Index()
        {
            ApplicationUser user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;           

            if(user.Profile == null)
            {
                return RedirectToAction("Create");
            }

            return View(user.Profile);
        }

        //// GET: UserProfile/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserProfile userProfile = DbContext.UserProfile.Find(id);
        //    if (userProfile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userProfile);
        //}

        // GET: UserProfile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfile/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RealName,Phone,Address")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;
                user.ProfileID = user.Id;

                userProfile.ID = user.Id;
                userProfile.CreateTime = DateTime.Now;
                AppDbContext.UserProfile.Add(userProfile);               

                AppDbContext.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        // GET: UserProfile/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = AppDbContext.UserProfile.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfile/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RealName,Phone,Address")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                UserProfile dbProfile = AppDbContext.UserProfile.Find(userProfile.ID);

                dbProfile.RealName = userProfile.RealName;
                dbProfile.Phone = userProfile.Phone;
                dbProfile.Address = userProfile.Address;
                dbProfile.UpdateTime = DateTime.Now;

                AppDbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        //// GET: UserProfile/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserProfile userProfile = DbContext.UserProfile.Find(id);
        //    if (userProfile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userProfile);
        //}

        //// POST: UserProfile/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    UserProfile userProfile = DbContext.UserProfile.Find(id);
        //    DbContext.UserProfile.Remove(userProfile);
        //    DbContext.SaveChanges();
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
