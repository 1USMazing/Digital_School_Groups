using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proiect_DSG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;
using System.Web.Mvc;

namespace Proiect_DSG.Controllers
{

    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Users
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;
            ViewBag.Users = users;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            string currentRole = user.Roles.FirstOrDefault().RoleId;

            var roleName = (from role in db.Roles
                            where role.Id == currentRole
                            select role.Name).First();

            ViewBag.RoleName = roleName;

            ViewBag.CurrentUser = User.Identity.GetUserId();

            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.UserRole = userRole.RoleId;
            return View(user);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }

            return selectList;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.UserRole = userRole.RoleId;

            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;

                    var roles = from role in db.Roles
                                select role;

                    foreach (var role in roles)
                    {
                        userManager.RemoveFromRole(id, role.Name);
                    }

                    var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                    userManager.AddToRole(id, selectedRole.Name);

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                newData.Id = id;
                return View(newData);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = userManager.Users.FirstOrDefault(u => u.Id == id);

            var groups = db.Groups.Where(a => a.GroupCreatorId == id);

            foreach (var group in groups)
            {
                db.Groups.Remove(group);
            }

            var posts = db.Posts.Where(post => post.UserId == id);
            foreach (var post in posts)
            {
                db.Posts.Remove(post);
            }

            db.SaveChanges();
            userManager.Delete(user);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Utilizator, Moderator")]
        public ActionResult GroupsList()
        {
            var memberships = db.Memberships.ToArray();

            var groupslist = new List<Group>();
            
            foreach (var membership in memberships)
            {
                if (membership.UserId == User.Identity.GetUserId())
                {
                    groupslist.Add(db.Groups.Find(membership.GroupId));
                }
            }

            ViewBag.Groups = groupslist;
            return View();
        }
    }
}