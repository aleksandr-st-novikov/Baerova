﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum Groups
    {
        Главная,
        Дистрибьюторам_TianDe,
        Бизнес_с_TianDe,
        Информация,
        Предпринимателям
    }

    public class MenuSet
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        public Groups Group { get; set; }

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        public Boolean IsActive { get; set; }

        [Display(Name = "№ п/п")]
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [Range(1,5, ErrorMessage = "от {1} до {2}")]
        public int Order { get; set; }

        [Display(Name = "Название")]
        [StringLength(250, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 1)]
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        public string Name { get; set; }

        [Display(Name = "Ссылка")]
        [StringLength(500, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 1)]
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        public string Link { get; set; }
    }
   
}
