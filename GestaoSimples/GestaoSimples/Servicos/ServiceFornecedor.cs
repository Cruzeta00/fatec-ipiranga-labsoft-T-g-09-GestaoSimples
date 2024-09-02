using GestaoSimples.Data;
using GestaoSimples.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Servicos
{
    internal class ServiceFornecedor
    {
        public List<Fornecedor> BuscarFornecedores()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Fornecedores.ToList();
            }
        }

        public Fornecedor BuscarFornecedor(int Id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return (Fornecedor)contexto.Fornecedores.FirstOrDefault(x => x.Id == Id);
            }
        }

        public void MudarStatus(int Id)
        {
            using(var contexto =new ContextoGestaoSimples())
            {
                Fornecedor fornecedor = BuscarFornecedor(Id);
                fornecedor.Ativo = !fornecedor.Ativo;
                contexto.Fornecedores.Update(fornecedor);
                contexto.SaveChanges();
            }
        }
    }
}
