using GestaoSimples.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestaoSimples.Data
{
    public class ContextoGestaoSimples : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=CRUZETOBOOK\SQLEXPRESS;Initial Catalog=GestaoSimples;Integrated Security=true;");
        }

        public void VerificarEAdicionarUsuarioAdministrador()
        {
            if (!Usuarios.Any(u => u.Cargo == Cargo.ADMINISTRADOR))
            {
                var usuarioAdministrador = new Usuario
                {
                    Login = "admin",
                    Senha = "admin", // Lembre-se de usar um mecanismo seguro para armazenar senhas em um ambiente de produção
                    Cargo = Cargo.ADMINISTRADOR
                };

                Usuarios.Add(usuarioAdministrador);
                SaveChanges();
            }
        }
    }
}
