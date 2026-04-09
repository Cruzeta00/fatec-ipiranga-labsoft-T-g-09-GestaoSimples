using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RelatorioItens : Page
{
    private readonly ServiceVenda _servicoVendas;
    private readonly ServiceCompra _servicoCompras;
    private readonly ServiceProduto _servicoProdutos;

    public RelatorioItens()
    {
        InitializeComponent();
        _servicoVendas = new ServiceVenda();
        _servicoCompras = new ServiceCompra();
        _servicoProdutos = new ServiceProduto();

        this.Loaded += RelatorioItens_Carregado;
    }

    private async void RelatorioItens_Carregado(object sender, RoutedEventArgs e)
    {
        await GraficoWebView.EnsureCoreWebView2Async();

        CarregarItens();
        CarregarMeses();
        CarregarAnos();

        if (ComboAno.SelectedItem != null)
        {
            AtualizarGrafico();
        }
    }

    private void CarregarItens()
    {
        var itens = _servicoProdutos
        .BuscarProdutos()
        .OrderBy(p => p.Descricao)
        .ToList();

        ComboItem.ItemsSource = itens;

        ComboItem.DisplayMemberPath = "Nome";
        ComboItem.SelectedValuePath = "Id";

        if (itens.Any())
            ComboItem.SelectedIndex = 0;
    }

    private void CarregarMeses()
    {
        var meses = Enumerable.Range(1, 12).ToList();

        ComboMes.ItemsSource = meses;

        ComboMes.SelectedItem = DateTime.Now.Month;
    }

    private void CarregarAnos()
    {
        var anos = _servicoVendas
            .BuscarVendas()
            .Select(v => DateTime.Parse(v.DataVendaFormatada).Year)
            .Distinct()
            .OrderBy(a => a)
            .ToList();

        ComboAno.ItemsSource = anos;

        int anoAtual = DateTime.Now.Year;

        if (anos.Contains(anoAtual))
            ComboAno.SelectedItem = anoAtual;
        else
            ComboAno.SelectedIndex = anos.Count - 1;
    }

    private void AtualizarGrafico()
    {
        if (ComboItem.SelectedValue == null ||
            ComboMes.SelectedItem == null ||
            ComboAno.SelectedItem == null)
            return;

        int itemId = (int)ComboItem.SelectedValue;
        int mes = (int)ComboMes.SelectedItem;
        int ano = (int)ComboAno.SelectedItem;


        DateTime inicioMes = new DateTime(ano, mes, 1);
        DateTime fimMes = inicioMes.AddMonths(1).AddDays(-1);


        var compras = _servicoCompras.BuscarCompras();
        var vendas = _servicoVendas.BuscarVendas();


        decimal estoqueInicial = CalcularEstoqueInicial(itemId, inicioMes);


        List<string> labels = new();
        List<decimal> estoquePorDia = new();

        decimal estoqueAtual = estoqueInicial;


        for (DateTime dia = inicioMes; dia <= fimMes; dia = dia.AddDays(1))
        {
            decimal comprasDia = compras
                .Where(c => DateTime.Parse(c.DataCompraFormatada).Date == dia)
                .SelectMany(c => c.ItensCompra)
                .Where(i => i.ProdutoId == itemId)
                .Sum(i => i.Quantidade);

            decimal vendasDia = vendas
                .Where(v => DateTime.Parse(v.DataVendaFormatada).Date == dia)
                .SelectMany(v => v.ItensVenda)
                .Where(i => i.ProdutoId == itemId)
                .Sum(i => i.Quantidade);


            estoqueAtual += comprasDia;
            estoqueAtual -= vendasDia;


            labels.Add(dia.Day.ToString());
            estoquePorDia.Add(estoqueAtual);
        }


        RenderizarGrafico(labels, estoquePorDia);
    }

    private decimal CalcularEstoqueInicial(int itemId, DateTime dataInicio)
    {
        var produto = _servicoProdutos
            .BuscarProdutos()
            .FirstOrDefault(p => p.Id == itemId);

        decimal estoqueAtual = produto?.Estoque ?? 0;

        int teste = _servicoCompras.BuscarCompras().First().ItensCompra.Count;

        var comprasDepois = _servicoCompras.BuscarCompras()
            .Where(c => DateTime.Parse(c.DataCompraFormatada) >= dataInicio)
            .SelectMany(c => c.ItensCompra)
            .Where(i => i.ProdutoId == itemId)
            .Sum(i => i.Quantidade);


        var vendasDepois = _servicoVendas.BuscarVendas()
            .Where(v => DateTime.Parse(v.DataVendaFormatada) >= dataInicio)
            .SelectMany(v => v.ItensVenda)
            .Where(i => i.ProdutoId == itemId)
            .Sum(i => i.Quantidade);


        return estoqueAtual - comprasDepois + vendasDepois;
    }

    private void RenderizarGrafico(List<string> labels, List<decimal> valores)
    {
        string labelsJson = JsonSerializer.Serialize(labels);
        string valoresJson = JsonSerializer.Serialize(valores);

        string chartJs = File.ReadAllText("Recursos/chart.js");

        string html = $@"
                        <html>
                        <head>
                        <meta charset='utf-8'>
                        <script>{chartJs}</script>

                        <style>
                        html, body {{
                        margin:0;
                        padding:0;
                        height:100%;
                        }}

                        #grafico {{
                        width:100%;
                        height:100%;
                        }}
                        </style>

                        </head>

                        <body>

                        <canvas id='grafico'></canvas>

                        <script>

                        window.onload = function() {{

                        const ctx = document.getElementById('grafico');

                        new Chart(ctx, {{
                        type: 'line',
                        data: {{

                        labels: {labelsJson},

                        datasets: [{{
                        label: 'Estoque diário',
                        data: {valoresJson},
                        fill: false,
                        tension: 0.2
                        }}]

                        }},

                        options: {{
                        responsive: true,
                        maintainAspectRatio: false
                        }}

                        }});

                        }}

                        </script>

                        </body>
                        </html>";

        GraficoWebView.NavigateToString(html);
    }

    private void Filtro_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        AtualizarGrafico();
    }
}
