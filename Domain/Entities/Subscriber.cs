using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Subscriber
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Поле {0} не содержит допустимый адрес e-mail.")]
        [StringLength(100)]
        public string EMail { get; set; }
    }
}
