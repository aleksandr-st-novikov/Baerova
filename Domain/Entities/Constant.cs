using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Constant
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        [StringLength(250, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        [Required(ErrorMessage = "Вы не заполнили поле {0}.")]
        public string Name { get; set; }
    }
}
