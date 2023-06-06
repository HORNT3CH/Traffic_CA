using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class LoadsContext : DbContext
    {
        public LoadsContext (DbContextOptions<LoadsContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Loads> Loads { get; set; } = default!;
        public DbSet<Traffic_CA.Models.TimeSlots> TimeSlots { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Carriers> Carriers { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Cities> Cities { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Coordinators> Coordinators { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Customers> Customers { get; set; } = default!;
        public DbSet<Traffic_CA.Models.States> States { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Associate> Associate { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Docklot> Docklot { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Doors> Doors { get; set; } = default!;
        public DbSet<Traffic_CA.Models.DefaultTimes> DefaultTimes { get; set; } = default!;
        public DbSet<Traffic_CA.Models.Lots> Lots { get; set; } = default!;
    }
}
