using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Vision_V1.Models;
using Vision_V1.ViewModels;

namespace Vision_V1.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var books = db.Books.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Books = books;

            return pc.AddHomePage("Books",books);
        }

        public ActionResult Index()
        {
            var books = db.Books.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Books = books;

            return pc.AddIndexPage("Books", books);
        }

        public ActionResult List()
        {
            var books = db.Books.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Books = books;

            return pc.AddListPage("Books", books);
        }

        public ActionResult Details(int? id)
        {
            Book book = null;
            if (id == null)
            {
                book = db.Books.Find(PageManager.CurrentBook.ID);
            }
            else
            {
                book = db.Books.Find(id);
            }
            if (book == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentBook = book;
            pc.RemoveAllPages("Arcs");
            return pc.AddDetailsPage("Books", book, 5);
        }

        public ActionResult Create()
        {
            pc.RemovePage("Index", "Books");
            return pc.AddCreatePage("Books", new Book(),3, new List<int>() { 2, 3, 1 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Books";
            if (PageManager.CurrentBook == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentBook = book;
            pc.RemovePage("Index", "Books");
            return pc.AddEditPage("Books", book,4, new List<int>() { 2, 3, 3, 1 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Books";
            if (PageManager.CurrentBook == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentBook = book;
            pc.RemovePage("Index", "Books");
            return pc.AddDeletePage("Books", book,5);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Summary,EstimatedDuration")] Book book)
        {
            pc.RemovePage("Create","Books");

            if (ModelState.IsValid)
            {
                book.ProjectId = PageManager.CurrentProject.ID;
                book.LastModified = DateTime.Now;
                db.Books.Add(book);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = book.ID });
            }

            return RedirectToAction("Create");
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Summary,EstimatedDuration")] Book book)
        {
            pc.RemovePage("Edit","Books");

            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                book.ProjectId = PageManager.CurrentProject.ID;
                book.LastModified = DateTime.Now;
                db.SaveChanges();

                if ((string)Session["destinedMethod"] == "Details")
                {
                    return RedirectToAction("Details", new { id = book.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"]);
                }
            }
            return RedirectToAction("Edit", new { id = book.ID });
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete","Books");

            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();

            return RedirectToAction("Home");
        }

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
