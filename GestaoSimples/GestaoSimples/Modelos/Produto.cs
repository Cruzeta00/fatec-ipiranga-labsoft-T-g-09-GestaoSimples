using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Modelos
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set;}
        public string Codigo { get; set; }
        public string CodigoDeBarras { get; set; }
        public double Preco { get; set; }
        public double Custo { get; set; }
        public int Estoque { get; set; }
        public string Unidade { get; set; }
        public string Categoria { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}
