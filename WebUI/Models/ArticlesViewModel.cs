using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebUI.Models
{
    public class ArticlesView
    {
        public List<Article> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class Carousel
    {
        //public int Id { get; set; }
        public string Directory { get; set; }
        public int NumSlides { get; set; }
        public string Extension { get; set; }
    }

    [Serializable]
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class urlset
    {
        [XmlElement("url")]
        public List<url> urls;
    }

    public class url
    {
        public string lastmod { get; set; }
        public string changefreq { get; set; }
        public Double priority { get; set; }
        public string loc { get; set; }
    }
}