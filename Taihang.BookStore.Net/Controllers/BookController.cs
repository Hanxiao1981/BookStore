using PagedList;
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
    public class BookController : BaseController
    {
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Book
        public ActionResult Index(string searchKey, int selectedCategory = 0, int pageNumber = 1, int pageSize = 5)
        {
            var query = AppDbContext.Book.Include(b => b.Category);

            if (!string.IsNullOrEmpty(searchKey))
            {
                query = query.Where(b => b.Name.IndexOf(searchKey) > -1);
            }

            if (selectedCategory > 0)
            {
                query = query.Where(b => b.CategoryID == selectedCategory);
            }

            // 图书类别
            List<BookCategory> categoryList = new List<BookCategory> { new BookCategory { ID = 0, Name = "全部" } };
            categoryList.AddRange(AppDbContext.BookCategory);

            ViewBag.BookCategory = categoryList; // 分类列表
            ViewBag.SearchKey = searchKey; // 设置搜索文本框
            //ViewBag.PageNumber = pageNumber; // 更新时定位到当前页
            ViewBag.SelectedCategory = selectedCategory;  // 选中的图书分类

            return View(query.OrderByDescending(b => b.ID).ToPagedList(pageNumber, pageSize));
        }

        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = AppDbContext.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(AppDbContext.BookCategory, "ID", "Name");
            return View();
        }

        // POST: Book/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Img,Name,Author,Describe,Price,Disable,CategoryID")] Book book)
        {
            if (ModelState.IsValid)
            {
                AppDbContext.Book.Add(book);
                AppDbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(AppDbContext.BookCategory, "ID", "Name", book.CategoryID);
            return View(book);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = AppDbContext.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(AppDbContext.BookCategory, "ID", "Name", book.CategoryID);
            return View(book);
        }

        // POST: Book/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Img,Name,Author,Describe,Price,Disable,CategoryID")] Book book)
        {
            if (ModelState.IsValid)
            {
                AppDbContext.Entry(book).State = EntityState.Modified;
                AppDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(AppDbContext.BookCategory, "ID", "Name", book.CategoryID);
            return View(book);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = AppDbContext.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = AppDbContext.Book.Find(id);
            AppDbContext.Book.Remove(book);
            AppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

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
