using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Modelos
{
    public class ItemCompra
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public double ValorTotalItem { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }


        public int CompraId { get; set; }
        public Compra Compra { get; set; }
    }
}
