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
    public class ServiceCompra
    {
        public List<Compra> BuscarCompras()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Compras.ToList();
            }
        }

        public Compra BuscarCompra(int id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return (Compra)contexto.Compras.FirstOrDefault(x => x.Id == id);
            }
        }

        public int NovaCompra()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return (int)contexto.Compras.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }

        public void AdicionarCompra(Compra compra)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                using(var transaction = contexto.Database.BeginTransaction())
                {
                    try
                    {
                        contexto.Compras.Add(compra);

                        foreach (var item in compra.ItensCompra)
                        {
                            var produto = contexto.Produtos.SingleOrDefault(p => p.Id == item.ProdutoId);
                            if (produto == null)
                                throw new InvalidOperationException($"Produto com Nome '{produto.Nome}' não encontrado.");
                            
                            produto.Estoque += item.Quantidade;

                            if (produto.Estoque > 0)
                                produto.Ativo = true;

                            contexto.Produtos.Update(produto);
                        }

                        contexto.SaveChanges();
                        transaction.Commit();
                    }
                    catch (System.Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }

        public List<ItemCompra> BuscarItensCompra(int idCompra)
        {
            using(var contexto = new ContextoGestaoSimples())
            {
                return contexto.ItensCompra.Where(x => x.CompraId == idCompra).ToList();
            }
        }
    }
}
