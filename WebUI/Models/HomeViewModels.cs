using Domain.Entities;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class PartnersView
    {
        public List<Partner> Partners { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}