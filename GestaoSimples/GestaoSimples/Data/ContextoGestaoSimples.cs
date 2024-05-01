using GestaoSimples.Modelos;
using Microsoft.EntityFrameworkCore;

namespace GestaoSimples.Data
{
    public class ContextoGestaoSimples : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=CRUZETOBOOK\SQLEXPRESS;Initial Catalog=GestaoSimples;Integrated Security=true;");
        }
    }
}
