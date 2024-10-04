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
    public sealed partial class Cliente : Page
    {
        private readonly ServiceCliente _servicoCliente;
        public Cliente()
        {
            this.InitializeComponent();

            _servicoCliente = new ServiceCliente();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                botao.Content = "Adicionar";
                PreencherDadosCliente();
            }
            else
            {
                botao.Content = "Atualizar";
                PreencherDadosCliente(e.Parameter.ToString());
            }
        }

        private void PreencherDadosCliente(string idFornecedor)
        {
            try
            {
                if (String.IsNullOrEmpty(idFornecedor))
                {
                    int novo = _servicoCliente.BuscarNovoCliente();

                    Id.Text = novo.ToString();
                }
                else
                {
                    Modelos.Cliente cliente = _servicoCliente.BuscarCliente(Convert.ToInt32(idFornecedor));

                    Id.Text = cliente.Id.ToString();
                    Nome.Text = cliente.Nome;
                    CPF.Text = cliente.CPF;
                    Telefone.Text = cliente.Telefone;
                }
            }
            catch (Exception ex) { }
        }

        private void PreencherDadosCliente()
        {
            try
            {
                int novo = _servicoCliente.BuscarNovoCliente();

                Id.Text = novo.ToString();

            }
            catch (Exception ex) { }
        }

        private async void Botao_Click(object sender, RoutedEventArgs e)
        {
            Modelos.Cliente cliente = new Modelos.Cliente();
            int error = 0;

            try
            {
                cliente.Nome = Nome.Text;
                cliente.CPF = CPF.Text;
                cliente.Telefone = Telefone.Text;
                

                if (error == 0)
                {
                    if (botao.Content.ToString() == "Adicionar")
                    {
                        error = _servicoCliente.AdicionarCliente(cliente);
                        if(error == 1)
                        {
                            Frame.Navigate(typeof(Clientes));
                        }
                        else
                        {
                            throw new Exception("Erro ao adicionar Cliente.");
                        }
                    }
                    else
                    {
                        cliente.Id = Convert.ToInt32(Id.Text);
                        error = _servicoCliente.AtualizarCliente(cliente);
                        if(error == 1)
                        {
                            Frame.Navigate(typeof(Clientes));
                        }
                        else
                        {
                            throw new Exception("Erro ao atualizar Cliente.");
                        }
                    }
                }
            }
            catch (Exception ex)
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
