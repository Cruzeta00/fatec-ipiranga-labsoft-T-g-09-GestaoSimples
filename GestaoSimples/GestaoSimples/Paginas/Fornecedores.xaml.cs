using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

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
