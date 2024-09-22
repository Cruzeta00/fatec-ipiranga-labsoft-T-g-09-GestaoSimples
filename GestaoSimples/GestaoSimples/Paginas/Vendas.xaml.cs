using GestaoSimples.Modelos;
using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Vendas : Page
    {
        private double _valorTotal;

        private readonly ServiceVenda _servicoVenda;
        private List<Modelos.ItemVenda> listaVendas { get; set; }

        public Vendas()
        {
            this.InitializeComponent();

            _servicoVenda = new ServiceVenda();
            NenhumaVenda.Visibility = Visibility.Visible;
        }

        private async void botaoAdicionar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Paginas.Venda));
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            /*string textoBuscado = Textobusca.Text.ToLower();
            var listaFiltrada = listaVendas.Where(f => f.Nome.ToLower().Contains(textoBuscado) ||
                                                       f.Descricao.ToLower().Contains(textoBuscado) ||
                                                       f.Categoria.ToString().ToLower().Contains(textoBuscado)).ToList();

            VendasListView.ItemsSource = listaFiltrada;*/
        }

        private void ClickVendaLista(object sender, ItemClickEventArgs e)
        {

        }
    }
}
