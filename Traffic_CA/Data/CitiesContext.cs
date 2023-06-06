using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class CitiesContext : DbContext
    {
        public CitiesContext (DbContextOptions<CitiesContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Cities> Cities { get; set; } = default!;
    }
}
