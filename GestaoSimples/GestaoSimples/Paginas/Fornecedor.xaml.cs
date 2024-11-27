using GestaoSimples.Janelas;
using GestaoSimples.Modelos;
using GestaoSimples.Servicos;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Fornecedor : Page
    {
        private readonly ServiceFornecedor _servicoFornecedor;
        Classificacao? selectedStatus;
        public Fornecedor()
        {
            this.InitializeComponent();

            _servicoFornecedor = new ServiceFornecedor();
            Class.ItemsSource = Enum.GetValues(typeof(Classificacao));

            Class.SelectionChanged += StatusComboBox_SelectionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                botao.Content = "Adicionar";
                PreencherDadosFornecedor();
            }
            else
            {
                botao.Content = "Atualizar";
                PreencherDadosFornecedor(e.Parameter.ToString());
            }
        }

        private void PreencherDadosFornecedor(string idFornecedor)
        {
            try
            {
                if (String.IsNullOrEmpty(idFornecedor))
                {
                    int novo = _servicoFornecedor.BuscarNovoFornecedor();

                    Id.Text = novo.ToString();
                }
                else
                {
                    Modelos.Fornecedor forn = _servicoFornecedor.BuscarFornecedor(Convert.ToInt32(idFornecedor));

                    Id.Text = forn.Id.ToString();
                    Nome.Text = forn.Nome;
                    CNPJ.Text = forn.CNPJ;
                    Telefone.Text = forn.Telefone;
                    EMail.Text = forn.EMail;
                    Ativo.IsChecked = forn.Ativo;
                    Obs.Text = forn.Observacoes;
                    Class.SelectedItem = forn.Classificacao;
                }
            }
            catch(Exception ex) { }
        }

        private void PreencherDadosFornecedor()
        {
            try
            {
                int novo = _servicoFornecedor.BuscarNovoFornecedor();

                Id.Text = novo.ToString();
                
            }
            catch (Exception ex) { }
        }

        private async void botao_Click(object sender, RoutedEventArgs e)
        {
            Modelos.Fornecedor  forn = new Modelos.Fornecedor();
            int error = 0;

            try
            {
                forn.Nome = Nome.Text;
                forn.CNPJ = CNPJ.Text;
                forn.Telefone = Telefone.Text;
                forn.EMail = EMail.Text;
                forn.Ativo = Ativo.IsChecked == true;
                forn.Observacoes = Obs.Text;
                forn.DataCadastro = DateTime.Today;

                if (!ValidarEmail(forn.EMail))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "O email não está em formato padrão. \nXXX@XXX.COM");
                }
                if (!ValidarTelefone(forn.Telefone))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "O telefone não está em formato padrão.\n(XX) XXXXX-XXXX");
                }
                if (!ValidarCNPJ(forn.CNPJ))
                {
                    error++;
                    await MostrarMensagemDeErroAsync("Erro", "O CNPJ precisa ser válido");
                }
                if (Class.SelectedValue == null)
                {
                    await MostrarMensagemDeErroAsync("Erro", "Classificação não selecionada.");
                    error++;
                }
                else
                {
                    forn.Classificacao = (Classificacao)Class.SelectedValue;
                }

                if(error == 0)
                {
                    if (botao.Content.ToString() == "Adicionar")
                    {
                        error = _servicoFornecedor.AdicionarFornecedor(forn);
                        if (error == 1)
                        {
                            Frame.Navigate(typeof(Fornecedores));
                        }
                        else
                        {
                            throw new Exception("Erro ao adicionar o Parceiro");
                        }
                    }
                    else 
                    {
                        forn.Id = Convert.ToInt32(Id.Text);
                        error = _servicoFornecedor.AtualizarFornecedor(forn);
                        if (error == 1)
                        {
                            Frame.Navigate(typeof(Fornecedores));
                        }
                        else
                        {
                            throw new Exception("Erro ao atualizar o Parceiro");
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

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Class.SelectedItem != null)
            {
                selectedStatus = (Classificacao)Class.SelectedItem;
            }
        }

        private bool ValidarTelefone(string Telefone)
        {
            // Expressão regular para validar o telefone no formato (XX) XXXXX-XXXX
            string padraoTelefone = @"^\(\d{2}\) \d{5}-\d{4}$";

            return Regex.IsMatch(Telefone, padraoTelefone);
        }

        private bool ValidarEmail(string Email)
        {
            // Expressão regular para validar o telefone no formato (XX) XXXXX-XXXX
            string padraoEmail= @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(Email, padraoEmail);
        }

        private bool ValidarCNPJ(string CNPJ)
        {
            // Expressão regular para validar o telefone no formato (XX) XXXXX-XXXX
            string padraoCNPJ= @"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$";

            return Regex.IsMatch(CNPJ, padraoCNPJ);
        }
    }
}
