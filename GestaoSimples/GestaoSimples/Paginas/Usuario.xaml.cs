using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using GestaoSimples.Modelos;
using GestaoSimples.Servicos;
using System.Diagnostics;
using System.Threading.Tasks;
using GestaoSimples.Recursos;

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
                usu.CPF = CPF.Text;

                if(Cargo.SelectedItem == null)
                {
                    await MostrarMensagemDeErroAsync("Erro", "Cargo n�o selecionado.");
                    error++;
                }
                else
                {
                    usu.Cargo = (Cargo)Cargo.SelectedValue;
                }
                if (usu.Cargo != Modelos.Cargo.ADMINISTRADOR && !ValidadorSenha.ValidarSenha(usu.Senha))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "A senha deve conter pelo menos 1 n�mero, 1 simbolo, 1 letra mai�scula e 1 letra min�scula.");
                }
                if (usu.Cargo != Modelos.Cargo.ADMINISTRADOR && !ValidadorCPF.Validar(usu.CPF))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "O CPF � inv�lido. � necess�rio informar um CPF v�lido.");
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
                Debug.WriteLine(ex.Message); // Para logar no console ou arquivo, se necess�rio

                // Exibe a notifica��o de erro
                await MostrarMensagemDeErroAsync("Erro",ex.Message);
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
                    CPF.Text = usu.CPF;
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
    }
}
