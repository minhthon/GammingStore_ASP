using DoAnASP.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Data
{
    public class DpContext:DbContext
    {
        public DpContext(DbContextOptions<DpContext> options) : base(options)
        {

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Category> Category { get; set; }

        public DbSet<Product> Product { get; set; }
        public DbSet<Payment> Payment { get; set; }

    }
}
