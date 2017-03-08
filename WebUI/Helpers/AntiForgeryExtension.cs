using System.Web.Mvc;

namespace WebUI.Helpers
{
    public static class AntiForgeryExtension
    {
        public static string RequestVerificationToken(this HtmlHelper helper)
        {
            return GetTokenHeaderValue();
        }

        private static string GetTokenHeaderValue()
        {
            string cookieToken, formToken;
            System.Web.Helpers.AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }
    }
}