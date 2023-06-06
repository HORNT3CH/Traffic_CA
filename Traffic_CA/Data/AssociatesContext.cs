using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Models;

namespace Traffic_CA.Data
{
    public class AssociatesContext : DbContext
    {
        public AssociatesContext (DbContextOptions<AssociatesContext> options)
            : base(options)
        {
        }

        public DbSet<Traffic_CA.Models.Associate> Associate { get; set; } = default!;
    }
}
