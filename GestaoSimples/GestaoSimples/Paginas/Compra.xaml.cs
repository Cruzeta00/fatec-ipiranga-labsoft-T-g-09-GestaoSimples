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
    public sealed partial class Compra : Page, INotifyPropertyChanged
    {
        private double _valorTotal = 0;

        private readonly ServiceCompra _servicoCompra;
        private readonly ServiceProduto _servicoProduto;
        private readonly ServiceFornecedor _servicoFornecedor;

        private List<Modelos.Produto> listaProdutos { get; set; }
        private List<ItemCompra> listaCompra { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public double ValorTotal
        {
            get { return _valorTotal; }
            set
            {
                _valorTotal = Math.Ceiling(value * 100) / 100;

                OnPropertyChanged(nameof(ValorTotal));
            }
        }

        public Compra()
        {
            this.InitializeComponent();

            _servicoCompra = new ServiceCompra();
            _servicoProduto = new ServiceProduto();
            _servicoFornecedor = new ServiceFornecedor();

            listaProdutos = _servicoProduto.BuscarProdutos();
            ProdutosListView.ItemsSource = listaProdutos;

            NenhumProduto.Visibility = Visibility.Visible;
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            string textoBuscado = Textobusca.Text.ToLower();
            var listaFiltrada = listaProdutos.Where(f => f.Nome.ToLower().Contains(textoBuscado) ||
                                                       f.Descricao.ToLower().Contains(textoBuscado) ||
                                                       f.Categoria.ToString().ToLower().Contains(textoBuscado) ||
                                                       f.Fornecedor.Nome.ToLower().Contains(textoBuscado)).ToList();

            ProdutosListView.ItemsSource = listaFiltrada;
        }

        private void AtualizarValorTotal()
        {
            ValorTotal = listaCompra.Sum(item => item.ValorTotalItem);
        }

        private async void BotaoAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if (ItensCompraListView.Items.Count == 0)
            {
                ContentDialog msgErro = new ContentDialog
                {
                    Title = "Erro - Nenhum Produto Selecionado",
                    Content = "Necessário selecionar pelo menos 1 produto para adicionar a compra.",
                    CloseButtonText = "OK",
                };
                msgErro.XamlRoot = botaoAddCompra.XamlRoot;
                await msgErro.ShowAsync();
            }
            else
            {
                var resultado = await ConfirmarCompraDialog.ShowAsync();

                if (resultado == ContentDialogResult.Primary)
                {
                    Frame.Navigate(typeof(Compras));
                }
                else
                {
                    LimparCompra();
                }
            }
        }

        private void LimparCompra()
        {
            listaCompra.Clear();
            ItensCompraListView.ItemsSource = null;
            ValorTotal = 0;
            NenhumProduto.Visibility = Visibility.Visible;
        }

        private void ConfirmarCompra_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {


                Modelos.Compra compra = new Modelos.Compra();
                compra.CompradorId = SessaoUsuario.Instancia.UsuarioId;
                compra.DataCompra = DateTime.Now;
                compra.ItensCompra = listaCompra;
                compra.ValorTotal = _valorTotal;
                VerificarFornecedoresUnicos(compra.ItensCompra);
                compra.FornecedorId = 1;

                _servicoCompra.AdicionarCompra(compra);
            }
            catch (Exception ex)
            {

            }
        }

        private void CancelarCompra_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        private void ClickItemLista(object sender, ItemClickEventArgs e)
        {
            NenhumProduto.Visibility = Visibility.Collapsed;

            Modelos.Produto produtoSelecionado = (Modelos.Produto)e.ClickedItem;

            if (listaCompra == null)
            {
                listaCompra = new List<ItemCompra>();
            }

            var itemVendaExistente = listaCompra.FirstOrDefault(i => i.Produto.Id == produtoSelecionado.Id);

            if (itemVendaExistente != null)
            {
                itemVendaExistente.Quantidade++;
                itemVendaExistente.ValorTotalItem = itemVendaExistente.Quantidade * itemVendaExistente.Produto.Preco;
            }
            else
            {
                var novoItemVenda = new ItemCompra
                {
                    Produto = produtoSelecionado,
                    Quantidade = 1,
                    ValorTotalItem = produtoSelecionado.Preco
                };

                listaCompra.Add(novoItemVenda);
            }

            ItensCompraListView.ItemsSource = null;
            ItensCompraListView.ItemsSource = listaCompra;

            AtualizarValorTotal();
        }

        public async void VerificarFornecedoresUnicos(List<ItemCompra> itensCompra)
        {
            
            var fornecedores = itensCompra.Select(i => i.Produto.FornecedorId).Distinct();

            if (fornecedores.Count() > 1)
            {
                ContentDialog msgErro = new ContentDialog
                {
                    Title = "Erro - Multiplos Fornecedores",
                    Content = "Necessário selecionar apenas produtos de 1 Fornecedor para adicionar a Compra.",
                    CloseButtonText = "OK",
                };
                msgErro.XamlRoot = botaoAddCompra.XamlRoot;
                await msgErro.ShowAsync();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
