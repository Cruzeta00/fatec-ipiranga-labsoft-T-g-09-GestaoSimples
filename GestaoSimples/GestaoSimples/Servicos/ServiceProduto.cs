using GestaoSimples.Data;
using GestaoSimples.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Servicos
{
    public class ServiceProduto
    {
        public List<Produto> BuscarProdutos()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Produtos.ToList();
            }
        }

        public Produto BuscarProduto(int Id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Produtos.FirstOrDefault(x => x.Id == Id);
            }
        }

        public void MudarStatus(int Id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                Produto produto = BuscarProduto(Id);
                produto.Ativo = !produto.Ativo;
                contexto.Produtos.Update(produto);
                contexto.SaveChanges();
            }
        }

        public int BuscarNovoProduto()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Produtos.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }

        public void AdicionarProduto(Produto prod)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Produtos.Add(prod);
                contexto.SaveChanges();
            }
        }

        public void AtualizarProduto(Produto prod)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Produtos.Update(prod);
                contexto.SaveChanges();
            }
        }
    }
}
