using GestaoSimples.Data;
using GestaoSimples.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Servicos
{
    public class ServiceCliente
    {
        public void AdicionarCliente(Cliente cliente)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Clientes.Add(cliente);
                contexto.SaveChanges();
            }
        }

        public void AtualizarCliente(Cliente cliente)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Clientes.Update(cliente);
                contexto.SaveChanges();
            }
        }

        public List<Cliente> BuscarClientes()
        {
            using(var contexto = new ContextoGestaoSimples())
            {
                return contexto.Clientes.ToList();
            }
        }

        public Cliente BuscarCliente(int ID)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Clientes.FirstOrDefault(x => x.Id == ID);
            }
        }

        public string BuscarNomeClientePorCPF(string cpf)
        {
            using(var contexto = new ContextoGestaoSimples())
            {
                return contexto.Clientes.Where(x => x.CPF == cpf).Select(x => x.Nome).FirstOrDefault();
            }
        }

        public int BuscarNovoCliente()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return (int)contexto.Clientes.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }
    }
}
