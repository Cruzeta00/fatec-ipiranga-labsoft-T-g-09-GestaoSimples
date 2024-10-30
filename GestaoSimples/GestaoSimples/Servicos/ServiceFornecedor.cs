using GestaoSimples.Data;
using GestaoSimples.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace GestaoSimples.Servicos
{
    internal class ServiceFornecedor
    {
        public List<Fornecedor> BuscarFornecedores()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Fornecedores.OrderByDescending(x => x.Ativo).ThenBy(x => x.Id).ToList();
            }
        }

        public Fornecedor BuscarFornecedor(int Id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return (Fornecedor)contexto.Fornecedores.FirstOrDefault(x => x.Id == Id);
            }
        }

        public int BuscarNovoFornecedor()
        {
            using(var contexto = new ContextoGestaoSimples())
            {
                return (int)contexto.Fornecedores.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }

        public int AdicionarFornecedor(Modelos.Fornecedor forn)
        {
            int retorno = -1;
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Fornecedores.Add(forn);
                retorno = contexto.SaveChanges();
            }
            return retorno;
        }

        public int AtualizarFornecedor(Modelos.Fornecedor forn)
        {
            int retorno = -1;
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Fornecedores.Update(forn);
                retorno = contexto.SaveChanges();
            }
            return retorno;
        }
    }
}
