using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class DefaultTimesContext : DbContext
    {
        public DefaultTimesContext (DbContextOptions<DefaultTimesContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.DefaultTimes> DefaultTimes { get; set; } = default!;
    }
}
