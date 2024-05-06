using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Modelos
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string EMail { get; set; }
        public bool Ativo {  get; set; }
        public string Observacoes { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataCadastro { get; set; }
        public Classificacao Classificacao { get; set; }
    }

    public enum Classificacao
    {
        PESSIMO = 1,
        RUIM = 2,
        MEDIANO = 3,
        BOM = 4,
        OTIMO = 5
    }
}
