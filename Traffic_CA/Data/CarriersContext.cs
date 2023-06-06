using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class CarriersContext : DbContext
    {
        public CarriersContext (DbContextOptions<CarriersContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Carriers> Carriers { get; set; } = default!;
    }
}
