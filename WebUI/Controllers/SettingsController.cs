using Domain.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using static WebUI.Helpers.MultiButton;
using System.Data.Entity;

namespace WebUI.Controllers
{
    public class SettingsController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        RoleManager<IdentityRole> _roleManager;

        public SettingsController()
        {
        }

        public SettingsController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        #region Пользователи
        public ViewResult UserManage()
        {
            return View();
        }

        public PartialViewResult UserList()
        {
            return PartialView("_UserList", UserManager.Users.AsEnumerable().OrderBy(r => r.UserName)
                .Select(r => new UserView { Id = new Guid(r.Id), UserName = r.UserName }).ToList());
        }

        [AllowAnonymous]
        public ActionResult AddUser()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Settings");
                }
                //AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Роли
        public ViewResult RoleManage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(RoleView roleView)
        {
            if (Request.IsAjaxRequest())
            {
                await RoleManager.CreateAsync(new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = roleView.Name
                });
            }
            return PartialView("_RoleList", RoleManager.Roles.AsEnumerable().OrderBy(r => r.Name).Select(r => new RoleView { Id = new Guid(r.Id), Name = r.Name }).ToList());
        }

        public PartialViewResult RoleList()
        {
            return PartialView("_RoleList", RoleManager.Roles.AsEnumerable().OrderBy(r => r.Name).Select(r => new RoleView { Id = new Guid(r.Id), Name = r.Name }).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "RoleSave")]
        public async Task<ActionResult> RoleSave()
        {
            if (Request.IsAjaxRequest())
            {
                await RoleManager.UpdateAsync(new IdentityRole()
                {
                    Id = Request["m.Id"].ToString(),
                    Name = Request["m.Name"].ToString()
                });
            }
            return PartialView("_RoleList", RoleManager.Roles.AsEnumerable().OrderBy(r => r.Name).Select(r => new RoleView { Id = new Guid(r.Id), Name = r.Name }).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "RoleDelete")]
        public async Task<ActionResult> RoleDelete()
        {
            if (Request.IsAjaxRequest())
            {
                ApplicationDbContext context = new ApplicationDbContext();
                String Id = Request["m.Id"].ToString();
                IdentityRole role = await context.Roles.FirstOrDefaultAsync(r => r.Id == Id);
                context.Roles.Remove(role);
                await context.SaveChangesAsync();

                //IdentityRole role = await RoleManager.FindByIdAsync(Request["m.Id"].ToString());
                //await RoleManager.DeleteAsync(role);
            }

            return PartialView("_RoleList", RoleManager.Roles.AsEnumerable().OrderBy(r => r.Name).Select(r => new RoleView { Id = new Guid(r.Id), Name = r.Name }).ToList());
        }
        #endregion
    }
}