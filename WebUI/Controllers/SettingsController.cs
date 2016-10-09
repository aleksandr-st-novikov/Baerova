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
using WebUI.Helpers;
using Hangfire;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class SettingsController : Controller
    {
        #region Инициализация
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        RoleManager<IdentityRole> _roleManager;
        private EFConstantContext _constantContext;

        public SettingsController()
        {
        }

        public SettingsController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            RoleManager<IdentityRole> roleManager,
            EFConstantContext constantContext
            )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            ConstantContext = constantContext;
        }

        public EFConstantContext ConstantContext
        {
            get
            {
                return _constantContext ?? new EFConstantContext();
            }
            private set
            {
                _constantContext = value;
            }
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
            IsActiveMenu("", "");
            return View();
        }

        private void IsActiveMenu(string Menu, string subMenu)
        {
            List<string> allSubMenu = new List<string>
            {
                "sMenu",
                "sConst",
                "sUser",
                "sRole",
                "sScheduleCreate"
            };
            allSubMenu.Remove(subMenu);
            foreach (string s in allSubMenu)
            {
                Session[s] = null;
            }
            Session[subMenu] = "active";

            List<string> allMenu = new List<string>
            {
                "sAll",
                "sUsers",
                "sSchedule"
            };
            allMenu.Remove(Menu);
            foreach (string s in allMenu)
            {
                Session[s] = null;
            }
            Session[Menu] = "in";
        }

        #region Пользователи
        public ViewResult UserManage()
        {
            IsActiveMenu("sUsers", "sUser");

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
        public async Task AddUser(UserView model, params string[] selectedRole)
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
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserSave")]
        public async Task UserSave([Bind(Prefix = "u")]UserEdit model, params string[] selectedRole)
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
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserUnLock")]
        public async Task UserUnLock([Bind(Prefix = "u")]UserEdit model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id.ToString());
                user.LockoutEndDateUtc = null;
                await UserManager.UpdateAsync(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserLock")]
        //CS$<>8__locals1.
        public async Task UserLock([Bind(Prefix = "u")]UserEdit model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id.ToString());
                user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(100);
                await UserManager.UpdateAsync(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "UserDelete")]
        public async Task UserDelete([Bind(Prefix = "u")]UserEdit model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id.ToString());
                await UserManager.DeleteAsync(user);
            }
        }

        public ActionResult UserChangePassword()
        {
            return PartialView("_UserChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task UserChangePassword(UserChangePasswordView model)
        {
            if (ModelState.IsValid && Request.IsAjaxRequest())
            {
                var user = await UserManager.FindByIdAsync(model.UserId.ToString());
                if (user != null)
                {
                    string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);
                }
            }
        }
        #endregion

        #region Роли
        public ViewResult RoleManage()
        {
            IsActiveMenu("sUsers", "sRole");
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
            IsActiveMenu("sAll", "sConst");
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

        #region Настройки главного меню
        public ViewResult MenuSetManage()
        {
            IsActiveMenu("sAll", "sMenu");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task MenuSetAdd(MenuSet menuSet)
        {
            using (EFMenuSetContext menuSetContext = new EFMenuSetContext())
            {
                if (ModelState.IsValid && Request.IsAjaxRequest())
                {
                    await menuSetContext.SaveMenuSetAsync(menuSet);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "MenuSetSave")]
        public async Task MenuSetSave([Bind(Prefix = "l")]MenuSet menuSet)
        {
            using (EFMenuSetContext menuSetContext = new EFMenuSetContext())
            {
                if (ModelState.IsValid && Request.IsAjaxRequest())
                {
                    await menuSetContext.SaveMenuSetAsync(menuSet);
                }
            }
        }

        public PartialViewResult MenuSetList()
        {
            using (EFMenuSetContext menuSetContext = new EFMenuSetContext())
            {
                return PartialView("_MenuSetList", menuSetContext.MenuSets.ToList());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "MenuSetDelete")]
        public async Task MenuSetDelete([Bind(Prefix = "l")]MenuSet menuSet)
        {
            using (EFMenuSetContext menuSetContext = new EFMenuSetContext())
            {
                if (Request.IsAjaxRequest())
                {
                    await menuSetContext.DeleteMenuSetAsync(menuSet.Id);
                }
            }
        }
        #endregion

        public ViewResult AddSchedule()
        {
            IsActiveMenu("sSchedule", "sScheduleCreate");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void AddSchedule(string Time)
        {
            if (!String.IsNullOrEmpty(Time) && !String.IsNullOrEmpty(ConstantContext.GetConstant("Рассылки: настройки почтового ящика")))
            {
                string[] time = Time.Split(':');
                string[] _params = ConstantContext.GetConstant("Рассылки: настройки почтового ящика").Split(';');
                foreach (string p in _params)
                {
                    if (String.IsNullOrEmpty(p))
                    {
                        return;
                    }
                }

                RecurringJob.AddOrUpdate("Рассылка новостей", () => Mailing.Send(_params), time[1] + " " + time[0] + " * * *", TimeZoneInfo.FindSystemTimeZoneById("Belarus Standard Time"));

                //RecurringJob.AddOrUpdate("Рассылка новостей",
                //    () => Services.SendMessage(_params, "test", "text", "novikov.it@bobruysk.korona.by"), time[1] + " " + time[0] + " * * *",
                //    TimeZoneInfo.FindSystemTimeZoneById("Belarus Standard Time"));
                //BackgroundJob.Schedule(() => Services.SendMessage(_params, Guid.NewGuid().ToString(), "text", "novikov.it@bobruysk.korona.by"), TimeSpan.FromMinutes(10));
            }
        }

        //[HttpPost]
        //public void send()
        //{
        //    //string[] _params = "noreply@e-tiande.by;Novikov;Djkmdjc60;smtp.yandex.ru;25;1".Split(';');
        //    string[] _params = "a_nov@tut.by;Novikov;novik12345;smtp.yandex.ru;25;1".Split(';');
        //    Mailing.Send(_params);
        //    //Services.SendMessage("test", "text", "novikov.it@bobruysk.korona.by");
        //}

    }
}
