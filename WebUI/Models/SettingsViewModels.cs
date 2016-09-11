using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class UsersAll
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public String EMail { get; set; }
        public DateTime DateLock { get; set; }
    }
}