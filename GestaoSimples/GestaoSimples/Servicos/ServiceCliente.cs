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
        public string BuscarNomeClientePorCPF(string cpf)
        {
            using(var contexto = new ContextoGestaoSimples())
            {
                return contexto.Clientes.Where(x => x.CPF == cpf).Select(x => x.Nome).FirstOrDefault();
            }
        }
    }
}
