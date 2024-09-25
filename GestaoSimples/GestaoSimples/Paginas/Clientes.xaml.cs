using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using GestaoSimples.Servicos;
using GestaoSimples.Modelos;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Clientes : Page
    {
        private readonly ServiceCliente _serviceCliente;
        private List<Modelos.Cliente> listaClientes {  get; set; }

        public Clientes()
        {
            this.InitializeComponent();

            _serviceCliente = new ServiceCliente();

            listaClientes = _serviceCliente.BuscarClientes();
            ClientesListView.ItemsSource = listaClientes;
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            string textoBuscado = Textobusca.Text.ToLower();
            if (listaClientes != null)
            {
                var listaFiltrada = listaClientes.Where(f => f.Nome.ToLower().Contains(textoBuscado) ||
                                                       f.CPF.ToLower().Contains(textoBuscado)).ToList();

                ClientesListView.ItemsSource = listaFiltrada;
            }
        }

        private void botaoCriar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Paginas.Cliente));
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            if (ClientesListView.SelectedItem != null)
            {
                var cliente = ClientesListView.SelectedItem as Modelos.Cliente;

                Frame.Navigate(typeof(Paginas.Cliente), cliente.Id);
            }
        }
    }
}
