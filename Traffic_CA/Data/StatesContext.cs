using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class StatesContext : DbContext
    {
        public StatesContext (DbContextOptions<StatesContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.States> States { get; set; } = default!;
    }
}
