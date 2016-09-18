using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum ViewType
    {
        Article
    }

    public class CountView
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ViewId { get; set; }
        public Decimal? ViewCount { get; set; }
        public ViewType ViewType { get; set; }
    }
}
