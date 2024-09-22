using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Modelos
{
    public class Venda
    {
        public int Id {  get; set; }
        public DateTime DataVenda { get; set; }
        public double ValorTotal { get; set; }
        public int VendedorId { get; set; }
        public int ClienteId { get; set; }

        public Usuario Vendedor { get; set; }
        public Cliente Cliente { get; set; }
        public List<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
    }
}
