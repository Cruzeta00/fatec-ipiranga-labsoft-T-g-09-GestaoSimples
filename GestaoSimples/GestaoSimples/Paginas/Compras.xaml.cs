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
using System.IO;
using System.Linq;
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
    public sealed partial class Compras : Page
    {
        private readonly ServiceCompra _servicoCompra;
        private readonly ServiceFornecedor _servicoFornecedor;
        private readonly ServiceUsuario _servicoUsuario;

        private List<Modelos.Compra> listaCompras {  get; set; }

        public Compras()
        {
            this.InitializeComponent();

            _servicoCompra = new ServiceCompra();
            _servicoFornecedor = new ServiceFornecedor();
            _servicoUsuario = new ServiceUsuario();

            listaCompras = _servicoCompra.BuscarCompras();
            listaCompras = PreencheNomes(listaCompras);
            ComprasListView.ItemsSource = listaCompras;

            NenhumaCompra.Visibility = Visibility.Collapsed;

            if(ComprasListView == null)
            {
                NenhumaCompra.Visibility = Visibility.Visible;
            }
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            string textoBuscado = Textobusca.Text.ToLower();

            if(listaCompras != null)
            {
                var listaFiltrada = listaCompras.Where(f => f.Fornecedor.Nome.ToLower().Contains(textoBuscado) ||
                                                           f.Comprador.Nome.ToLower().Contains(textoBuscado) ||
                                                           f.ItensCompra.ToString().ToLower().Contains(textoBuscado)).ToList();

                ComprasListView.ItemsSource = listaFiltrada;
            }
        }

        private void BotaoAdicionar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Compra));
        }

        private List<Modelos.Compra> PreencheNomes(List<Modelos.Compra> listaCompras)
        {
            foreach (Modelos.Compra c in listaCompras)
            {
                c.Fornecedor = _servicoFornecedor.BuscarFornecedor(c.FornecedorId);
                c.Comprador = _servicoUsuario.BuscarUsuario(c.CompradorId);
            }
            return listaCompras;
        }

        private void ComprasListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ItensCompra), e.ClickedItem);
        }
    }
}
