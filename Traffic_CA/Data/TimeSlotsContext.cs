using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class TimeSlotsContext : DbContext
    {
        public TimeSlotsContext (DbContextOptions<TimeSlotsContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.TimeSlots> TimeSlots { get; set; } = default!;
    }
}
