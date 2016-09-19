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

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [Display(Name = "Название")]
        [StringLength(300, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [StringLength(500)]
        public string Descr { get; set; }

        [Display(Name = "Картинка (поделиться)")]
        [StringLength(250)]
        public string PictShare { get; set; }

        [UIHint("Boolean")]
        [Display(Name = "Показывать статью")]
        public Boolean IsVisible { get; set; }

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [Display(Name = "Дата публикации")]
        public DateTime? DatePublish { get; set; }

        //[Required(ErrorMessage = "Вы не заполнили {0}.")]
        [Display(Name = "Заголовок публикации")]
        public String TextMain { get; set; }

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [Display(Name = "Текст публикации")]
        public String TextArticle { get; set; }

        [StringLength(500, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        public string Link { get; set; }
    }
}
