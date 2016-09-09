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
        public DateTime DateCreate { get; set; }
    }
}
