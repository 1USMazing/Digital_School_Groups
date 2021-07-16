using Microsoft.AspNet.Identity;
using Proiect_DSG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using System.Web.Mvc;

namespace Proiect_DSG.Controllers
{
    public class MembershipsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Membership
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Moderator, Administrator, Utilizator")]
        public ActionResult New(int id)
        {
            bool existenta = false;
            var members = from m in db.Memberships
                             select m;
            foreach(var member in members)
            {
                if (member.GroupId == id && User.Identity.GetUserId() == member.UserId) existenta = true;
            }

            if (existenta == false)
            {
                Membership membership = new Membership();

                membership.GroupId = id;
                membership.UserId = User.Identity.GetUserId();
                membership.UserName = User.Identity.GetUserName();
                membership.Status = "Accepted";
                membership.Role = "Membru";
                db.Memberships.Add(membership);
                db.SaveChanges();
                ViewBag.GroupId = id;

                return View();
            }

            else return Redirect("/Groups/Show/" + id.ToString());
        }

        [Authorize(Roles = "Moderator, Administrator, Utilizator")]
        public ActionResult Delete(int id)
        {
            bool existenta = false;
            var members = from m in db.Memberships
                          select m;
            var membership = new Membership();
            foreach (var member in members)
            {
                if (member.GroupId == id && User.Identity.GetUserId() == member.UserId) { existenta = true; membership = member; }
            }

            if (membership != null)
            {
                db.Memberships.Remove(membership);
                db.SaveChanges();
                return Redirect("/Groups/Show/" + id.ToString());
            }

            return Redirect("/Groups/Show/" + id.ToString());
        }

        [Authorize(Roles = "Moderator, Administrator, Utilizator")]
        public ActionResult List(int id, string name)
        {
            var memberships = from m in db.Memberships
                              select m;

            var memberslist = new List<SelectListItem>();

            foreach (var membership in memberships)
            {
                if (membership.GroupId == id)
                {
                    memberslist.Add(new SelectListItem
                    {
                       Value=membership.UserId.ToString(),
                       Text=membership.UserName.ToString()
                    });
                }
            }

            SetAccesRights(id);

            ViewBag.Lista = memberslist;
            ViewBag.GroupId = id;
            ViewBag.GroupName = name;
            ViewBag.CurrentUser = User.Identity.GetUserId();

            return View();
        }


        [Authorize(Roles = "Moderator, Administrator, Utilizator")]
        public ActionResult Kick(int id, string name)
        {
            bool existenta = false;
            var members = from m in db.Memberships
                          select m;
            var membership = new Membership();
            foreach (var member in members)
            {
                if (member.GroupId == id && name == member.UserId) { existenta = true; membership = member; }
            }
            
            if (membership != null)
            {
                db.Memberships.Remove(membership);
                db.SaveChanges();
                return Redirect("/Memberships/List/" + id.ToString());
            }

            return Redirect("/Groups/Show/" + id.ToString());
        }


        /*
        [Authorize(Roles = "Moderator, Administrator, Utilizator")]
        [HttpPost]
        public ActionResult New(Membership membership)
        {
            membership.User.Id = User.Identity.GetUserId();
            membership.GroupId = ViewBag.GroupId;

            membership.Status = "Accepted";

            if (User.IsInRole("Utilizator")) membership.Role = "Membru";
            if (User.IsInRole("Moderator")) membership.Role = "Moderator";
            if (User.IsInRole("Administrator")) membership.Role = "Administrator";

            TempData["message"] = "Cererea a fost creata cu succes.";
            //try{
            db.Memberships.Add(membership);
            db.SaveChanges();
            return Redirect("/Groups/Index/");

            /*}
            catch (Exception e)
            {
               return View();
            }
        }*/

        private void SetAccesRights(int id = 0)
        {
            ViewBag.CurrentUser = User.Identity.GetUserId();

            if (User.IsInRole("Administrator")) ViewBag.IsAdmin = true;
            else ViewBag.IsAdmin = false;

            var members = from m in db.Memberships
                          select m;

            ViewBag.Autorizatie = false;
            foreach (var member in members)
            {
                if (member.GroupId == id && ViewBag.CurrentUser == member.UserId && member.Role=="Moderator")
                {
                    ViewBag.Autorizatie = true;
                    break;
                }
            }

        }

    }
}