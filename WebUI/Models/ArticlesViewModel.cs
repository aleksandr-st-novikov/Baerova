using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}