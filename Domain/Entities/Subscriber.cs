using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Subscriber
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес E-Mail.")]
        [StringLength(100)]
        [Required(ErrorMessage = "Вы не указали адрес E-Mail.")]
        public string EMail { get; set; }

        public DateTime DateCreate { get; set; }

        public Boolean IsActive { get; set; }
    }
}
