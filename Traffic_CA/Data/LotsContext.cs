using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class LotsContext : DbContext
    {
        public LotsContext (DbContextOptions<LotsContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Lots> Lots { get; set; } = default!;
    }
}
