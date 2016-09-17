﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Entities;

namespace Domain.Context
{
    public class mainDbContext : DbContext
    {
        public mainDbContext()
            : base("mainDbContext")
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        //public DbSet<AspNetUser> AspNetUsers { get; set; }
    }
}