﻿using GestaoSimples.Modelos;
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
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<ItemCompra> ItensCompra { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("Configuracoes\\appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlite(connectionString).EnableSensitiveDataLogging();
                //optionsBuilder.UseSqlServer(connectionString).EnableSensitiveDataLogging();
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
                    Nome = "admin",
                    Senha = "admin", // Lembre-se de usar um mecanismo seguro para armazenar senhas em um ambiente de produção
                    Cargo = Cargo.ADMINISTRADOR
                };

                Usuarios.Add(usuarioAdministrador);
                SaveChanges();
            }
        }

        public void VerificaEAdicionarClienteAvulso()
        {
            if(!Clientes.Any(u => u.Nome == "Cliente Avulso"))
            {
                var clienteAvulso = new Cliente
                {
                    Id = 0,
                    CPF = "",
                    Nome = "Cliente Avulso",
                    Telefone = ""
                };

                Clientes.Add(clienteAvulso);
                SaveChanges();
            }
        }
    }
}
