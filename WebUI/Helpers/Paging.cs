using System;
using System.Text;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Helpers
{
    public static class Paging
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl)
        {
            if (pagingInfo.TotalPages == 0) return (null);

            StringBuilder result = new StringBuilder();

            TagBuilder tag = new TagBuilder("a");
            tag.InnerHtml = "&lt;&lt;";
            if (1 == pagingInfo.CurrentPage)
            {
                tag.AddCssClass("disabled");
            }
            else
            {
                tag.MergeAttribute("href", pageUrl(1));
            }
            tag.AddCssClass("btn btn-sm btn-dafault");
            result.Append(tag.ToString());

            tag = new TagBuilder("a");
            tag.InnerHtml = "&lt;";
            if (1 == pagingInfo.CurrentPage)
            {
                tag.AddCssClass("disabled");
            }
            else
            {
                tag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
            }
            tag.AddCssClass("btn btn-sm btn-dafault");
            result.Append(tag.ToString());

            if (pagingInfo.TotalPages <= 5)
            {
                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    if (i == pagingInfo.CurrentPage)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-sm btn-primary");
                    }
                    tag.AddCssClass("btn btn-sm btn-dafault");
                    result.Append(tag.ToString());
                }
            }
            else
            {
                if (pagingInfo.CurrentPage != 1)
                {
                    tag = new TagBuilder("a");
                    tag.MergeAttribute("href", "#");
                    tag.InnerHtml = "...";
                    tag.AddCssClass("disabled");
                    tag.AddCssClass("btn btn-sm btn-dafault");
                    result.Append(tag.ToString());
                }

                if (pagingInfo.TotalPages - pagingInfo.CurrentPage >= 5)
                {
                    for (int i = pagingInfo.CurrentPage; i <= pagingInfo.CurrentPage + 4; i++)
                    {
                        tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrl(i));
                        tag.InnerHtml = i.ToString();
                        if (i == pagingInfo.CurrentPage)
                        {
                            tag.AddCssClass("selected");
                            tag.AddCssClass("btn-sm btn-primary");
                        }
                        tag.AddCssClass("btn btn-sm btn-dafault");
                        result.Append(tag.ToString());
                    }
                    tag = new TagBuilder("a");
                    tag.MergeAttribute("href", "#");
                    tag.InnerHtml = "...";
                    tag.AddCssClass("disabled");
                    tag.AddCssClass("btn btn-sm btn-dafault");
                    result.Append(tag.ToString());
                }
                else
                {
                    for (int i = pagingInfo.TotalPages - 4; i <= pagingInfo.TotalPages; i++)
                    {
                        tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrl(i));
                        tag.InnerHtml = i.ToString();
                        if (i == pagingInfo.CurrentPage)
                        {
                            tag.AddCssClass("selected");
                            tag.AddCssClass("btn-sm btn-primary");
                        }
                        tag.AddCssClass("btn btn-sm btn-dafault");
                        result.Append(tag.ToString());
                    }
                }
            }

            tag = new TagBuilder("a");
            tag.InnerHtml = "&gt;";
            if (pagingInfo.TotalPages == pagingInfo.CurrentPage)
            {
                tag.AddCssClass("disabled");
            }
            else
            {
                tag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
            }
            tag.AddCssClass("btn btn-sm btn-dafault");
            result.Append(tag.ToString());

            tag = new TagBuilder("a");
            tag.InnerHtml = "&gt;&gt;";
            if (pagingInfo.TotalPages == pagingInfo.CurrentPage)
            {
                tag.AddCssClass("disabled");
            }
            else
            {
                tag.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
            }
            tag.AddCssClass("btn btn-sm btn-dafault");
            result.Append(tag.ToString());

            return MvcHtmlString.Create(result.ToString());
        }
    }
}