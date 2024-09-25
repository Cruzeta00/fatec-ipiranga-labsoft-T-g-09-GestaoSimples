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

        public int BuscarNovoFornecedor()
        {
            using(var contexto = new ContextoGestaoSimples())
            {
                return (int)contexto.Fornecedores.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }

        public void AdicionarFornecedor(Modelos.Fornecedor forn)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Fornecedores.Add(forn);
                contexto.SaveChanges();
            }
        }

        public void AtualizarFornecedor(Modelos.Fornecedor forn)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Fornecedores.Update(forn);
                contexto.SaveChanges();
            }
        }
    }
}
