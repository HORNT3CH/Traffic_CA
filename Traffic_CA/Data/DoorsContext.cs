using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class DoorsContext : DbContext
    {
        public DoorsContext (DbContextOptions<DoorsContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Doors> Doors { get; set; } = default!;
    }
}
