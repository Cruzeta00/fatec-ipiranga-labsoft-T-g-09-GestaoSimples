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
        private readonly ServiceVenda _servicoVenda;
        private readonly ServiceCliente _servicoCliente;
        private readonly ServiceUsuario _servicoUsuario;
        private List<Modelos.Venda> listaVendas { get; set; }

        public Vendas()
        {
            this.InitializeComponent();

            _servicoVenda = new ServiceVenda();
            _servicoCliente = new ServiceCliente();
            _servicoUsuario = new ServiceUsuario();

            listaVendas = _servicoVenda.BuscarVendas();
            listaVendas = PreencheNomes(listaVendas);
            VendasListView.ItemsSource = listaVendas;

            NenhumaVenda.Visibility = Visibility.Collapsed;

            if(VendasListView == null)
            {
                NenhumaVenda.Visibility = Visibility.Visible;
            }
        }

        private List<Modelos.Venda> PreencheNomes(List<Modelos.Venda> listaVendas)
        {
            foreach(Modelos.Venda v in  listaVendas)
            {
                v.Cliente = _servicoCliente.BuscarCliente(v.ClienteId);
                v.Vendedor = _servicoUsuario.BuscarUsuario(v.VendedorId);
            }
            return listaVendas;
        }

        private void botaoAdicionar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Paginas.Venda));
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            string textoBuscado = Textobusca.Text.ToLower();

            if(listaVendas != null)
            {
                var listaFiltrada = listaVendas.Where(f => f.Cliente.Nome.ToLower().Contains(textoBuscado) ||
                                                           f.Vendedor.Nome.ToLower().Contains(textoBuscado) ||
                                                           f.ItensVenda.ToString().ToLower().Contains(textoBuscado)).ToList();

                VendasListView.ItemsSource = listaFiltrada;
            }
        }

        private void ClickVenda_ItemLine(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ItensVenda), e.ClickedItem);
        }
    }
}
