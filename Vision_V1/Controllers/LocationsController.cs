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
    public class LocationsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var locations = db.Locations.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Locations = locations;

            return pc.AddHomePage("Locations", locations);
        }

        public ActionResult Index()
        {
            var locations = db.Locations.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Locations = locations;

            return pc.AddIndexPage("Locations", locations);
        }

        public ActionResult List()
        {
            var locations = db.Locations.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Locations = locations;

            return pc.AddListPage("Locations", locations);
        }

        public ActionResult Details(int? id)
        {
            Location location = null;
            if (id == null)
            {
                location = db.Locations.Find(PageManager.CurrentLocation.ID);
            }
            else
            {
                location = db.Locations.Find(id);
            }
            if (location == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentLocation = location;
            return pc.AddDetailsPage("Locations", location, 6);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("Locations", new Location(), 4, new List<int>() { 2, 1, 3, 1 });
        }

        public ActionResult CreateChild()
        {
            return pc.AddCreateChildPage("Locations", new Location(), 5, new List<int>() { 2, 1, 3, 1, 2 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Locations";
            if (PageManager.CurrentLocation == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentLocation = location;

            return pc.AddEditPage("Locations", location, 5, new List<int>() { 2, 1, 3, 1, 2 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Locations";
            if (PageManager.CurrentLocation == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentLocation = location;

            return pc.AddDeletePage("Locations", location, 6);
        }

        public ActionResult PickForL()
        {
            var scene = PageManager.CurrentScene;
            var locations = db.Locations.Where(l => l.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickableLocations = new List<Location>();
            locations.ForEach(l =>
            {
                if(l.Scenes == null || !l.Scenes.Any() || !l.Scenes.Contains(scene))
                {
                    pickableLocations.Add(l);
                }
            });
            Session["destinedController"] = "Scenes";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindLocationToScene";

            return pc.AddPickOnePage("Locations", pickableLocations);
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Climate,Type,Name,Description")] Location location)
        {
            pc.RemovePage("Create", "Locations");

            if (ModelState.IsValid)
            {
                location.LastModified = DateTime.Now;
                location.ProjectId = PageManager.CurrentProject.ID;
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = location.ID });
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateChild([Bind(Include = "Orientation,Climate,Type,Name,Description")] Location location)
        {
            pc.RemovePage("CreateChild", "Locations");

            if (ModelState.IsValid)
            {
                var parent = db.Locations.Find(PageManager.CurrentLocation.ID);

                location.ProjectId = PageManager.CurrentProject.ID;
                location.LastModified = DateTime.Now;
                location.ParentId = parent.ID;
                db.Locations.Add(location);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = location.ID });
            }

            return RedirectToAction("Create");
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Orientation,Climate,Type,Name,Description")] Location location)
        {
            pc.RemovePage("Edit", "Locations");

            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                location.ProjectId = PageManager.CurrentProject.ID;
                location.LastModified = DateTime.Now;
                db.SaveChanges();
                if ((string)Session["destinedMethod"] == "Details")
                {
                    return RedirectToAction("Details", new { id = location.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"]);
                }
            }
            return RedirectToAction("Edit", new { id = location.ID });
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete", "Locations");

            Location location = db.Locations.Find(id);

            Helper.RemoveLocationReferences(location);

            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult BindLocationToScene(int? id)
        {
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            Location location = null;
            if (id != null)
            {
                location = db.Locations.Find(id);
                if (scene == null || location == null)
                {
                    return HttpNotFound();
                }

                scene.Location = location;
                scene.LocationId = location.ID;
                location.Scenes.Add(scene);
            }
            else scene.LocationId = null;
            db.SaveChanges();

            pc.RemovePage("PickOne", "Locations");

            return RedirectToAction("Details", "Scenes", new { id = scene.ID });
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
