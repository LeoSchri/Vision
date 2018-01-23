using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vision_V1.Models;
using Vision_V1.ViewModels;

namespace Vision_V1.Controllers
{
    public class AttributesController : Controller
    {

        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var attributes = db.Attributes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Attributes = attributes;

            return pc.AddHomePage("Attributes", attributes);
        }

        public ActionResult Index()
        {
            var attributes = db.Attributes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Attributes = attributes;

            return pc.AddIndexPage("Attributes", attributes);
        }

        public ActionResult Create()
        {
            pc.RemovePage("Index", "Attributes");
            return pc.AddCreatePage("Attributes", new Models.Attribute(), 3, new List<int>() { 2, 1, 1 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Attribute attribute = db.Attributes.Find(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentAttribute = attribute;
            pc.RemovePage("Index", "Attributes");
            return pc.AddEditPage("Attributes", attribute, 3, new List<int>() { 2, 1, 1 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Attribute attribute = db.Attributes.Find(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentAttribute = attribute;
            pc.RemovePage("Index", "Attributes");
            return pc.AddDeletePage("Attributes", attribute, 3);
        }

        // POST: Attributes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Size,ShownInTableYN")] Models.Attribute attribute)
        {
            pc.RemovePage("Create", "Attributes");

            if (ModelState.IsValid)
            {
                attribute.ProjectId = PageManager.CurrentProject.ID;
                attribute.LastModified = DateTime.Now;

                var attributes = db.Attributes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).OrderBy(b=> b.OrderID).ToList();
                if(attributes == null || !attributes.Any())
                {
                    attribute.OrderID = 1;
                }
                else
                {
                    attribute.OrderID = attributes.LastOrDefault().OrderID + 1;
                }

                db.Attributes.Add(attribute);
                db.SaveChanges();

                var characters = db.Characters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
                if(characters != null && characters.Any())
                {
                    characters.ForEach(c =>
                    {
                        var characterAttribute = new CharacterAttribute() {Attribute = attribute, AttributeId = attribute.ID, Character = c, CharacterId = c.ID, Value=null};
                        db.CharacterAttributes.Add(characterAttribute);
                        db.SaveChanges();
                    });
                }

                return RedirectToAction("Home");
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Size,ShownInTableYN")] Models.Attribute attribute)
        {
            pc.RemovePage("Edit", "Attributes");

            if (ModelState.IsValid)
            {
                db.Entry(attribute).State = EntityState.Modified;
                attribute.ProjectId = PageManager.CurrentProject.ID;
                attribute.LastModified = DateTime.Now;

                db.SaveChanges();

                return RedirectToAction("Home");
            }

            return RedirectToAction("Edit");
        }

        // POST: Attributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete", "Attributes");

            Models.Attribute attribute = db.Attributes.Find(id);

            Helper.RemoveAttributeReferences(attribute);

            db.Attributes.Remove(attribute);
            db.SaveChanges();

            var characterAttributes = db.CharacterAttributes.Where(ca => ca.AttributeId == attribute.ID).ToList();
            if(characterAttributes != null && characterAttributes.Any())
            {
                characterAttributes.ForEach(ca =>
                {
                    db.CharacterAttributes.Remove(ca);
                    db.SaveChanges();
                });
            }

            return RedirectToAction("Home");
        }

        public ActionResult MoveUp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var attributes = db.Attributes.Where(s => s.ProjectId == PageManager.CurrentProject.ID);
            Models.Attribute attribute = db.Attributes.Find(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }

            var desiredOrderID = attribute.OrderID - 1;
            var attributeWithDesiredOrderID = attributes.ToList().Find(s => s.OrderID == desiredOrderID);
            if (attributeWithDesiredOrderID != null)
            {
                db.Entry(attributeWithDesiredOrderID).State = EntityState.Modified;
                attributeWithDesiredOrderID.OrderID = attribute.OrderID;
                db.SaveChanges();
                db.Entry(attribute).State = EntityState.Modified;
                attribute.OrderID = desiredOrderID;
                db.SaveChanges();
            }

            return RedirectToAction("Home", "Attributes");
        }

        public ActionResult MoveDown(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var attributes = db.Attributes.Where(s => s.ProjectId == PageManager.CurrentProject.ID);
            Models.Attribute attribute = db.Attributes.Find(id);
            if (attributes == null)
            {
                return HttpNotFound();
            }

            var desiredOrderID = attribute.OrderID + 1;
            var attributeWithDesiredOrderID = attributes.ToList().Find(s => s.OrderID == desiredOrderID);
            if (attributeWithDesiredOrderID != null)
            {
                attributeWithDesiredOrderID.OrderID = attribute.OrderID;
                db.SaveChanges();
                db.Entry(attribute).State = EntityState.Modified;
                attribute.OrderID = desiredOrderID;
                db.SaveChanges();
            }

            return RedirectToAction("Home", "Attributes");
        }
    }
}
