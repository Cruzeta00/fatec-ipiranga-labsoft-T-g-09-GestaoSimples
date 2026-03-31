using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RelatorioCV : Page
    {
        private readonly ServiceVenda _servicoVendas;
        private readonly ServiceCompra _servicoCompras;
        public RelatorioCV()
        {
            InitializeComponent();
            _servicoVendas = new ServiceVenda();
            _servicoCompras = new ServiceCompra();

            this.Loaded += Relatorios_Carregados;
        }

        private async void Relatorios_Carregados(object sender, RoutedEventArgs e)
        {
            await GraficoWebView.EnsureCoreWebView2Async();

            CarregarAnos();

            if (ComboAno.SelectedItem != null)
            {
                AtualizarGrafico((int)ComboAno.SelectedItem);
            }
        }

        private void ComboAno_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboAno.SelectedItem == null)
                return;

            int anoSelecionado = (int)ComboAno.SelectedItem;

            AtualizarGrafico(anoSelecionado);
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

        private void AtualizarGrafico(int ano)
        {
            var vendas = _servicoVendas
                .BuscarVendas()
                .Where(v => DateTime.Parse(v.DataVendaFormatada).Year == ano)
                .GroupBy(v => DateTime.Parse(v.DataVendaFormatada).Month)
                .Select(g => new
                {
                    NumeroMes = g.Key,
                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key),
                    Total = g.Sum(x => x.ValorTotal)
                })
                .OrderBy(x => x.Mes)
                .ToList();

            var compras = _servicoCompras
                .BuscarCompras()
                .Where(c => DateTime.Parse(c.DataCompraFormatada).Year == ano)
                .GroupBy(c => DateTime.Parse(c.DataCompraFormatada).Month)
                .Select(g => new
                {
                    NumeroMes = g.Key,
                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key),
                    Total = g.Sum(x => x.ValorTotal)
                })
                .OrderBy(x => x.Mes)
                .ToList();

            var labels = vendas.OrderBy(v => v.NumeroMes).Select(v => v.Mes).ToList();
            var valoresVendas = vendas.Select(v => v.Total).ToList();
            var valoresCompras = compras.Select(c => c.Total).ToList();

            string labelsJson = JsonSerializer.Serialize(labels);
            string vendasJson = JsonSerializer.Serialize(valoresVendas);
            string comprasJson = JsonSerializer.Serialize(valoresCompras);

            string html = $@"
                            <html>
                            <head>
                            <meta charset='utf-8'>
                            <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
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
                                type: 'bar',
                                data: {{
                                    labels: {labelsJson},
                                    datasets: [
                                    {{
                                        label: 'Compras',
                                        data: {comprasJson}
                                    }},
                                    {{
                                        label: 'Vendas',
                                        data: {vendasJson}
                                    }}
                                    ]
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
    }
}
