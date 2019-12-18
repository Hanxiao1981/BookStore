using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using Taihang.BookStore.Net.Areas.Admin.Models;
using Taihang.BookStore.Net.Models;

namespace Taihang.BookStore.Net.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admins")]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/User
        public ActionResult Index(string searchKey, int pageNumber = 1, int pageSize = 10)
        {
            IQueryable<ApplicationUser> query = db.Users.OrderBy(user => user.UserName);

            if(!string.IsNullOrEmpty(searchKey))
            {
                query = db.Users.Where(user => user.UserName.IndexOf(searchKey) > -1).OrderBy(user => user.UserName);                
            }

            ViewBag.SearchKey = searchKey;
            ViewBag.PageNumber = pageNumber;

            return View(query.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            string search = form["searchKey"];
            int page = int.Parse(form["pageNumber"]);

            string selectUsers = form["selectedUsers"];

            if (!string.IsNullOrEmpty(selectUsers))
            {
                string[] idArr = selectUsers.Split(',');

                foreach (string id in idArr)
                {
                    ApplicationUser user = db.Users.Find(id);

                    switch (form["command"])
                    {
                        case "Enable":
                            user.Disable = false;
                            break;
                        case "Disable":
                            user.Disable = true;
                            break;
                    }
                }

                db.SaveChanges();
            }

            return RedirectToAction("Index", new { searchKey = search, pageNumber = page });
        }

        //// GET: Admin/User/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = db.Users.Find(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// GET: Admin/User/Create
        //public ActionResult Create()
        //{
        //    //ViewBag.ProfileID = new SelectList(db.UserProfiles, "ID", "RealName");
        //    return View();
        //}

        //// POST: Admin/User/Create
        //// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        //// 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Disable,ProfileID,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(applicationUser);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    //ViewBag.ProfileID = new SelectList(db.UserProfiles, "ID", "RealName", applicationUser.ProfileID);
        //    return View(applicationUser);
        //}

        // GET: Admin/User/UserRole/5, id 代表用户id
        public ActionResult UserRole(string id)
        {
            List<UserRolesViewModel> userRolesVM = new List<UserRolesViewModel>();

            var userStore = new UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(db);
            var userManager = new ApplicationUserManager(userStore);

            List<ApplicationRole> allRoles = db.Roles.Where(r => !r.Disable).ToList();

            foreach (ApplicationRole role in allRoles)
            {
                UserRolesViewModel vm = new UserRolesViewModel { ID = role.Id, Name = role.Name };

                vm.Selected = userManager.IsInRoleAsync(id, role.Name).Result;

                userRolesVM.Add(vm);
            }

            ViewBag.UserID = id;
            
            return View(userRolesVM);
        }

        // POST: Admin/User/UserRole/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRole(FormCollection form)
        {
            string userID = form["userID"]; // 用户
            string userRoles = form["selectRoles"]; // 用户角色

            // 借助ASP.NET Identity完成角色设置，方便又快捷
            var userStore = new UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(db);
            var userManager = new ApplicationUserManager(userStore);

            // 清空先前所有角色
            ApplicationUser user = userManager.FindByIdAsync(userID).Result;
            user.Roles.Clear(); 

            db.SaveChanges(); // 保存更改

            // 添加所选的角色
            IdentityResult result = userManager.AddToRolesAsync(userID, userRoles.Split(',')).Result;

            if(!result.Succeeded)
            {
                throw new Exception("设置用户角色时出错");
            }

            db.SaveChanges(); // 保存更改

            return Json(new { Message = "OK" });
        }

        //// GET: Admin/User/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = db.Users.Find(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// POST: Admin/User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    ApplicationUser applicationUser = db.Users.Find(id);
        //    db.Users.Remove(applicationUser);
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
