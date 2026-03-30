using GestaoSimples.Recursos;
using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
        private bool _formatandoCpf = false;
        private bool _formatandoTelefone = false;
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

                if (!ValidadorCPF.Validar(cliente.CPF))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("O CPF é inválido", "É necessário informar um CPF válido.");
                }

                if (!ValidarTelefone(cliente.Telefone))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "O telefone não está em formato padrão.\n(XX) XXXXX-XXXX");
                }

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

        private bool ValidarTelefone(string Telefone)
        {
            // Expressão regular para validar o telefone no formato (XX) XXXXX-XXXX
            string padraoTelefone = @"^\(\d{2}\) \d{5}-\d{4}$";

            return Regex.IsMatch(Telefone, padraoTelefone);
        }

        private void Aplicando_CPF(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (_formatandoCpf)
                return;

            _formatandoCpf = true;

            string numeros = new string(sender.Text.Where(char.IsDigit).ToArray());

            if (numeros.Length > 11)
                numeros = numeros.Substring(0, 11);

            if (numeros.Length >= 4)
                numeros = numeros.Insert(3, ".");

            if (numeros.Length >= 8)
                numeros = numeros.Insert(7, ".");

            if (numeros.Length >= 12)
                numeros = numeros.Insert(11, "-");

            sender.Text = numeros;
            sender.SelectionStart = sender.Text.Length;

            _formatandoCpf = false;
        }

        private void Aplicando_Telefone(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (_formatandoTelefone)
                return;

            _formatandoTelefone = true;

            string numeros = new string(sender.Text.Where(char.IsDigit).ToArray());

            if (numeros.Length > 11)
                numeros = numeros.Substring(0, 11);

            if (numeros.Length >= 2)
                numeros = $"({numeros.Substring(0, 2)}) {numeros.Substring(2)}";

            if (numeros.Length >= 10)
                numeros = numeros.Insert(10, "-");

            sender.Text = numeros;
            sender.SelectionStart = sender.Text.Length;

            _formatandoTelefone = false;
        }
    }
}
