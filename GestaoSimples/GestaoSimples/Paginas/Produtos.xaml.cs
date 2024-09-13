using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Produtos : Page
    {
        private readonly ServiceProduto _servicoProduto;
        private List<Modelos.Produto> listaProdutos{ get; set; }

        public Produtos()
        {
            this.InitializeComponent();

            _servicoProduto = new ServiceProduto();
        }

        private void botaoBuscar_Click(object sender, RoutedEventArgs e)
        {
            var produtos = _servicoProduto.BuscarProdutos();
            listaProdutos = produtos;
            ProdutosListView.ItemsSource = produtos;

            if(ProdutosListView == null)
            {
                
            }
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            if (ProdutosListView.SelectedItem != null)
            {
                var produto = ProdutosListView.SelectedItem as Modelos.Produto;

                Frame.Navigate(typeof(Produto), produto.Id);
            }
            else
            {
                Frame.Navigate(typeof(Produto));
            }
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            string textoBuscado = Textobusca.Text.ToLower();
            var listaFiltrada = listaProdutos.Where(f => f.Nome.ToLower().Contains(textoBuscado) ||
                                                       f.Descricao.ToLower().Contains(textoBuscado) ||
                                                       f.Categoria.ToLower().Contains(textoBuscado)).ToList();

            ProdutosListView.ItemsSource = listaFiltrada;
        }
    }
}
