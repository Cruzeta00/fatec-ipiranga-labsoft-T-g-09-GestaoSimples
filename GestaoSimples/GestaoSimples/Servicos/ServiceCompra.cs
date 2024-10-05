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
                contexto.Compras.Add(compra);

                foreach (var item in compra.ItensCompra)
                {
                    var produto = contexto.Produtos.FirstOrDefault(p => p.Id == item.ProdutoId);
                    if (produto != null)
                    {
                        if (produto.FornecedorId != compra.FornecedorId)
                        {
                            throw new InvalidOperationException("O fornecedor do produto não corresponde ao fornecedor da compra.");
                        }

                        produto.Estoque += item.Quantidade;
                        contexto.Entry(produto).State = EntityState.Modified;
                    }
                }

                contexto.SaveChanges();
            }
        }
    }
}
