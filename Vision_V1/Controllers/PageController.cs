using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vision_V1.Models;
using Vision_V1.ViewModels;

namespace Vision_V1.Controllers
{
    public class PageController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult AddPage(string name, string controller, object model, int propertyCount = 0, List<int> propertySizes = null, bool standAlone = false)
        {
            var page = new Page(name, controller, model, propertyCount, propertySizes);

            if(standAlone)
            {
                PageManager.Reset();
                PageManager.Pages = new List<Page>
                {
                    page
                };
            }
            else
            {
                var index = Helper.RemoveIfExists(new Page(name, controller, null));
                if (index != null)
                {
                    if (index < PageManager.Pages.Count)
                    {
                        PageManager.Pages.Insert((int)index, page);
                    }
                    else
                    {
                        PageManager.Pages.Add(page);
                    }
                }
                else
                {
                    PageManager.Pages.Add(page);
                }
            }

            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddHomePage(string controller, object model)
        {
            var page = new Page("Index", controller, model);

            PageManager.Reset();
            PageManager.Pages = new List<Page>
            {
                page
            };
            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddIndexPage(string controller, object model, bool standAlone = false)
        {
            var page = new Page("Index", controller, model);

            if(standAlone)
            {
                PageManager.Pages = new List<Page>();
                PageManager.StructureId = 1;

                PageManager.Pages.Add(page);
            }
            else
            {
                var index = Helper.RemoveIfExists(new Page("List", controller, null));
                var index2 = Helper.RemoveIfExists(new Page("Index", controller, null));
                if (index != null)
                {
                    if(index < PageManager.Pages.Count)
                    {
                        PageManager.Pages.Insert((int)index, page);
                    }
                    else
                    {
                        PageManager.Pages.Add(page);
                    }
                }
                else if(index2 != null)
                {
                    PageManager.Pages.Insert((int)index2, page);
                }
                else
                {
                    PageManager.Pages.Add(page);
                }
            }
            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddListPage(string controller, object model)
        {
            var page = new Page("List", controller, model);

            var index = Helper.RemoveIfExists(new Page("List", controller, null));
            var index2 = Helper.RemoveIfExists(new Page("Index", controller, null));
            if (index != null)
            {
                if (index < PageManager.Pages.Count)
                {
                    PageManager.Pages.Insert((int)index, page);
                }
                else
                {
                    PageManager.Pages.Add(page);
                }
            }
            else if (index2 != null)
            {
                PageManager.Pages.Insert((int)index2, page);
            }
            else
            {
                PageManager.Pages.Add(page);
            }
            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddDetailsPage(string controller, object model, int propertyCount = 0, bool standAlone = false)
        {
            var page = new Page("Details", controller, model, propertyCount);

            if (standAlone)
            {
                PageManager.Pages = new List<Page>
                {
                    page
                };
            }
            else
            {
                Helper.RemoveIfExists(new Page("Edit", controller, null));
                Helper.RemoveIfExists(new Page("Delete", controller, null));
                Helper.RemoveIfExists(new Page("Details", controller, null));

                ReloadModel(controller);

                switch(controller)
                {
                    case "Scenes": ReloadModel("Plotlines"); break;
                    case "Chapters": ReloadModel("Arcs"); break;
                    case "Arcs": ReloadModel("Books"); break;
                    case "RelationshipPhases": ReloadModel("Relationships"); break;
                    case "CharacterEvolvements": ReloadModel("Characters"); break;
                }

                PageManager.Pages.Add(page);
            }
            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddCreatePage(string controller, object model, int propertyCount = 0, List<int> propertySizes = null, bool standAlone = false)
        {
            var page = new Page("Create", controller, model, propertyCount, propertySizes);

            if (standAlone)
            {
                PageManager.Pages = new List<Page>
                {
                    page
                };
            }
            else
            {
                Helper.RemoveIfExists(new Page("Create", controller, null));
                PageManager.Pages.Add(page);
            }
            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddCreateChildPage(string controller, object model, int propertyCount = 0, List<int> propertySizes = null, bool standAlone = false)
        {
            var page = new Page("CreateChild", controller, model, propertyCount, propertySizes);

            if (standAlone)
            {
                PageManager.Pages = new List<Page>
                {
                    page
                };
            }
            else
            {
                Helper.RemoveIfExists(new Page("CreateChild", controller, null));
                PageManager.Pages.Add(page);
            }
            Helper.SetStructureId();

            return View("PageView");
        }


        public ActionResult AddEditPage(string controller, object model, int propertyCount = 0, List<int> propertySizes = null, bool standAlone = false)
        {
            var page = new Page("Edit", controller, model, propertyCount, propertySizes);

            if (standAlone)
            {
                PageManager.Pages = new List<Page>
                {
                    page
                };
            }
            else
            {
                Helper.RemoveIfExists(new Page("Delete", controller, null));
                Helper.RemoveIfExists(new Page("Details", controller, null));
                Helper.RemoveIfExists(new Page("Edit", controller, null));
                PageManager.Pages.Add(page);
            }
            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddDeletePage(string controller, object model, int propertyCount = 0, bool standAlone = false)
        {
            var page = new Page("Delete", controller, model, propertyCount);

            if (standAlone)
            {
                PageManager.Pages = new List<Page>
                {
                    page
                };
            }
            else
            {
                Helper.RemoveIfExists(new Page("Delete", controller, null));
                Helper.RemoveIfExists(new Page("Edit", controller, null));
                Helper.RemoveIfExists(new Page("Details", controller, null));
                PageManager.Pages.Add(page);
            }
            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddPickOnePage(string controller, object model)
        {
            var page = new Page("PickOne", controller, model);

            Helper.RemoveIfExists(new Page("PickOne", controller, null));
            var index = Helper.RemoveIfExists(new Page("PickMany", controller, null));
            if (index != null)
            {
                if (index < PageManager.Pages.Count)
                {
                    PageManager.Pages.Insert((int)index, page);
                }
                else
                {
                    PageManager.Pages.Add(page);
                }
            }
            else
            {
                PageManager.Pages.Add(page);
            }

            Helper.SetStructureId();

            return View("PageView");
        }

        public ActionResult AddPickManyPage(string controller, object model)
        {
            var page = new Page("PickMany", controller, model);

            Helper.RemoveIfExists(new Page("PickMany", controller, null));
            var index = Helper.RemoveIfExists(new Page("PickMany", controller, null));
            if (index != null)
            {
                if (index < PageManager.Pages.Count)
                {
                    PageManager.Pages.Insert((int)index, page);
                }
                else
                {
                    PageManager.Pages.Add(page);
                }
            }
            else
            {
                PageManager.Pages.Add(page);
            }

            Helper.SetStructureId();

            return View("PageView");
        }

        public void RemovePage(string name, string controller)
        {
            var page = PageManager.Pages.Find(p => p.Name == name && p.Controller == controller);
            if (page != null)
            {
                PageManager.Pages.Remove(page);
                UpdatePageManager(page);
            }

            Helper.SetStructureId();
        }

        public ActionResult RemovePageWID(int? id)
        {
            if(id!= null)
            {
                var page = PageManager.Pages.ElementAt((int)id);
                RemovePage(page.Name, page.Controller);
            }

            if (PageManager.Pages.Count == 1 && PageManager.Pages.FirstOrDefault().Name== "List")
            {
                var listPage = PageManager.Pages.FirstOrDefault();
                ExchangePages(listPage.Name, listPage.Controller,"Index", listPage.Controller, listPage.Model);
                ReloadModel(listPage.Controller);
            }


            return View("PageView");
        }

        public void RemoveAllPages(string controller)
        {
            RemovePage("Index",controller);
            RemovePage("List",controller);
            RemovePage("Create",controller);
            RemovePage("Edit",controller);
            RemovePage("Details",controller);
            RemovePage("Delete",controller);
            RemovePage("PickOne",controller);
            RemovePage("PickMany",controller);
        }

        public void ExchangePages(string name1, string controller1,string name2, string controller2, object model2)
        {
            var page = new Page(name2,controller2,model2);

            var index = Helper.RemoveIfExists(new Page(name2, controller2, null));
            var index2 = Helper.RemoveIfExists(new Page(name1, controller1, null));
            if (index2 != null)
            {
                if (index2 < PageManager.Pages.Count)
                {
                    PageManager.Pages.Insert((int)index2, page);
                }
                else
                {
                    PageManager.Pages.Add(page);
                }
            }
            else if(index != null)
            {
                if (index < PageManager.Pages.Count)
                {
                    PageManager.Pages.Insert((int)index, page);
                }
                else
                {
                    PageManager.Pages.Add(page);
                }
            }
            else
            {
                PageManager.Pages.Add(page);
            }
        }

        public ActionResult ChangeStructure(int? id)
        {
            if(id != null)
            {
                PageManager.StructureId = (int)id;
            }

            return View("PageView");
        }

        private void ReloadModel(string controller)
        {
            var page = PageManager.Pages.Find(p => (p.Name == "Index" || p.Name == "List") && p.Controller == controller);
            if (page != null)
            {
                switch (controller)
                {
                    case "Books": page.Model = db.Books.Where(l => l.ProjectId == PageManager.CurrentProject.ID).ToList(); break;
                    case "Arcs": page.Model = db.Arcs.Where(l => l.BookId == PageManager.CurrentBook.ID).ToList(); break;
                    case "Chapters": page.Model = db.Chapters.Where(l => l.ArcId == PageManager.CurrentArc.ID).ToList(); break;
                    case "Plotlines": page.Model = db.Plotlines.Where(l => l.ProjectId == PageManager.CurrentProject.ID).ToList(); break;
                    case "Scenes":
                        if (PageManager.CurrentPlotline != null)
                        {
                            page.Model = db.Scenes.Where(l => l.Plotlines.Contains(PageManager.CurrentPlotline)).ToList(); break;
                        }
                        else if (PageManager.CurrentChapter != null)
                        {
                            page.Model = db.Scenes.Where(l => l.ChapterId == PageManager.CurrentChapter.ID).ToList(); break;
                        }
                        else
                        {
                            page.Model = db.Scenes.Where(l => l.ProjectId == PageManager.CurrentProject.ID).ToList(); break;
                        }
                    case "Characters": page.Model = db.Characters.Where(l => l.ProjectId == PageManager.CurrentProject.ID).ToList(); break;
                    case "Locations": page.Model = db.Locations.Where(l => l.ProjectId == PageManager.CurrentProject.ID).ToList(); break;
                    case "Information": page.Model = db.Information.Where(l => l.ProjectId == PageManager.CurrentProject.ID).ToList(); break;
                }
            }

            var page2 = PageManager.Pages.Find(p => p.Name == "Details" && p.Controller == controller);
            if (page2 != null)
            {
                switch (controller)
                {
                    case "Books": page2.Model = db.Books.Where(l => l.ID == PageManager.CurrentBook.ID).FirstOrDefault(); break;
                    case "Arcs": page2.Model = db.Arcs.Where(l => l.ID == PageManager.CurrentArc.ID).FirstOrDefault(); break;
                    case "Chapters": page2.Model = db.Chapters.Where(l => l.ID == PageManager.CurrentChapter.ID).FirstOrDefault(); break;
                    case "Plotlines": page2.Model = db.Plotlines.Where(l => l.ID == PageManager.CurrentPlotline.ID).FirstOrDefault(); break;
                    case "Scenes": page2.Model = db.Scenes.Where(l => l.ID == PageManager.CurrentScene.ID).ToList().FirstOrDefault(); break;
                    case "Characters": page2.Model = db.Characters.Where(l => l.ID == PageManager.CurrentCharacter.ID).FirstOrDefault(); break;
                    case "Locations": page2.Model = db.Locations.Where(l => l.ID == PageManager.CurrentLocation.ID).FirstOrDefault(); break;
                    case "Information": page2.Model = db.Information.Where(l => l.ID == PageManager.CurrentInformation.ID).FirstOrDefault(); break;
                }
            }
        }

        private void UpdatePageManager(Page page)
        {
            switch(page.Name)
            {
                case "Index":
                case "List": ClearList(page.Controller); break;
                case "Create":
                case "Edit":
                case "Delete":
                case "Details": ClearList($"Current{page.Controller.Substring(0,page.Controller.Length-2)}"); break;
            }
        }

        private void ClearList(string name)
        {
            var pc = new PageManager();
            var props = pc.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name.Contains(name))
                {
                    prop.SetValue(pc, null);
                }
            }
        }
    }
}