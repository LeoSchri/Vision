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
    public class InformationController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var information = db.Information.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Information = information;

            Session["InformationId"] = null;

            return pc.AddHomePage("Information", information);
        }

        public ActionResult Index()
        {
            var information = db.Information.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Information = information;

            Session["InformationId"] = null;

            return pc.AddIndexPage("Information", information);
        }

        public ActionResult List()
        {
            var information = db.Information.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Information = information;

            Session["InformationId"] = null;

            return pc.AddListPage("Information", information);
        }

        public ActionResult Details(int? id)
        {
            Information information = null;
            if (id == null)
            {
                information = db.Information.Find(PageManager.CurrentInformation.ID);
            }
            else
            {
                information = db.Information.Find(id);
            }
            if (information == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentInformation = information;
            Session["InformationId"] = null;

            return pc.AddDetailsPage("Information", information, 3);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("Information", new Information(), 2, new List<int>() { 2, 3 });
        }

        public ActionResult CreateChild(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }
            Session["InformationId"] = information.ID;
            Session["Type"] = "Create";

            return View("PageView");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Information";
            if (PageManager.CurrentInformation == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentInformation = information;
            return pc.AddEditPage("Information", information, 2, new List<int>() { 2, 3 });
        }

        public ActionResult EditChild(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }

            Session["InformationId"] = information.ID;
            Session["Type"] = "Edit";

            return View("PageView");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Information";
            if (PageManager.CurrentInformation == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentInformation = information;

            return pc.AddDeletePage("Information", information, 3);
        }

        public ActionResult DeleteChild(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }

            Session["InformationId"] = information.ID;
            Session["Type"] = "Delete";

            return View("PageView");
        }

        // POST: Information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateChild([Bind(Include = "Content,Name")] Information information)
        {
            pc.RemovePage("CreateChild", "Information");

            if (ModelState.IsValid)
            {
                information.LastModified = DateTime.Now;
                information.ProjectId = PageManager.CurrentProject.ID;
                var parent = db.Information.Find(Session["InformationId"]);
                information.ParentId = parent.ID;
                db.Information.Add(information);
                db.SaveChanges();

                Session["InformationId"] = null;
                return RedirectToAction("Details", new { id = parent.ID });
            }

            return RedirectToAction("CreateChild", new { id = Session["InformationId"] });
        }

        // POST: Information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Content,Name")] Information information)
        {
            pc.RemovePage("Create", "Information");

            if (ModelState.IsValid)
            {
                information.LastModified = DateTime.Now;
                information.ProjectId = PageManager.CurrentProject.ID;
                db.Information.Add(information);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = information.ID });
            }

            return RedirectToAction("Create");
        }

        // POST: Information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Name,ProjectId")] Information information)
        {
            pc.RemovePage("Edit", "Information");

            if (ModelState.IsValid)
            {
                db.Entry(information).State = EntityState.Modified;
                information.LastModified = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return RedirectToAction("Edit", new { id = information.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditChild([Bind(Include = "ID,Content,Name,ParentId,ProjectId")] Information information)
        {
            pc.RemovePage("EditChild", "Information");

            if (ModelState.IsValid)
            {
                db.Entry(information).State = EntityState.Modified;
                information.LastModified = DateTime.Now;
                db.SaveChanges();

                Session["InformationId"] = null;
                return RedirectToAction("Details");
            }
            return RedirectToAction("Edit", new { id = information.ID });
        }

        // POST: Information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete", "Information");

            Information information = db.Information.Find(id);
            Helper.RemoveInformationReferences(information);
            db.Information.Remove(information);
            db.SaveChanges();

            return RedirectToAction("Home");
        }

        [HttpPost, ActionName("DeleteChild")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteChildConfirmed(int id)
        {
            pc.RemovePage("Delete", "Information");

            Information information = db.Information.Find(id);
            Helper.RemoveInformationReferences(information);
            db.Information.Remove(information);
            db.SaveChanges();

            Session["InformationId"] = null;

            return RedirectToAction("Details");
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
