﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Context;
using Domain.Entities;
using System.Threading.Tasks;

namespace WebUI.Helpers
{
    public class Mailing
    {
        public async Task Send()
        {
            using (EFArticleContext articleContext = new EFArticleContext())
            using (EFMailArticleContext mailArticleContext = new EFMailArticleContext())
            using (EFSubscriberContext subscriberContext = new EFSubscriberContext())
            {
                List<Article> forMailing = await articleContext.ArticlesForMailing();
            }
        }

    }
}