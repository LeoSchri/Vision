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
    public class ScenesController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var scenes = db.Scenes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Scenes = scenes;

            return pc.AddHomePage("Scenes", scenes);
        }

        public ActionResult Index()
        {
            var scenes = db.Scenes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Scenes = scenes;

            return pc.AddIndexPage("Scenes", scenes);
        }

        public ActionResult IndexToList()
        {
            var scenes = db.Scenes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Scenes = scenes;

            if(PageManager.CurrentChapter != null)
            {
                pc.ExchangePages("Details", "Chapters", "List", "Scenes", scenes);
            }
            else if (PageManager.CurrentPlotline != null)
            {
                pc.ExchangePages("Details", "Plotlines", "List", "Scenes", scenes);
            }

            return View("PageView");
        }

        public ActionResult List()
        {
            var scenes = db.Scenes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Scenes = scenes;

            return pc.AddListPage("Scenes", scenes);
        }

        public ActionResult Details(int? id)
        {
            Scene scene = null;
            if (id == null)
            {
                scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            }
            else
            {
                scene = db.Scenes.Find(id);
            }
            if (scene == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentScene = scene;
            pc.RemoveAllPages("Characters");
            pc.RemoveAllPages("CharacterEvolvements");
            pc.RemoveAllPages("RelationshipPhases");
            pc.RemoveAllPages("Locations");
            return pc.AddDetailsPage("Scenes", scene, 8);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("Scenes", new Scene(), 5, new List<int>() { 2, 3, 3, 1, 1 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scene scene = db.Scenes.Find(id);
            if (scene == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentScene == null)
            {
                if (PageManager.CurrentChapter != null)
                {
                    Session["destinedController"] = "Chapters";
                }
                else if (PageManager.CurrentPlotline != null)
                {
                    Session["destinedController"] = "Plotlines";
                }
                else
                {
                    Session["destinedMethod"] = "Home";
                    Session["destinedController"] = "Scenes";
                }
            }
            else
            {
                Session["destinedController"] = "Scenes";
            }

            PageManager.CurrentScene = scene;

            return pc.AddEditPage("Scenes", scene, 5, new List<int>() { 2, 3, 3, 1, 1 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scene scene = db.Scenes.Find(id);
            if (scene == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentScene == null)
            {
                if (PageManager.CurrentChapter != null)
                {
                    Session["destinedController"] = "Chapters";
                }
                else if (PageManager.CurrentPlotline != null)
                {
                    Session["destinedController"] = "Plotlines";
                }
                else
                {
                    Session["destinedMethod"] = "Home";
                    Session["destinedController"] = "Scenes";
                }
            }
            else
            {
                Session["destinedController"] = "Scenes";
            }

            PageManager.CurrentScene = scene;

            return pc.AddDeletePage("Scenes", scene, 9);
        }

        public ActionResult PickForC()
        {
            var scenes = db.Scenes.Where(c => c.ProjectId == PageManager.CurrentProject.ID && c.ChapterId == null).ToList();
            Session["destinedController"] = "Chapters";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindSceneToChapter";

            return pc.AddPickManyPage("Scenes", scenes);
        }

        public ActionResult PickForP()
        {
            var plotline = PageManager.CurrentPlotline;
            var scenes = db.Scenes.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickableScenes = new List<Scene>();
            scenes.ForEach(s =>
            {
                if(s.Plotlines == null || !s.Plotlines.Any())
                {
                    pickableScenes.Add(s);
                }
                else
                {
                    var contains = false;
                    s.Plotlines.ToList().ForEach(p =>
                    {
                        if (p.ID == plotline.ID)
                            contains = true;
                    });

                    if(!contains)
                    {
                        pickableScenes.Add(s);
                    }
                }
            });
            Session["destinedController"] = "Plotlines";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindSceneToPlotline";

            return pc.AddPickManyPage("Scenes", pickableScenes);
        }

        public ActionResult PickForDS()
        {
            var scene = PageManager.CurrentScene;
            var scenes = db.Scenes.Where(c => c.ProjectId == PageManager.CurrentProject.ID && c.ChapterId == null).ToList();
            var pickableScenes = RemoveUsedScenes(scenes,scene);
            Session["destinedController"] = "Scenes";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindSceneToScene";

            return pc.AddPickManyPage("Scenes", pickableScenes);
        }

        // POST: Scenes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Mood,Weather,Name,Summary")] Scene scene)
        {
            pc.RemovePage("Create","Scenes");

            if (ModelState.IsValid)
            {
                scene.LastModified = DateTime.Now;
                scene.ProjectId = PageManager.CurrentProject.ID;
                scene.OrderID = null;

                var plotline = db.Plotlines.Find(PageManager.CurrentPlotline.ID);
                scene.Plotlines.Add(plotline);
                db.Scenes.Add(scene);
                plotline.Scenes.Add(scene);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = scene.ID });
            }

            return RedirectToAction("Create");
        }

        // POST: Scenes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Mood,Weather,Name,Summary")] Scene scene)
        {
            pc.RemovePage("Edit","Scenes");

            if (ModelState.IsValid)
            {
                db.Entry(scene).State = EntityState.Modified;
                scene.LastModified = DateTime.Now;
                scene.ProjectId = PageManager.CurrentProject.ID;
                db.SaveChanges();
                if ((string)Session["destinedController"] == "Scenes" && (string)Session["destinedMethod"] == "Details")
                {
                    return RedirectToAction("Details", new { id = scene.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"], (string)Session["destinedController"]);
                }
            }
            return RedirectToAction("Edit", new { id = scene.ID });
        }

        // POST: Scenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete","Scenes");

            Scene scene = db.Scenes.Find(id);

            Helper.RemoveSceneReferences(scene);

            db.Scenes.Remove(scene);
            db.SaveChanges();
            if(PageManager.CurrentPlotline != null)
            {
                return RedirectToAction("Details", "Plotlines");
            }
            else
            {
                return RedirectToAction("Index", "Scenes");
            }
        }

        public ActionResult BindSceneToChapter(int? id)
        {
            var chapter = db.Chapters.Find(PageManager.CurrentChapter.ID);
            Scene scene = null;
            if (id != null)
            {
                scene = db.Scenes.Find(id);
                if (chapter == null || scene == null)
                {
                    return HttpNotFound();
                }
            }

            int? desiredOrderID = null;
            var chapters = GetChapters();

            if(chapters.Any())
            {
                chapter = chapters.Find(c => c.ID == chapter.ID);
                if (chapter!= null)
                {
                    var chapterIndex = chapters.IndexOf(chapter);
                    if(chapter.Scenes != null && chapter.Scenes.Any())
                    {
                        desiredOrderID = chapter.Scenes.LastOrDefault().OrderID + 1;
                    }
                    else
                    {
                        Chapter lastChapter = null;
                        var lastChapterIndex = chapterIndex - 1;
                        if(lastChapterIndex < 0)
                        {
                            desiredOrderID = 1;
                        }
                        else
                        {
                            lastChapter = chapters[lastChapterIndex];
                            while(lastChapter.Scenes == null || !lastChapter.Scenes.Any())
                            {
                                lastChapterIndex = lastChapterIndex - 1;
                                if (lastChapterIndex < 0)
                                {
                                    desiredOrderID = 1;
                                }
                                lastChapter = chapters[lastChapterIndex];
                            }
                        }

                        if(lastChapter != null)
                        {
                            desiredOrderID = lastChapter.Scenes.LastOrDefault().OrderID + 1;
                        }
                    }
                    if (chapterIndex != chapters.Count - 1)
                    {
                        for (int i = chapterIndex+1; i < chapters.Count; i++)
                        {
                            chapters[i].Scenes.ToList().ForEach(s =>
                            {
                                db.Entry(s).State = EntityState.Modified;
                                s.OrderID = s.OrderID + 1;
                                db.SaveChanges();
                            });
                        }
                    }
                }
            }

            db.Entry(scene).State = EntityState.Modified;
            scene.OrderID = desiredOrderID;
            db.SaveChanges();

            chapter.Scenes.Add(scene);
            scene.Chapter = chapter;
            scene.ChapterId = chapter.ID;
            db.SaveChanges();

            pc.RemovePage("PickMany", "Scenes");

            var scenes = db.Scenes.Where(c => c.ProjectId == PageManager.CurrentProject.ID && c.ChapterId == null).ToList();
            if(scenes == null || !scenes.Any())
            {
                return RedirectToAction("Details","Chapters", new { id = chapter.ID });
            }
            else
            {
                return RedirectToAction("PickForC");
            }
        }

        public ActionResult RemoveFromChapter(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chapter = db.Chapters.Find(PageManager.CurrentChapter.ID);
            var scene = db.Scenes.Find(id);
            if (chapter == null || scene == null)
            {
                return HttpNotFound();
            }

            scene.OrderID = null;

            chapter.Scenes.Remove(scene);
            scene.ChapterId = null;
            scene.Chapter = null;
            db.SaveChanges();

            return RedirectToAction("Details", "Chapters", new { id = chapter.ID });
        }

        public ActionResult BindSceneToPlotline(int? id)
        {
            var plotline = db.Plotlines.Find(PageManager.CurrentPlotline.ID);
            Scene scene = null;
            if (id != null)
            {
                scene = db.Scenes.Find(id);
                if (plotline == null || scene == null)
                {
                    return HttpNotFound();
                }
            }

            plotline.Scenes.Add(scene);
            scene.Plotlines.Add(plotline);
            db.SaveChanges();
            pc.RemovePage("PickMany", "Scenes");

            return RedirectToAction("PickForP");
        }

        public ActionResult RemoveFromPlotline(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var plotline = db.Plotlines.Find(PageManager.CurrentPlotline.ID);
            var scene = db.Scenes.Find(id);
            if (plotline == null || scene == null)
            {
                return HttpNotFound();
            }

            plotline.Scenes.Remove(scene);
            scene.Plotlines.Remove(plotline);
            db.SaveChanges();

            return RedirectToAction("Details", "Plotlines", new { id = plotline.ID });
        }

        public ActionResult BindSceneToScene(int? id)
        {
            var sceneOrg = db.Scenes.Find(PageManager.CurrentScene.ID);
            Scene scene = null;
            if (id != null)
            {
                scene = db.Scenes.Find(id);
                if (sceneOrg == null || scene == null)
                {
                    return HttpNotFound();
                }
            }

            sceneOrg.DependentScenesF.Add(scene);
            scene.DependentScenesA.Add(sceneOrg);
            db.SaveChanges();
            pc.RemovePage("PickMany", "Scenes");

            return RedirectToAction("PickForDS");
        }

        public ActionResult RemoveFromScene(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sceneOrg = db.Scenes.Find(PageManager.CurrentScene.ID);
            var scene = db.Scenes.Find(id);
            if (sceneOrg == null || scene == null)
            {
                return HttpNotFound();
            }

            sceneOrg.DependentScenesF.Remove(scene);
            scene.DependentScenesA.Remove(sceneOrg);
            db.SaveChanges();

            return RedirectToAction("Details", "Scenes", new { id = sceneOrg.ID });
        }

        public ActionResult MoveUp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var scenes = db.Scenes.Where(s => s.ProjectId == PageManager.CurrentProject.ID);
            Scene scene = db.Scenes.Find(id);
            if (scene == null)
            {
                return HttpNotFound();
            }

            var desiredOrderID = scene.OrderID-1;
            var sceneWithDesiredOrderID = scenes.ToList().Find(s => s.OrderID == desiredOrderID);
            if(sceneWithDesiredOrderID != null)
            {
                db.Entry(sceneWithDesiredOrderID).State = EntityState.Modified;
                sceneWithDesiredOrderID.OrderID = scene.OrderID;
                db.SaveChanges();
                db.Entry(scene).State = EntityState.Modified;
                scene.OrderID = desiredOrderID;
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Chapters");
        }

        public ActionResult MoveDown(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var scenes = db.Scenes.Where(s => s.ProjectId == PageManager.CurrentProject.ID);
            Scene scene = db.Scenes.Find(id);
            if (scene == null)
            {
                return HttpNotFound();
            }

            var desiredOrderID = scene.OrderID + 1;
            var sceneWithDesiredOrderID = scenes.ToList().Find(s => s.OrderID == desiredOrderID);
            if (sceneWithDesiredOrderID != null)
            {
                sceneWithDesiredOrderID.OrderID = scene.OrderID;
                db.SaveChanges();
                db.Entry(scene).State = EntityState.Modified;
                scene.OrderID = desiredOrderID;
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Chapters");
        }

        public ActionResult MoveToPrevious(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scene scene = db.Scenes.Find(id);
            if (scene == null)
            {
                return HttpNotFound();
            }

            var chapter = db.Chapters.Find(PageManager.CurrentChapter.ID);
            var chapters = GetChapters();

            var chap = chapters.Find(c => c.ID == chapter.ID);
            if(chap != null)
            {
                chap.Scenes.Remove(scene);

                var previousChapterIndex = chapters.IndexOf(chap) - 1;
                var previousChapter = chapters[previousChapterIndex];
                previousChapter.Scenes.Add(scene);
                scene.ChapterId = previousChapter.ID;

                db.SaveChanges();
            }

            return RedirectToAction("Details", "Chapters");
        }

        public ActionResult MoveToNext(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scene scene = db.Scenes.Find(id);
            if (scene == null)
            {
                return HttpNotFound();
            }

            var chapter = db.Chapters.Find(PageManager.CurrentChapter.ID);
            var chapters = GetChapters();

            var chap = chapters.Find(c => c.ID == chapter.ID);
            if (chap != null)
            {
                chap.Scenes.Remove(scene);

                var nextChapterIndex = chapters.IndexOf(chap) +1;
                var nextChapter = chapters[nextChapterIndex];
                nextChapter.Scenes.Add(scene);
                scene.ChapterId = nextChapter.ID;

                db.SaveChanges();
            }

            return RedirectToAction("Details", "Chapters");
        }

        private List<Chapter> GetChapters()
        {
            var chapters = new List<Chapter>();
            var books = db.Books.Where(b => b.ProjectId == PageManager.CurrentProject.ID);
            var arcs = new List<Arc>();

            if (books != null && books.Any())
            {
                books.OrderBy(b => b.ID).ToList().ForEach(b =>
                {
                    arcs.AddRange(b.Arcs.OrderBy(a => a.ID));
                });

                if (arcs.Any())
                {
                    arcs.ToList().ForEach(a =>
                    {
                        chapters.AddRange(a.Chapters.OrderBy(c => c.ID));
                    });
                }
            }

            return chapters;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Scene> RemoveUsedScenes(List<Scene> scenes, Scene scene)
        {
            var pickableScenes = new List<Scene>();
            if (scenes != null && scenes.Any())
            {
                scenes.ForEach(p =>
                {
                    if (scene != p && (scene.DependentScenesF == null || !scene.DependentScenesF.Any() || !scene.DependentScenesF.Contains(p)) && (scene.DependentScenesA == null || !scene.DependentScenesA.Any() || !scene.DependentScenesA.Contains(p)))
                    {
                        pickableScenes.Add(p);
                    }
                });
            }

            return pickableScenes;
        }
    }
}
