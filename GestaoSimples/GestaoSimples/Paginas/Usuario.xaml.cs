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
using GestaoSimples.Modelos;
using GestaoSimples.Servicos;
using System.Diagnostics;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Usuario : Page
    {
        private readonly ServiceUsuario _servicoUsuario;
        Cargo _cargo;

        public Usuario()
        {
            this.InitializeComponent();

            _servicoUsuario = new ServiceUsuario();

            Cargo.ItemsSource = Enum.GetValues(typeof(Cargo));

            Cargo.SelectionChanged += SelecaoCargo;
        }

        private async void Botao_Click(object sender, RoutedEventArgs e)
        {
            Modelos.Usuario usu = new Modelos.Usuario();
            int error = 0;

            try
            {
                usu.Login = Login.Text;
                usu.Nome = Nome.Text;
                usu.Senha = Senha.Password;

                if(Cargo.SelectedItem == null)
                {
                    await ShowErrorNotificationAsync("Cargo não selecionado.");
                    error++;
                }
                else
                {
                    usu.Cargo = (Cargo)Cargo.SelectedValue;
                }
                if (error == 0)
                {
                    if (botao.Content.ToString() == "Adicionar")
                    {
                        error = _servicoUsuario.AdicionarUsuario(usu);
                        if (error == 1)
                        {
                            Frame.Navigate(typeof(Usuarios));
                        }
                        else
                        {
                            throw new Exception("Erro ao adicionar Cliente.");
                        }
                    }
                    else
                    {
                        usu.Id = Convert.ToInt32(Id.Text);
                        error = _servicoUsuario.AtualizarUsuario(usu);
                        if (error == 1)
                        {
                            Frame.Navigate(typeof(Usuarios));
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                botao.Content = "Adicionar";
                PreencherDadosUsuario();
            }
            else
            {
                botao.Content = "Atualizar";
                PreencherDadosUsuario(e.Parameter.ToString());
            }
        }

        private void PreencherDadosUsuario()
        {
            try
            {
                int novo = _servicoUsuario.BuscarNovoUsuario();

                Id.Text = novo.ToString();

            }
            catch (Exception ex) { }
        }

        private void PreencherDadosUsuario(string idUsuario)
        {
            try
            {
                if (String.IsNullOrEmpty(idUsuario))
                {
                    int novo = _servicoUsuario.BuscarNovoUsuario();

                    Id.Text = novo.ToString();
                }
                else
                {
                    Modelos.Usuario usu = _servicoUsuario.BuscarUsuario(Convert.ToInt32(idUsuario));

                    Id.Text = usu.Id.ToString();
                    Login.Text = usu.Login;
                    Nome.Text = usu.Nome;
                    Cargo.SelectedItem = usu.Cargo;
                    Senha.Password = usu.Senha;
                }
            }
            catch (Exception ex) { }
        }

        private void SelecaoCargo(object sender, SelectionChangedEventArgs e)
        {
            if(Cargo.SelectedItem != null)
            {
                _cargo = (Cargo)Cargo.SelectedItem;
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
