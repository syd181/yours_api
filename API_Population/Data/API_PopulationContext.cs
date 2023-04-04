using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Population.Models;

namespace API_Population.Data
{
    public class API_PopulationContext : DbContext
    {
        public API_PopulationContext (DbContextOptions<API_PopulationContext> options)
            : base(options)
        {
        }

        public DbSet<API_Population.Models.Pays> Pays { get; set; } = default!;

        public DbSet<API_Population.Models.Population> Population { get; set; } = default!;
    }
}
