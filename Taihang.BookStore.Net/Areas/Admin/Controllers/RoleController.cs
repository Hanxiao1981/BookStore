using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Taihang.BookStore.Net.Models;

namespace Taihang.BookStore.Net.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admins")]
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Role
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            string selectedRoles = form["selectedRoles"];
            if (!string.IsNullOrEmpty(selectedRoles))
            {
                string[] idArr = selectedRoles.Split(',');

                foreach (string id in idArr)
                {
                    ApplicationRole role = db.Roles.Find(id);

                    switch (form["command"])
                    {
                        case "Enable":
                            role.Disable = false;
                            break;
                        case "Disable":
                            role.Disable = true;
                            break;
                    }
                }

                db.SaveChanges();                                
            }           

            return View(db.Roles.ToList());
        }

        // GET: Admin/Role/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.Roles.Find(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // GET: Admin/Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Role/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Describe,Name")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(applicationRole);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return Json(new { Message = "OK" });
        }

        // GET: Admin/Role/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.Roles.Find(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // POST: Admin/Role/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Describe,Disable,Name")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationRole).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            //return View(applicationRole);
            return Json(new { Message = "OK" });
        }

        // GET: Admin/Role/Delete/5
        public ActionResult Delete(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //ApplicationRole applicationRole = db.Roles.Find(id);
            //if (applicationRole == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(applicationRole);

            ApplicationRole applicationRole = db.Roles.Find(id);
            db.Roles.Remove(applicationRole);
            db.SaveChanges();

            //return RedirectToAction("Index");
            return Json(new { Message = "OK" }, JsonRequestBehavior.AllowGet);
        }

        //// POST: Admin/Role/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    ApplicationRole applicationRole = db.Roles.Find(id);
        //    db.Roles.Remove(applicationRole);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
