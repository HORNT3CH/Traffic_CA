using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class CoordinatorsContext : DbContext
    {
        public CoordinatorsContext (DbContextOptions<CoordinatorsContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Coordinators> Coordinators { get; set; } = default!;
    }
}
