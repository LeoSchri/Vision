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
    public class CharactersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var characters = db.Characters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Characters = characters;

            var attributes = db.Attributes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Attributes = attributes;

            return pc.AddHomePage("Characters", characters);
        }

        public ActionResult Index()
        {
            var characters = db.Characters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Characters = characters;

            var attributes = db.Attributes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Attributes = attributes;

            return pc.AddIndexPage("Characters", characters);
        }

        public ActionResult IndexToList()
        {
            var characters = db.Characters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Characters = characters;
            pc.ExchangePages("Details", "Scenes", "List", "Characters", characters);

            return View("PageView");
        }

        public ActionResult List()
        {
            var characters = db.Characters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Characters = characters;

            return pc.AddListPage("Characters", characters);
        }

        public ActionResult Details(int? id)
        {
            Character character = null;
            if (id == null)
            {
                character = db.Characters.Find(PageManager.CurrentCharacter.ID);
            }
            else
            {
                character = db.Characters.Find(id);
            }
            if (character == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentCharacter = character;
            var propertyCount = 4;

            pc.RemoveAllPages("CharacterEvolvements");
            pc.RemoveAllPages("Relationships");

            pc.RemoveAllPages("Locations");

            return pc.AddDetailsPage("Characters", character, propertyCount);
        }

        public ActionResult Create()
        {
            var propertyCount = 2;
            var propertySizes = new List<int>{2,3};

            return pc.AddCreatePage("Characters", new Character(), propertyCount, propertySizes);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Characters";
            if (PageManager.CurrentCharacter == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            character.CharacterAttributes.ToList().ForEach(ca =>
            {
                ca.Attribute = db.Attributes.Find(ca.AttributeId);
            });

            PageManager.CurrentCharacter = character;
            var propertyCount = 2;
            var propertySizes = new List<int> { 2,3 };


            return pc.AddEditPage("Characters", character, propertyCount, propertySizes);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Characters";
            if (PageManager.CurrentCharacter == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentCharacter = character;
            var propertyCount = 4;

            return pc.AddDeletePage("Characters", character, propertyCount);
        }

        public ActionResult PickForMC()
        {
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID && !c.IsMainCharacter).ToList();
            Session["destinedController"] = "MainCharacters";
            Session["destinedMethod"] = "Home";
            Session["method"] = "MakeMC";

            return pc.AddPickManyPage("Characters", characters);
        }

        public ActionResult PickForAC()
        {
            var scene = PageManager.CurrentScene;
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickableCharacters = new List<Character>();
            characters.ForEach(c =>
            {
                var Scene = c.Scenes.ToList().Find(s => s.ID == scene.ID);
                if(c.Scenes == null || !c.Scenes.Any() || Scene == null)
                {
                    pickableCharacters.Add(c);
                }
            });

            Session["destinedController"] = "Scenes";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindAttendantCharacterToScene";

            return pc.AddPickManyPage("Characters", pickableCharacters);
        }

        public ActionResult PickForCPC()
        {
            var character = PageManager.CurrentCharacter;
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickableCharacters = new List<Character>();
            if (characters != null && characters.Any())
            {
                var pCharacters = characters.Where(c => c.ID != character.ID);

                if (pCharacters != null && pCharacters.Any())
                {
                    pCharacters.ToList().ForEach(c =>
                    {
                        var containsCharacter = false;
                        character.Relationships.ToList().ForEach(r =>
                        {
                            if (r.CounterpartCharacterId == c.ID)
                            {
                                containsCharacter = true;
                            }
                        });

                        if (!containsCharacter)
                        {
                            pickableCharacters.Add(c);
                        }
                    });
                }
            }

            Session["destinedController"] = "Characters";
            Session["destinedMethod"] = "Details";
            Session["method"] = "CreateRelationship";

            return pc.AddPickOnePage("Characters", pickableCharacters);
        }

        // POST: Characters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstCharacterNature,IsMainCharacter,CharacterAttributes,History,Name,ProjectId")] Character character)
        {
            Helper.RemoveBinding(ModelState, "ID");

            pc.RemovePage("Create","Characters");

            if (ModelState.IsValid)
            {
                character.ProjectId = PageManager.CurrentProject.ID;
                character.LastModified = DateTime.Now;
                db.Characters.Add(character);
                db.SaveChanges();

                if(!string.IsNullOrEmpty(character.FirstCharacterNature))
                {
                    var evolvementStep = new CharacterEvolvement()
                    {
                        Name = "Initial character",
                        Character = character,
                        CharacterId = character.ID,
                        CharacterDescription = character.FirstCharacterNature,
                        LastModified = DateTime.Now
                    };
                    db.CharacterEvolvements.Add(evolvementStep);
                    db.SaveChanges();

                    character.EvolvementSteps.Add(evolvementStep);
                }

                var attributes = db.Attributes.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
                if (attributes != null && attributes.Any())
                {
                    attributes.ForEach(a =>
                    {
                        var characterAttribute = new CharacterAttribute() { Attribute = a, AttributeId = a.ID, Character = character, CharacterId = character.ID, Value = null };
                        db.CharacterAttributes.Add(characterAttribute);
                        db.SaveChanges();
                    });
                }

                db.SaveChanges();



                return RedirectToAction("Home");
            }

            return RedirectToAction("Create");
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IsMainCharacter,CharacterAttributes,History,Name,ProjectId")] Character character)
        {
            pc.RemovePage("Edit","Characters");

            if (ModelState.IsValid)
            {
                db.Entry(character).State = EntityState.Modified;
                character.LastModified = DateTime.Now;
                db.SaveChanges();

                character.CharacterAttributes.ForEach(ca =>
                {
                    db.Entry(ca).State = EntityState.Modified;
                    db.SaveChanges();
                });

                if((string)Session["destinedMethod"] == "Details")
                {
                    return RedirectToAction("Details", new { id = character.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"]);
                }
            }
            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete","Characters");

            Character character = db.Characters.Find(id);

            Helper.RemoveCharacterReferences(character);

            db.Characters.Remove(character);
            db.SaveChanges();

            var relationships = db.Relationships.Where(x => x.CounterpartCharacter.ID == id);
            if(relationships != null)
            {
                relationships.ForEachAsync(relation =>
                {
                    db.Relationships.Remove(relation);
                });
            }

            return RedirectToAction("Index");
        }

        public ActionResult MakeMC(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }

            db.Entry(character).State = EntityState.Modified;
            character.LastModified = DateTime.Now;
            character.IsMainCharacter = true;
            db.SaveChanges();

            var mainCharacterController = new MainCharactersController();
            mainCharacterController.Create(new MainCharacter() { Name= character.Name }, character.ID);

            pc.RemovePage("PickMany", "Characters");

            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID && !c.IsMainCharacter).ToList();
            if(characters == null || !characters.Any())
            {
                return RedirectToAction("Home", "MainCharacters");
            }
            else
            {
                return RedirectToAction("PickForMC");
            }

        }

        public ActionResult BindAttendantCharacterToScene(int? id)
        {
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            Character character = null;
            if (id != null)
            {
                character = db.Characters.Find(id);
                if (scene == null || character == null)
                {
                    return HttpNotFound();
                }

                scene.AttendantCharacters.Add(character);
                character.Scenes.Add(scene);
                db.SaveChanges();
            }

            pc.RemovePage("PickMany", "Characters");
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickableCharacters = new List<Character>();
            characters.ForEach(c =>
            {
                if (c.Scenes == null || !c.Scenes.Any() || !c.Scenes.Contains(scene))
                {
                    pickableCharacters.Add(c);
                }
            });
            if (pickableCharacters == null || !pickableCharacters.Any())
            {
                return RedirectToAction("Details", "Scenes", new { id = scene.ID });
            }
            else
            {
                return RedirectToAction("PickForAC");
            }
        }

        public ActionResult RemoveAttendantCharacterFromScene(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var character = db.Characters.Find(id);
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            if (character == null || scene == null)
            {
                return HttpNotFound();
            }

            character.Scenes.Remove(scene);
            scene.AttendantCharacters.Remove(character);
            db.SaveChanges();

            return RedirectToAction("Details", "Scenes", new { id = scene.ID });
        }

        public ActionResult CreateRelationship(int? id)
        {
            var characterOrg = db.Characters.Find(PageManager.CurrentCharacter.ID);
            Character character = null;
            if (id != null)
            {
                character = db.Characters.Find(id);
                if (characterOrg == null || character == null)
                {
                    return HttpNotFound();
                }

                var relationship = new Relationship()
                {
                    Name = $"{characterOrg.Name}-{character.Name}",
                    Character = characterOrg,
                    CharacterId = characterOrg.ID,
                    CounterpartCharacter = character,
                    CounterpartCharacterId = character.ID,
                    LastModified = DateTime.Now
                };

                characterOrg.Relationships.Add(relationship);
                character.RelationshipCounterparts.Add(relationship);
                db.SaveChanges();

                var relationship2 = new Relationship()
                {
                    Name = $"{character.Name}-{characterOrg.Name}",
                    Character = character,
                    CharacterId = character.ID,
                    CounterpartCharacter = characterOrg,
                    CounterpartCharacterId = characterOrg.ID,
                    LastModified = DateTime.Now
                };

                character.Relationships.Add(relationship2);
                characterOrg.RelationshipCounterparts.Add(relationship2);
                db.SaveChanges();
            }

            pc.RemovePage("PickOne", "Characters");

            return RedirectToAction("Details", "Characters", new { id = characterOrg.ID });
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
