using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class RoleView
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Требуется поле {0}.")]
        [Display(Name = "Наименование")]
        public string Name { get; set; }
    }

    public class UserView
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Требуется поле {0}.")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется поле {0}.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Требуется поле {0}.")]
        [StringLength(100, ErrorMessage = "Длина {0} должна быть не меньше {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Подтверждение пароля не совпадает.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Разрешена блокировка")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Дата разблокировки")]
        public DateTime? LockoutEndDateUtc { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> RolesList { get; set; }
    }

    public class UserEdit
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Требуется поле {0}.")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Требуется поле {0}.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Разрешена блокировка")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Дата разблокировки")]
        public DateTime? LockoutEndDateUtc { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> RolesList { get; set; }
    }
}