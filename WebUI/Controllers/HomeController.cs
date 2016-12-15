using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Context;
using Domain.Entities;
using System.Threading.Tasks;
using WebUI.Models;
using static WebUI.Helpers.MultiButton;
using System.Text;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private int PageSize = 20;
        private EFConstantContext _constantContext;

        public HomeController()
        {
        }

        public HomeController(EFConstantContext constantContext)
        {
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

        [AllowAnonymous]
        public ActionResult Index()
        {
            Session["regActive"] = null;
            Session["Link"] = null;
            Session["Group"] = null;
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult SideMenu()
        {
            Session["AlwaysIn"] = ConstantContext.GetConstant("Главная: меню развернуто") == "1" ? "in" : String.Empty;
            ViewBag.CatalogLink = ConstantContext.GetConstantAsync("Общие: ссылка на каталог");
            using (EFMenuSetContext menuSetContext = new EFMenuSetContext())
            {
                return PartialView("_SideMenu", menuSetContext.MenuSets.ToList());
            }
        }

        [AllowAnonymous]
        public ActionResult Catalog()
        {
            ViewBag.CatalogLink = ConstantContext.GetConstant("Общие: ссылка на каталог");
            return PartialView("_Catalog");
        }

        [AllowAnonymous]
        public async Task<ViewResult> Register()
        {
            Session["regActive"] = "active";
            Session["Link"] = null;
            ViewBag.RegisterLink = await ConstantContext.GetConstantAsync("Общие: ссылка на регистрацию");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task Register(Partner partner)
        {
            if (ModelState.IsValid && Request.IsAjaxRequest())
            {
                using (EFPartnerContext partnerContext = new EFPartnerContext())
                {
                    await partnerContext.SavePartnerAsync(partner);
                    //отправляем письмо
                    StringBuilder mes = new StringBuilder();
                    mes.Append("<p><strong>Заявка на регистриацию:</strong></p>");
                    mes.Append("ФИО: " + partner.Surname + " " + partner.Name + " " + partner.Patronymic + "<br/>");
                    mes.Append("Телефон: " + partner.Phone + "<br/>");
                    mes.Append("E-mail: " + partner.EMail + "<br/><br/>");
                    mes.Append("Цель регистрации:<br/>");
                    if (partner.IsDiscount)
                    {
                        mes.Append("- Получать скидку.<br/>");
                    }
                    if (partner.IsDistributor)
                    {
                        mes.Append("- Стать дистрибьютором.<br/>");
                    }
                    if (partner.IsShopkeeper)
                    {
                        mes.Append("- Открыть свой магазин.<br/>");
                    }
                    string[] _params = ConstantContext.GetConstant("Общие: служебный почтовый ящик").Split(';');
                    Helpers.Services.SendMessage(_params, partner.Surname + " " + partner.Name + " " + partner.Patronymic + ". Заявка на регистрацию.", mes.ToString(), ConstantContext.GetConstant("Общие: получатель"));
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

        #region Подписчики
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task AddSubscriber(Subscriber subscriber)
        {
            if (ModelState.IsValid && Request.IsAjaxRequest())
            {
                using (EFSubscriberContext subscriberContext = new EFSubscriberContext())
                {
                    await subscriberContext.SaveSubscriberAsync(subscriber);
                }
            }
        }

        public ActionResult Subscriber()
        {
            return PartialView("_Subscriber");
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult ManageSubscribers(int page = 1)
        {
            return View(GetListSubscribersModel(page));
        }

        [Authorize(Roles = "Администратор")]
        public PartialViewResult SubscribersList(int page = 1)
        {
            return PartialView("_SubscribersList", GetListSubscribersModel(page).Subscribers);
        }

        private SubscribersView GetListSubscribersModel(int page)
        {
            using (EFSubscriberContext subscriberContext = new EFSubscriberContext())
            {
                int tp = (Int32)Math.Ceiling((decimal)subscriberContext.Subscribers.Count() / PageSize);
                page = page < 0 ? 1 : page > tp ? tp : page;

                SubscribersView model = new SubscribersView
                {
                    Subscribers = subscriberContext.Subscribers.Count() == 0 ? subscriberContext.Subscribers.ToList() :
                        subscriberContext.Subscribers.OrderByDescending(a => a.DateCreate).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemPerPage = PageSize,
                        TotalItems = subscriberContext.Subscribers.Count()
                    }
                };
                return model;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор")]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "SubscriberSave")]
        public async Task SubscriberSave([Bind(Prefix = "m")]Subscriber subscriber)
        {
            if (ModelState.IsValid && Request.IsAjaxRequest())
            {
                using (EFSubscriberContext subscriberContext = new EFSubscriberContext())
                {
                    await subscriberContext.SaveSubscriberAsync(subscriber);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор")]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "SubscriberDelete")]
        public async Task SubscriberDelete([Bind(Prefix = "m")]Subscriber subscriber)
        {
            using (EFSubscriberContext subscriberContext = new EFSubscriberContext())
            {
                if (Request.IsAjaxRequest())
                {
                    await subscriberContext.DeleteSubscriberAsync(subscriber.Id);
                }
            }
        }

        public async Task<ActionResult> UnSubscribe(Guid? id)
        {
            if (id != null)
            {
                using (EFSubscriberContext subscriberContext = new EFSubscriberContext())
                {
                    await subscriberContext.UnSubscribeAsync((Guid)id);
                }
            }
            return View();
        }

        #endregion

        public ActionResult MainArticles(int page = 1)
        {
            using (EFArticleContext articleContext = new EFArticleContext())
            using (EFCountViewContext countViewContext = new EFCountViewContext())
            {
                List<Article> articles = articleContext.GetArticles(page, "main");
                List<MainArticleView> model = new List<MainArticleView>();
                foreach(Article a in articles)
                {
                    model.Add(new MainArticleView { article = a, CountView = countViewContext.GetCountView(a.Id, Domain.Entities.ViewType.Article) });
                }
                Session["ArticlesCount"] = articleContext.GetArticlesCount();
                Session["ArticleOnPage"] = ConstantContext.GetConstant("Главная: количество публикаций") ?? "3";
                Session["ArticleMainPage"] = page;
                return PartialView("_MainArticles", model);
            }
        }

        public ActionResult AboutUs(int page = 1)
        {
            using (EFArticleContext articleContext = new EFArticleContext())
            using (EFCountViewContext countViewContext = new EFCountViewContext())
            {
                List<Article> articles = articleContext.GetArticles(page, "aboutUs");
                List<MainArticleView> model = new List<MainArticleView>();
                foreach (Article a in articles)
                {
                    model.Add(new MainArticleView { article = a, CountView = countViewContext.GetCountView(a.Id, Domain.Entities.ViewType.Article) });
                }
                Session["ArticlesCount"] = articleContext.GetArticlesCount();
                Session["ArticleOnPage"] = ConstantContext.GetConstant("Главная: количество публикаций") ?? "3";
                Session["ArticleMainPage"] = page;
                return PartialView("_MainArticles", model);
            }
        }

        public ActionResult FixedArticle(string category)
        {
            using (EFArticleContext articleContext = new EFArticleContext())
            using (EFCountViewContext countViewContext = new EFCountViewContext())
            {
                category = category == "index" ? "main" : category;
                Article a = articleContext.GetFixedArticle(category);
                if (a != null)
                {
                    MainArticleView model = new MainArticleView()
                    {
                        article = a,
                        CountView = countViewContext.GetCountView(a.Id, Domain.Entities.ViewType.Article)
                    };
                    return PartialView("_FixedArticle", model);
                }
                else
                {
                    return PartialView("_FixedArticle", null);
                }
            }
        }

    }
}