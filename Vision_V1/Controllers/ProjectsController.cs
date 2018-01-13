using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vision_V1.Models;
using Vision_V1.ViewModels;
using static Vision_V1.Models.Types;

namespace Vision_V1.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var userID = User.Identity.GetUserId();
            var projects = db.Projects.Where(b => b.ApplicationUserId == userID).ToList();
            PageManager.Projects = projects;

            return pc.AddHomePage("Projects", projects);
        }

        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var projects = db.Projects.Where(b => b.ApplicationUserId == userID).ToList();
            PageManager.Projects = projects;

            return pc.AddIndexPage("Projects", projects);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("Projects", new Project(), 1, new List<int>() { 2 },true);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            return pc.AddEditPage("Projects", project,1, new List<int>() { 2 }, true);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentProject = project;
            return pc.AddDeletePage("Projects", project,5, standAlone:true);
        }

        public ActionResult Load(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            if(ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            }
            PageManager.CurrentProject = project;

            return RedirectToAction("Index", "Projects");
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.ApplicationUserId = User.Identity.GetUserId();
                project.LastModified = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            return RedirectToAction("Create");
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FolderLocation,BackupLocation,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                project.ApplicationUserId = User.Identity.GetUserId();
                project.LastModified = DateTime.Now;
                db.SaveChanges();

                return RedirectToAction("Home");
            }
            return RedirectToAction("Edit");
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            Helper.RemoveReferences(project);
            db.Projects.Remove(project);
            db.SaveChanges();

            if(PageManager.CurrentProject == project)
            {
                PageManager.CurrentProject = null;
            }

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
