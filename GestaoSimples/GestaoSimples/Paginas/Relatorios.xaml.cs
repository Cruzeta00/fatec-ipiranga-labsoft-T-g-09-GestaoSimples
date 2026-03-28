using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Relatorios : Page
{
    private readonly ServiceVenda _servicoVendas;
    public Relatorios()
    {
        InitializeComponent();
        _servicoVendas = new ServiceVenda();

        this.Loaded += Relatorios_Carregados;
    }

    private async void Relatorios_Carregados(object sender, RoutedEventArgs e)
    {
        await GraficoWebView.EnsureCoreWebView2Async();

        var labels = new List<string> { "Jan", "Fev", "Mar" };
        var valores = new List<double> { 1000, 2000, 1500 };

        string labelsJson = JsonSerializer.Serialize(labels);
        string valoresJson = JsonSerializer.Serialize(valores);

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
                                datasets: [{{
                                    label: 'Vendas',
                                    data: {valoresJson}
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
}
