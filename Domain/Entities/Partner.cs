using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Partner
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [Display(Name = "Фамилия")]
        [StringLength(150, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [Display(Name = "Имя")]
        [StringLength(100, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(100, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        [Display(Name = "Дата рождения")]
        //[DataType(DataType.DateTime, ErrorMessage = "Введенное значение должно быть датой.")]
        public DateTime? DOB { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Поле {0} не содержит допустимый адрес e-mail.")]
        [StringLength(100)]
        public string EMail { get; set; }

        [Display(Name = "Телефон")]
        [StringLength(30, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        public string Phone { get; set; }

        public DateTime DateCreate { get; set; }
    }
}
