using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

using Taihang.BookStore.Net.Models;

namespace Taihang.BookStore.Net.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admins")]
    public class BookController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Book
        public ActionResult Index(string searchKey, int selectedCategory = 0, int pageNumber = 1, int pageSize = 5)
        {
            var query = db.Book.Include(b => b.Category);

            if(!string.IsNullOrEmpty(searchKey))
            {
                query = query.Where(b => b.Name.IndexOf(searchKey) > -1);
            }

            if(selectedCategory > 0)
            {
                query = query.Where(b => b.CategoryID == selectedCategory);
            }

            // 图书类别
            List<SelectListItem> categoryList = new List<SelectListItem> { new SelectListItem { Text = "全部", Value = "0", Selected = selectedCategory == 0 } };
            foreach(var item in db.BookCategory)
            {
                categoryList.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString(), Selected = selectedCategory == item.ID });
            }
            
            ViewBag.BookCategory = categoryList; // 构建分类下拉列表框
            ViewBag.SearchKey = searchKey; // 设置搜索文本框
            ViewBag.PageNumber = pageNumber; // 更新时定位到当前页
            ViewBag.SelectedCategory = selectedCategory;  // 选中的图书分类          

            return View(query.OrderByDescending(b => b.ID).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            string search = form["searchKey"]; // 搜索关键字
            int page = int.Parse(form["pageNumber"]); // 页码
            int category = int.Parse(form["CategoryID"]); // 图书分类

            string selectBooks = form["selectedBooks"]; // 选中图书
            string command = form["command"]; // 选中操作
            switch (command)
            {
                case "Enable":                   
                case "Disable":
                    if (!string.IsNullOrEmpty(selectBooks))
                    {
                        string[] idArr = selectBooks.Split(',');

                        foreach (string id in idArr)
                        {
                            Book book = db.Book.Find(int.Parse(id));

                            book.Disable = command == "Disable";
                        }

                        db.SaveChanges();
                    }
                    break;
                case "Search":
                    page = 1;
                    break;
            }

            return RedirectToAction("Index", new { searchKey = search, selectedCategory = category, pageNumber = page });
        }

        // GET: Admin/Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Admin/Book/Create
        public ActionResult Create()
        {
            // 图书类别
            List<SelectListItem> categoryList = new List<SelectListItem>();

            foreach (var item in db.BookCategory)
            {
                categoryList.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }

            ViewBag.BookCategory = categoryList; // 构建分类下拉列表框

            return View();
        }

        // POST: Admin/Book/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Author,Describe,Price,CategoryID")] Book book)
        {
            if (ModelState.IsValid)
            {                
                HttpPostedFileBase uploadFile = Request.Files["bookImg"];

                if(uploadFile != null)
                {
                    // 图片扩展名
                    string extension = Path.GetExtension(uploadFile.FileName);
                    string savePath = Server.MapPath($"/Imgs/{Guid.NewGuid()}{extension}");

                    uploadFile.SaveAs(savePath);
                    book.Img = Path.GetFileName(savePath);
                }
                
                db.Book.Add(book);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            //return View(book);
            return Json(new { Message = "OK" });
        }

        // GET: Admin/Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            // 图书类别
            List<SelectListItem> categoryList = new List<SelectListItem>();

            foreach (var item in db.BookCategory)
            {
                categoryList.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString(), Selected = item.ID == book.CategoryID });
            }

            ViewBag.BookCategory = categoryList; // 构建分类下拉列表框

            return View(book);
        }

        // POST: Admin/Book/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Author,Describe,Price,CategoryID")] Book book)
        {
            if (ModelState.IsValid)
            {
                Book dbbook = db.Book.Find(book.ID);
                dbbook.Name = book.Name;
                dbbook.Author = book.Author;
                dbbook.Describe = book.Describe;
                dbbook.Price = book.Price;
                dbbook.CategoryID = book.CategoryID;

                HttpPostedFileBase uploadFile = Request.Files["bookImg"];

                if (uploadFile != null && !string.IsNullOrEmpty(uploadFile.FileName))
                {
                    // 图片扩展名
                    string extension = Path.GetExtension(uploadFile.FileName);
                    string savePath1 = Server.MapPath($"/Imgs/{dbbook.Img}");
                    string savePath2 = Server.MapPath($"/Imgs/{Guid.NewGuid()}{extension}");

                    System.IO.File.Delete(savePath1); // 删除原来的图片

                    uploadFile.SaveAs(savePath2); // 保存最新的图片
                    dbbook.Img = Path.GetFileName(savePath2);
                }

                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            
            //return View(book);
            return Json(new { Message = "OK" });
        }

        public ActionResult ManageCategory()
        {
            return View(db.BookCategory);
        }

        public ActionResult AddCategory(BookCategory category)
        {
            db.BookCategory.Add(category);
            db.SaveChanges();

            return Json(new { Message = "OK" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCategory(BookCategory category)
        {
            BookCategory dbCategory = db.BookCategory.Find(category.ID);
            dbCategory.Name = category.Name;
            db.SaveChanges();

            return Json(new { Message = "OK" }, JsonRequestBehavior.AllowGet);
        }
        //// GET: Admin/Book/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Book book = db.Book.Find(id);
        //    if (book == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(book);
        //}

        //// POST: Admin/Book/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Book book = db.Book.Find(id);
        //    db.Book.Remove(book);
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
