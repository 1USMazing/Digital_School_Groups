using Microsoft.AspNet.Identity;
using Proiect_DSG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using System.Web.Mvc;

namespace Proiect_DSG.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllGroups()
        {
            // generam o lista goala             
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date             
            var groups = from gr in db.Groups select gr;
            // iteram prin categorii             
            foreach (var group in groups)
            {
                // Adaugam in lista elementele necesare pentru dropdown                 
                selectList.Add(new SelectListItem
                {
                    Value = group.GroupId.ToString(),
                    Text = group.GroupName.ToString()
                });
            }
            // returnam lista de categorii             
            return selectList;
        }

        [Authorize(Roles = "Moderator,Administrator, Utilizator")]
        public ActionResult New(int id)
        {
            //TempData["message"] = "Postarea a fost creata cu succes.";
            Post post = new Post();
            post.Groups = GetAllGroups();
            post.GroupId = id;
            post.UserId = User.Identity.GetUserId();
            post.UserName = User.Identity.GetUserName();

            return View(post);
        }

        [Authorize(Roles = "Moderator,Administrator, Utilizator")]
        [HttpPost]
        public ActionResult New(int id, Post post)
        {
            post.PostData = DateTime.Now;
            post.GroupId = id;
            post.UserId = User.Identity.GetUserId();
            post.UserName = User.Identity.GetUserName();
            TempData["message"] = "Postarea a fost creata cu succes.";
            /*try
            {*/
            db.Posts.Add(post);
            db.SaveChanges();
            return Redirect("/Groups/Show/" + post.GroupId);

            //}
            /*catch (Exception e)
            {*/
            //   return View();
            // }
        }



        [Authorize(Roles = "Moderator,Administrator, Utilizator")]
        [HttpDelete]
        public ActionResult Delete(int id, string name)
        {
            Post post = db.Posts.Find(id);

            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator") || User.Identity.GetUserId()==name)
            {
                TempData["message"] = "Postarea a fost stearsa din baza de date.";
                db.Posts.Remove(post);
                db.SaveChanges();

                return Redirect("/Groups/Show/" + post.GroupId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o postare care nu va apartine!";
                return RedirectToAction("Index", "Groups");
            }
        }

        [Authorize(Roles = "Moderator,Administrator, Utilizator")]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            //TempData["message"] = "Postarea a fost editata cu succes.";
            ViewBag.Post = post;
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei postari care nu va apartine!";
                return RedirectToAction("Index", "Groups");
            }
        }

        [Authorize(Roles = "Moderator,Administrator, Utilizator")]
        [HttpPut]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Post post = db.Posts.Find(id);

                    if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(post))
                        {
                            TempData["message"] = "Postarea a fost editata cu succes.";
                            post.Text = requestPost.Text;
                            db.SaveChanges();
                        }

                        return Redirect("/Groups/Show/" + post.GroupId);
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei postari care nu va apartine!";
                        return RedirectToAction("Index", "Groups");
                    }

                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return View(requestPost);
            }
        }
    }
}