using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Modelos
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime DataCompra { get; set; }
        public double ValorTotal { get; set; }
        public int CompradorId { get; set; }
        public int FornecedorId { get; set; }

        public Usuario Comprador { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public List<ItemCompra> ItensCompra { get; set; } = new List<ItemCompra>();
    }
}
