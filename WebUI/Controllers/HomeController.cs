using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Context;
using Domain.Entities;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private int PageSize = 20;

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult SideMenu()
        {
            using (EFMenuSetContext menuSetContext = new EFMenuSetContext())
            {
                return PartialView("_SideMenu", menuSetContext.MenuSets.ToList());
            }
        }

        [AllowAnonymous]
        public ViewResult Register()
        {
            Session["regActive"] = "active";
            Session["Link"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task Register(Partner partner)
        {
            if(ModelState.IsValid && Request.IsAjaxRequest())
            {
                using (EFPartnerContext partnerContext = new EFPartnerContext())
                {
                    await partnerContext.SavePartnerAsync(partner);
                }
            }
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult ManagePartners(int page = 1)
        {
            using (EFPartnerContext partnerContext = new EFPartnerContext())
            {
                PartnersView model = new PartnersView
                {
                    Partners = partnerContext.Partners.OrderByDescending(p => p.DateCreate).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemPerPage = PageSize,
                        TotalItems = partnerContext.Partners.Count()
                    }
                };
                return View(model);
            }
            
        }
    }
}