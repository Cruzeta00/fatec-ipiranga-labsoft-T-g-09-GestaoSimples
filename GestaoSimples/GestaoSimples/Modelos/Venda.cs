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

        public List<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
    }
}
