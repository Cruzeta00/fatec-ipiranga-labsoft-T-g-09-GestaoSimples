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
        public Unidade Unidade { get; set; }
        public Categoria Categoria { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }

    public enum Unidade
    {
        Litro = 0,      // L
        Mililitro = 1,  // ml
        Gotas = 2,      // gotas
        Gramas = 3,     // g
        Quilogramas = 4,// kg
        Unidade = 5,    // unid
        Caixa = 6,      // cx
        Pacote = 7,     // pct
        Frasco = 8,     // frasco
        Comprimido = 9, // comp
        Ampola = 10,    // amp
        Tubo = 11,      // tubo
        Sachê = 12,     // sache
        Bisnaga = 13,   // bisnaga
        Envelope = 14,  // envelope
        Cartela = 15,   // cartela
    }

    public enum Categoria
    {
        Medicamento = 0,        // Medicamentos em geral
        Suplemento = 1,         // Suplementos alimentares
        Cosmetico = 2,          // Cosméticos e produtos de beleza
        HigienePessoal = 3,     // Produtos de higiene pessoal
        Perfumaria = 4,         // Perfumaria e fragrâncias
        Bebes = 5,              // Produtos para bebês (fraldas, pomadas, etc.)
        Ortopedico = 6,         // Produtos ortopédicos
        Fitoterapico = 7,       // Medicamentos fitoterápicos
        EquipamentoMedico = 8,  // Equipamentos médicos e acessórios
        Vitaminas = 9,          // Vitaminas e minerais
        AlimentoInfantil = 10,  // Alimentos infantis
        PrimeirosSocorros = 11, // Kits e produtos de primeiros socorros
        Fraldas = 12,           // Fraldas geriátricas e infantis
        ProtetorSolar = 13,     // Protetores solares e bronzeadores
        Homeopatico = 14,       // Medicamentos homeopáticos
        ProdutosNaturais = 15,  // Produtos naturais
        TestesDiagnosticos = 16 // Testes de diagnósticos (gravidez, glicose, etc.)
    }
}
