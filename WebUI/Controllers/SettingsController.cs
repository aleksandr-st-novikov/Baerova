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
using Domain.Entities;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class SettingsController : Controller
    {
        #region Инициализация
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
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new Models.ApplicationDbContext()));
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        #region Пользователи
        public ViewResult UserManage()
        {
            ViewBag.allRoles = RoleManager.Roles.OrderBy(r => r.Name).ToList().Select(x => new SelectListItem()
            {
                Selected = false,
                Text = x.Name,
                Value = x.Name
            });
            return View();
        }

        public PartialViewResult UserList()
        {
            return PartialView("_UserList", GetUserList());
        }

        private List<UserEdit> GetUserList()
        {
            List<UserEdit> model = UserManager
                            .Users.AsEnumerable()
                            .OrderBy(r => r.UserName).
                            Select(r => new UserEdit
                            {
                                Id = new Guid(r.Id),
                                UserName = r.UserName,
                                Email = r.Email,
                                LockoutEnabled = r.LockoutEnabled,
                                LockoutEndDateUtc = r.LockoutEndDateUtc
                            }).ToList();
            foreach (UserEdit uv in model)
            {
                var userRoles = UserManager.GetRoles(uv.Id.ToString());
                uv.RolesList = RoleManager.Roles.OrderBy(r => r.Name).ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });
            }

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(UserView model, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, LockoutEnabled = model.LockoutEnabled };
                await UserManager.CreateAsync(user, model.Password);

                var userRoles = await UserManager.GetRolesAsync(user.Id);
                selectedRole = selectedRole ?? new string[] { };
                await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());
                await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());
            }
            return PartialView("_UserList", GetUserList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserSave")]
        public async Task<ActionResult> UserSave([Bind(Prefix = "CS$<>8__locals1.u")]UserEdit model, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id.ToString());
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.LockoutEnabled = model.LockoutEnabled;
                await UserManager.UpdateAsync(user);

                var userRoles = await UserManager.GetRolesAsync(user.Id);
                selectedRole = selectedRole ?? new string[] { };
                await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());
                await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());
            }
            return PartialView("_UserList", GetUserList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserUnLock")]
        public async Task<ActionResult> UserUnLock([Bind(Prefix = "CS$<>8__locals1.u")]UserEdit model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id.ToString());
                user.LockoutEndDateUtc = null;
                await UserManager.UpdateAsync(user);
            }
            return PartialView("_UserList", GetUserList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserLock")]
        public async Task<ActionResult> UserLock([Bind(Prefix = "CS$<>8__locals1.u")]UserEdit model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id.ToString());
                user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(100);
                await UserManager.UpdateAsync(user);
            }
            return PartialView("_UserList", GetUserList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserDelete")]
        public async Task<ActionResult> UserDelete([Bind(Prefix = "CS$<>8__locals1.u")]UserEdit model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id.ToString());
                await UserManager.DeleteAsync(user);
            }
            return PartialView("_UserList", GetUserList());
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
                Models.ApplicationDbContext context = new Models.ApplicationDbContext();
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

        #region Константы
        public ViewResult Constants()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task ConstantAdd(Constant constant)
        {
            using (EFConstantContext constantContext = new EFConstantContext())
            {
                if (ModelState.IsValid && Request.IsAjaxRequest())
                {
                    await constantContext.SaveConstantAsync(constant);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "ConstantSave")]
        public async Task ConstantSave([Bind(Prefix = "m")]Constant constant)
        {
            using (EFConstantContext constantContext = new EFConstantContext())
            {
                if (ModelState.IsValid && Request.IsAjaxRequest())
                {
                    await constantContext.SaveConstantAsync(constant);
                }
            }
        }

        public PartialViewResult ConstantList()
        {
            using (EFConstantContext constantContext = new EFConstantContext())
            {
                return PartialView("_ConstantList", constantContext.Constants.OrderBy(c => c.Name).ToList());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "ConstantDelete")]
        public async Task ConstantDelete([Bind(Prefix = "m")]Constant constant)
        {
            using (EFConstantContext constantContext = new EFConstantContext())
            {
                if (Request.IsAjaxRequest())
                {
                    await constantContext.DeleteConstantAsync(constant.Id);
                }
            }
        }
        #endregion
    }
}
