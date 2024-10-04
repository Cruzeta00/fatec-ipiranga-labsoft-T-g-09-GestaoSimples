using GestaoSimples.Data;
using GestaoSimples.Modelos;
using GestaoSimples.Paginas;
using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Janelas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Fornecedores : Page
    {
        private readonly ServiceFornecedor _servicoFornecedor;
        private List<Modelos.Fornecedor> listaFornecedores { get; set; }

        public Fornecedores()
        {
            this.InitializeComponent();

            _servicoFornecedor = new ServiceFornecedor();

            listaFornecedores = _servicoFornecedor.BuscarFornecedores();
            FornecedoresListView.ItemsSource = listaFornecedores;

            NenhumFornecedor.Visibility = Visibility.Collapsed;

            if (FornecedoresListView == null)
            {
                NenhumFornecedor.Visibility = Visibility.Visible;
            }
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            if (FornecedoresListView.SelectedItem != null)
            {
                var fornecedor = FornecedoresListView.SelectedItem as Modelos.Fornecedor;

                Frame.Navigate(typeof(Paginas.Fornecedor), fornecedor.Id);
            }
        }

        private void botaoCriar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Paginas.Fornecedor));
        }
        
        private void Buscando(object sender, TextChangedEventArgs e)
        {
            string textoBuscado = Textobusca.Text.ToLower();
            if(listaFornecedores != null)
            {
                var listaFiltrada = listaFornecedores.Where(f => f.Nome.ToLower().Contains(textoBuscado) ||
                                                       f.CNPJ.ToLower().Contains(textoBuscado) ||
                                                       f.EMail.ToLower().Contains(textoBuscado) ||
                                                       f.Observacoes.ToLower().Contains(textoBuscado) ||
                                                       f.Classificacao.ToString().ToLower().Contains(textoBuscado)).ToList();

                FornecedoresListView.ItemsSource = listaFiltrada;
            }
        }
    }
}
