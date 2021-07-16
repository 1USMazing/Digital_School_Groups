using Microsoft.AspNet.Identity;
using Proiect_DSG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;
using System.Web.Mvc;

namespace Proiect_DSG.Controllers
{
    
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //GET: Categories
        [Authorize(Roles = "Utilizator,Moderator,Administrator")]
        public ActionResult Index()
        {
            var categories = from cat in db.Categories
                             select cat;
            ViewBag.Categories = categories;

            if (TempData.ContainsKey("Mesaj"))
            {
                ViewBag.Mesaj = TempData["Mesaj"].ToString();
            }

            SetAccesRights();

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Show(int id)
        {
            Category cat = db.Categories.Find(id);
            ViewBag.Category = cat;

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["Mesaj"] = "Categoria " + category.CategoryName + " a fost adaugata cu succes!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View();
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.CategoryName = requestCategory.CategoryName;
                    category.CategoryDescription = requestCategory.CategoryDescription;
                    db.SaveChanges();
                }
                TempData["Mesaj"] = "Categoria " + category.CategoryName + " a fost editata cu succes!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            TempData["Mesaj"] = "Categoria " + category.CategoryName + " a fost stearsa cu succes!";
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void SetAccesRights()
        {
            ViewBag.CurrentUser = User.Identity.GetUserId();

            if (User.IsInRole("Administrator")) ViewBag.IsAdmin = true;
            else ViewBag.IsAdmin = false;

            if (User.IsInRole("Moderator")) ViewBag.Autorizatie = true;
            else ViewBag.Autorizatie = false;
        }
    }
}