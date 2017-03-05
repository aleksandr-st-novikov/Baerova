using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BannerLink
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Номер слайда")]
        public int NumSlide { get; set; }

        [Display(Name = "Ссылка на публикацию")]
        [StringLength(500, ErrorMessage = "Длина должна быть от {2} до {1} символов!", MinimumLength = 2)]
        public string Link { get; set; }
    }
}
