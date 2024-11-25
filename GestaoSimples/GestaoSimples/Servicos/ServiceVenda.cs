using GestaoSimples.Data;
using GestaoSimples.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoSimples.Servicos
{
    public class ServiceVenda
    {
        public List<Venda> BuscarVendas()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Vendas.ToList();
            }
        }

        public Venda BuscarVenda(int id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return (Venda)contexto.Vendas.FirstOrDefault(x => x.Id == id);
            }
        }

        public int NovaVenda()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return (int)contexto.Vendas.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }

        public void AdicionarVenda(Venda venda)
        {
            using(var contexto = new ContextoGestaoSimples())
            {
                contexto.Vendas.Add(venda);

                foreach (var item in venda.ItensVenda)
                {
                    var produto = contexto.Produtos.FirstOrDefault(p => p.Id == item.ProdutoId);
                    if (produto != null)
                    {
                        produto.Estoque -= item.Quantidade;
                        contexto.Entry(produto).State = EntityState.Modified;
                    }
                }

                contexto.SaveChanges();
            }
        }

        public List<ItemVenda> BuscarItensVenda(int idVenda)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.ItensVenda.Where(x => x.VendaId == idVenda).ToList();
            }
        }
    }
}
