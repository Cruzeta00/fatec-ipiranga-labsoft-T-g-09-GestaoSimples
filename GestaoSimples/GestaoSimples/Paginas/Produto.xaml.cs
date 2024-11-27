using GestaoSimples.Modelos;
using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Produto : Page
    {
        private readonly ServiceProduto _servicoProduto;
        private readonly ServiceFornecedor _servicoFornecedor;
        Unidade _unidade;
        Categoria _categoria;
        Modelos.Fornecedor _fornecedor;


        public Produto()
        {
            this.InitializeComponent();

            _servicoProduto = new ServiceProduto();
            _servicoFornecedor = new ServiceFornecedor();

            Unidade.ItemsSource = Enum.GetValues(typeof(Unidade));
            Categoria.ItemsSource = Enum.GetValues(typeof(Categoria));
            Forn.ItemsSource = _servicoFornecedor.BuscarFornecedores();

            Unidade.SelectionChanged += SelecaoUnidade;
            Categoria.SelectionChanged += SelecaoCategoria;
            Forn.SelectionChanged += SelecaoFornecedor;

            Forn.DisplayMemberPath = "Nome";
            Forn.SelectedValuePath = "Id";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                botao.Content = "Adicionar";
                PreencherDadosProduto();
            }
            else
            {
                botao.Content = "Atualizar";
                PreencherDadosProduto(e.Parameter.ToString());
            }
        }
        
        private void PreencherDadosProduto(string idProduto)
        {
            try
            {
                if (String.IsNullOrEmpty(idProduto))
                {
                    int novo = _servicoProduto.BuscarNovoProduto();

                    Id.Text = novo.ToString();
                }
                else
                {
                    Modelos.Produto prod = _servicoProduto.BuscarProduto(Convert.ToInt32(idProduto));
                    prod.Fornecedor = _servicoFornecedor.BuscarFornecedor(prod.FornecedorId);

                    Id.Text = prod.Id.ToString();
                    Nome.Text = prod.Nome;
                    Descricao.Text = prod.Descricao;
                    Codigo.Text = prod.Codigo;
                    Barras.Text = prod.CodigoDeBarras;
                    Preco.Text = prod.Preco.ToString();
                    Custo.Text = prod.Custo.ToString();
                    Estoque.Text = prod.Estoque.ToString();
                    Unidade.SelectedItem = prod.Unidade;
                    Categoria.SelectedItem = prod.Categoria;
                    Ativo.IsChecked = prod.Ativo;
                    Validade.Date = prod.DataValidade;
                    Forn.SelectedValue = prod.Fornecedor.Id;
                }
            }
            catch (Exception ex) { }
        }

        private void PreencherDadosProduto()
        {
            try
            {
                int novo = _servicoProduto.BuscarNovoProduto();

                Id.Text = novo.ToString();

            }
            catch (Exception ex) { }
        }

        private async void Botao_Click(object sender, RoutedEventArgs e)
        {
            Modelos.Produto prod = new Modelos.Produto();
            int error = 0;

            try
            {
                prod.Nome = Nome.Text;
                prod.Descricao = Descricao.Text;
                prod.Codigo = Codigo.Text;
                prod.CodigoDeBarras = Barras.Text;
                prod.Ativo = Ativo.IsChecked == true;
                prod.DataValidade = Validade.Date.Value.DateTime;


                if (!IsNumber(Preco.Text))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "Pre�o tem que ser um n�mero fracionado ou inteiro\n10,50");
                }
                if (!IsNumber(Custo.Text))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "Custo tem que ser um n�mero fracionado ou inteiro\n10,50");
                }
                if (!IsNumber(Estoque.Text))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "Estoque tem que ser um n�mero inteiro\n10");
                }
                if (Unidade.SelectedItem == null)
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "Unidade n�o selecionada.");
                }
                else
                {
                    prod.Unidade =  (Unidade)Unidade.SelectedValue;
                }
                if(Categoria.SelectedItem == null)
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "Categoria n�o selecionada.");
                }
                else
                {
                    prod.Categoria = (Categoria)Categoria.SelectedValue;
                }
                if(Forn.SelectedItem == null)
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "Fornecedor n�o selecionada.");
                }
                else
                {
                    prod.FornecedorId = _fornecedor.Id;
                }
                if(prod.Estoque < 0)
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "Produtos n�o podem ser criados com quantidades negativas.");
                }

                if (error == 0)
                {
                    prod.Preco = Convert.ToDouble(Preco.Text);
                    prod.Custo = Convert.ToDouble(Custo.Text);
                    prod.Estoque = Convert.ToInt32(Estoque.Text);

                    if (botao.Content.ToString() == "Adicionar")
                    {
                        prod.DataAtualizacao = DateTime.Now;
                        prod.DataCriacao = DateTime.Now;
                        error = _servicoProduto.AdicionarProduto(prod);
                        if(error == 1)
                        {
                            Frame.Navigate(typeof(Produtos));
                        }
                        else
                        {
                            throw new Exception("Erro ao adicionar Produto.");
                        }
                    }
                    else
                    {
                        prod.Id = Convert.ToInt32(Id.Text);
                        prod.DataAtualizacao = DateTime.Now;
                        error = _servicoProduto.AtualizarProduto(prod);
                        if (error == 1)
                        {
                            Frame.Navigate(typeof(Produtos));
                        }
                        else
                        {
                            throw new Exception("Erro ao atualizar Produto.");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message); // Para logar no console ou arquivo, se necess�rio

                // Exibe a notifica��o de erro
                await MostrarMensagemDeErroAsync("Erro", ex.Message);
            }
        }

        private async Task MostrarMensagemDeErroAsync(string titulo, string mensagem)
        {
            var dialog = new ContentDialog
            {
                Title = titulo,
                Content = mensagem,
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot // Certifique-se de passar o contexto correto
            };

            await dialog.ShowAsync();
        }

        private void SelecaoUnidade(object sender, SelectionChangedEventArgs e)
        {
            if (Unidade.SelectedItem != null)
            {
                _unidade = (Unidade)Unidade.SelectedItem;
            }
        }

        private void SelecaoCategoria(object sender, SelectionChangedEventArgs e)
        {
            if (Categoria.SelectedItem != null)
            {
                _categoria = (Categoria)Categoria.SelectedItem;
            }
        }

        private void SelecaoFornecedor(object sender, SelectionChangedEventArgs e)
        {
            if(Forn.SelectedItem != null)
            {
                _fornecedor = (Modelos.Fornecedor)Forn.SelectedItem;
            }
        }

        private static bool IsNumber(string value)
        {
            return double.TryParse(value, out _); // Verifica se � um n�mero inteiro
        }
    }
}
