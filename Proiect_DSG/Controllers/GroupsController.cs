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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: Groups
        [Authorize(Roles = "Utilizator,Moderator,Administrator")]
        public ActionResult Index()
        {
            var groups = db.Groups.Include("Category").Include("User");
            ViewBag.Groups = groups;

            if(TempData.ContainsKey("MesajStergere"))
            {
                ViewBag.Mesaj = TempData["MesajStergere"].ToString();
            }
            else if (TempData.ContainsKey("MesajEditare"))
            {
                ViewBag.Mesaj = TempData["MesajEditare"].ToString();
            }
            else if (TempData.ContainsKey("MesajAdaugare"))
            {
                ViewBag.Mesaj = TempData["MesajAdaugare"].ToString();
            }

            return View();
        }

        [Authorize(Roles = "Utilizator,Moderator,Administrator")]
        public ActionResult Show(int id)
        {
            Group group = db.Groups.Find(id);
            ViewBag.Group = group;

            SetAccesRights(id);

            return View(group);
        }

        
        [HttpPost]
        [Authorize(Roles = "Utilizator,Moderator,Administrator")]
        public ActionResult Show(Post post)
        {
            post.PostData = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return Redirect("/Groups/Show/" + post.GroupId);
                }

                else
                {
                    Group group = db.Groups.Find(post.GroupId);

                    SetAccesRights();

                    return View(group);
                }

            }

            catch (Exception e)
            {
                Group group = db.Groups.Find(post.GroupId);

                SetAccesRights();

                return View(group);
            }

        }
        

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }

            // returnam lista de categorii
            return selectList;
        }

        [Authorize(Roles = "Utilizator,Moderator,Administrator")]
        public ActionResult New()
        {
            /*var categories = from cat in db.Categories
                             select cat;
            ViewBag.Categories = categories;*/
            Group group = new Group();
            group.Categories = GetAllCategories();
            group.GroupCreatorId = User.Identity.GetUserId();
            group.GroupCreatorName = User.Identity.GetUserName();
            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "Utilizator,Moderator,Administrator")]
        public ActionResult New(Group group)
        {
            try
            {
                group.GroupCreatorId = User.Identity.GetUserId();
                group.GroupCreatorName = User.Identity.GetUserName();
                db.Groups.Add(group);
                db.SaveChanges();
                TempData["MesajAdaugare"] = "Grupul " + group.GroupName + " a fost adaugat cu succes!";

                Membership membership = new Membership();
                membership.GroupId = group.GroupId;
                membership.UserId = User.Identity.GetUserId();
                membership.UserName = User.Identity.GetUserName();
                membership.Status = "Accepted";
                membership.Role = "Moderator";
                db.Memberships.Add(membership);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Utilizator, Moderator,Administrator")]
        public ActionResult Edit(int id)
        {
            Group group = db.Groups.Find(id);
            ViewBag.Group = group;
            if (group.GroupCreatorId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                return View(group);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui grup care nu va apartine!";
                return Redirect("/Groups/Show/" + id);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Utilizator, Moderator,Administrator")]
        public ActionResult Edit(int id, Group requestGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Group group = db.Groups.Find(id);

                    if (group.GroupCreatorId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(group))
                        {
                            group.GroupName = requestGroup.GroupName;
                            group.GroupDescription = requestGroup.GroupDescription;
                            db.SaveChanges();
                            TempData["MesajEditare"] = "Grupul " + group.GroupName + " a fost editat cu succes!";
                        }

                        return Redirect("/Groups/Show/" + id);

                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui grup care nu va apartine!";
                        return Redirect("/Groups/Show/" + id);

                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Utilizator, Moderator, Administrator")]
        public ActionResult Delete(int id)
        {
            Group group = db.Groups.Find(id);
            if (group.GroupCreatorId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                TempData["MesajStergere"] = "Grupul " + group.GroupName + " a fost sters cu succes!";
                db.Groups.Remove(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un grup care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        private void SetAccesRights(int id=0)
        {
            ViewBag.CurrentUser = User.Identity.GetUserId();

            if (User.IsInRole("Administrator")) ViewBag.IsAdmin = true;
            else ViewBag.IsAdmin = false;

            if (User.IsInRole("Moderator")) ViewBag.Autorizatie = true;
            else ViewBag.Autorizatie = false;

            var members = from m in db.Memberships
                          select m;

            ViewBag.Member = false;
            foreach(var member in members)
            {
                if (member.GroupId == id && User.Identity.GetUserId() == member.UserId)
                {
                    ViewBag.Member = true;
                    break;
                }
            }

        }

    }
}