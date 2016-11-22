using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Context;
using Domain.Entities;
using System.Threading.Tasks;
using System.Text;
using System.Web.Hosting;

namespace WebUI.Helpers
{
    public static class Mailing
    {
        public static async Task SendNews(string[] _params)
        {
            using (EFArticleContext articleContext = new EFArticleContext())
            using (EFMailArticleContext mailArticleContext = new EFMailArticleContext())
            using (EFSubscriberContext subscriberContext = new EFSubscriberContext())
            using (EFConstantContext constantContext = new EFConstantContext())
            {
                List<Article> forMailing = await articleContext.ArticlesForMailingAsync();
                if (forMailing.Count() == 0) return;

                string siteUrl = constantContext.GetConstant("Общие: URL сайта");

                StringBuilder news = new StringBuilder();
                string message = String.Empty;
                foreach (Article a in forMailing)
                {
                    news.Append("<a class=\"title-main-article\" href=" + siteUrl + "/articles/article/" + a.Link + ">" + a.Title + "</a>");
                    news.Append(System.Net.WebUtility.HtmlDecode(a.TextMain));
                    news.Append("<span style=\"display:block;text-align:right;\"><a class=\"btn-main-article\" href=" + siteUrl + "/articles/article/" + a.Link + ">Читать полностью</a></span>");
                };
                news.Append("<hr/>");

                //string path = "e:\\vs\\baerova\\WebUI\\Content\\Delivery\\LetterNews.html";

                //string file = "~/Content/Delivery/LetterNews.html";
                //string path = HostingEnvironment.MapPath(file);
                string path = "e:\\VS\\Baerova\\WebUI\\Content\\Delivery\\LetterNews.html";

                if (System.IO.File.Exists(path))
                {
                    string[] lines = System.IO.File.ReadAllLines(path);
                    message = String.Join(" ", lines).Replace("{0}", news.ToString());
                }

                //отбираем получателей и отправляем письмо пачками по 20 получателей и что останется
                if (subscriberContext.Subscribers.Count() == 0) return;

                string messageTo = String.Empty;
                string messageCC = String.Empty;
                int count = 1;
                int countSubscribers = subscriberContext.Subscribers.Count();
                foreach (var s in subscriberContext.Subscribers.Where(s => s.IsActive == true))
                {
                    if (count == 1 || count % 21 == 0)
                    {
                        messageTo = s.EMail;
                    }
                    else
                    {
                        messageCC += s.EMail + (count == countSubscribers && count % 20 == 0 ? "" : ",");
                    }
                    if (count % 20 == 0)
                    {
                        Services.SendMessage(_params, "Baeroff.com – Рассылка новостей от " + DateTime.Now.ToShortDateString(), message.ToString(), messageTo, messageCC);
                        messageTo = messageCC = "";
                    }
                    count++;
                }
                if (count % 20 != 0)
                {
                    Services.SendMessage(_params, "Baeroff.com – Рассылка новостей от " + DateTime.Now.ToShortDateString(), message.ToString(), messageTo, messageCC);
                    messageTo = messageCC = "";
                }

                //сохраняем в отправленных
                foreach (Article a in forMailing)
                {
                    mailArticleContext.SaveMailArticle(new MailArticle { Id = Guid.NewGuid(), ArticleId = a.Id, DateMailing = DateTime.Now, CountRecipient = count - 1 });
                }
            }
        }

    }
}