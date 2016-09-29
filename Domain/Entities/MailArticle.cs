using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MailArticle
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public DateTime DateMailing { get; set; }
        public int CountRecipient { get; set; }
    }
}
