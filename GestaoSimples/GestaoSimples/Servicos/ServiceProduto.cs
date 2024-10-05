using GestaoSimples.Data;
using GestaoSimples.Modelos;
using Microsoft.EntityFrameworkCore;
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
                return contexto.Produtos.Include(f => f.Fornecedor).ToList();
            }
        }

        public Produto BuscarProduto(int Id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Produtos.FirstOrDefault(x => x.Id == Id);
            }
        }

        public int BuscarNovoProduto()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Produtos.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }

        public int AdicionarProduto(Produto prod)
        {
            int retorno = -1;
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Produtos.Add(prod);
                retorno = contexto.SaveChanges();
            }
            return retorno;
        }

        public int AtualizarProduto(Produto prod)
        {
            int retorno = -1;
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Produtos.Update(prod);
                retorno = contexto.SaveChanges();
            }
            return retorno;
        }
    }
}
