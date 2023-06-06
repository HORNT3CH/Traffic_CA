using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class DocklotsContext : DbContext
    {
        public DocklotsContext (DbContextOptions<DocklotsContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Docklot> Docklot { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Lots> Lots { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Doors> Doors { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Carriers> Carriers { get; set; } = default!;
    }
}
