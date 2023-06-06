using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class CustomersContext : DbContext
    {
        public CustomersContext (DbContextOptions<CustomersContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Customers> Customers { get; set; } = default!;
    }
}
