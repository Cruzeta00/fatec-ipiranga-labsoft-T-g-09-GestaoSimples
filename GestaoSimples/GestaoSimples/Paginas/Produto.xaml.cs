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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static Vanara.PInvoke.Kernel32.COPYFILE2_MESSAGE;
using static Vanara.PInvoke.User32;

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

        public Produto()
        {
            this.InitializeComponent();

            _servicoProduto = new ServiceProduto();
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

                    Id.Text = prod.Id.ToString();
                    Nome.Text = prod.Nome;
                    Descricao.Text = prod.Descricao;
                    Codigo.Text = prod.Codigo;
                    Barras.Text = prod.CodigoDeBarras;
                    Preco.Text = prod.Preco.ToString();
                    Custo.Text = prod.Custo.ToString();
                    Estoque.SelectedItem = prod.Estoque;
                    Unidade.SelectedItem = prod.Unidade;
                    Categoria.SelectedItem = prod.Categoria;
                    Ativo.IsChecked = prod.Ativo;
                    Validade.Date = prod.DataValidade;
                    Atualizacao.Date = prod.DataAtualizacao;
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

        private async void botao_Click(object sender, RoutedEventArgs e)
        {
            Modelos.Produto prod = new Modelos.Produto();
            int error = 0;

            try
            {
                prod.Nome = Nome.Text;
                prod.Descricao = Descricao.Text;
                prod.Codigo = Codigo.Text;
                prod.CodigoDeBarras = Barras.Text;
                prod.Preco = Convert.ToDouble(Preco.Text);
                prod.Custo = Convert.ToDouble(Custo.Text);
                prod.Estoque = Convert.ToInt32(Estoque.Text);
                prod.Unidade = Unidade.Text;
                prod.Categoria = Categoria.Text;
                prod.Ativo = Ativo.IsChecked == true;
                prod.DataValidade = Validade.Date.Value.DateTime;
                prod.DataAtualizacao = Atualizacao.Date.Value.DateTime;

                if (error == 0)
                {
                    if (botao.Content.ToString() == "Adicionar")
                    {
                        _servicoProduto.AdicionarProduto(prod);
                    }
                    else
                    {
                        prod.Id = Convert.ToInt32(Id.Text);
                        _servicoProduto.AtualizarProduto(prod);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message); // Para logar no console ou arquivo, se necessário

                // Exibe a notificação de erro
                await ShowErrorNotificationAsync(ex.Message);
                if (ex.InnerException != null)
                    _ = ShowErrorNotificationAsync(ex.InnerException.ToString());
            }
        }

        private async Task ShowErrorNotificationAsync(string message)
        {
            ErrorTextBlock.Text = message;  // Define o texto do erro
            ErrorNotification.Visibility = Visibility.Visible;  // Torna a notificação visível

            // Aguarda 3 segundos antes de ocultar
            await Task.Delay(3000);
            ErrorNotification.Visibility = Visibility.Collapsed;  // Oculta a notificação
        }
    }
}
