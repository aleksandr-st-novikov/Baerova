using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? DateCreate { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Descr { get; set; }

        [Display(Name = "Картинка (поделиться)")]
        public string PictShare { get; set; }

        [Display(Name = "Показывать статью")]
        public Boolean IsVisible { get; set; }

        [Display(Name = "Дата публикации")]
        public DateTime? DatePublish { get; set; }

        [Display(Name = "Заголовок")]
        public String TextMain { get; set; }

        [Display(Name = "Текст")]
        public String TextArticle { get; set; }
    }
}
