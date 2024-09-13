using GestaoSimples.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace GestaoSimples.Data
{
    public class ContextoGestaoSimples : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("Configuracoes\\appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
            //optionsBuilder.UseSqlServer(@"Data Source=CRUZETOBOOK\SQLEXPRESS;Initial Catalog=GestaoSimples;Integrated Security=true;");
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
