using Microsoft.EntityFrameworkCore;

namespace SuperHeldAPI.Datenzugriff
{
    public class DatenzugriffKontext : DbContext
    {
        public DatenzugriffKontext(DbContextOptions<DatenzugriffKontext> options) : base(options) { }
        public DbSet<SuperHeld> SuperHelden { get; set; }
    }
}
