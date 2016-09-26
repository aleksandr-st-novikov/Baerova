using Domain.Entities;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class PartnersView
    {
        public List<Partner> Partners { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class SubscribersView
    {
        public List<Subscriber> Subscribers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class MainArticleView
    {
        public Article article { get; set; }
        public string CountView { get; set; }
    }
}