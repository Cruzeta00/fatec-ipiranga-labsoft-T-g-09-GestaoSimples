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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
        private DateTimeOffset? selectedDate;

        public Fornecedor()
        {
            this.InitializeComponent();

            _servicoFornecedor = new ServiceFornecedor();
            DataCad.DateChanged += DatePicker_DateChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                botao.Content = "Adicionar";
                PreencherDadosFornecedor(string.Empty);
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
                    DataCad.Date = forn.DataCadastro;
                    Class.Text = forn.Classificacao.ToString();
                }
            }
            catch(Exception ex) { }
        }

        private async void botao_Click(object sender, RoutedEventArgs e)
        {
            Modelos.Fornecedor  forn = new Modelos.Fornecedor();

            try
            {
                forn.Nome = Nome.Text;
                forn.CNPJ = CNPJ.Text;
                forn.Telefone = Telefone.Text;
                forn.EMail = EMail.Text;
                if (Ativo.IsChecked == true) forn.Ativo = true;
                else forn.Ativo = false;
                forn.Observacoes = Obs.Text;
                forn.DataCadastro = DateTime.Parse(selectedDate.ToString());
                Enum.TryParse(Class.Text, out Classificacao valor);
                forn.Classificacao = valor;

            
                if (botao.Content.ToString() == "Adicionar")
                {
                    _servicoFornecedor.AdicionarFornecedor(forn);
                }
                else _servicoFornecedor.AtualizarFornecedor(forn);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message); // Para logar no console ou arquivo, se necessário

                // Exibe a notificação de erro
                await ShowErrorNotificationAsync(ex.Message);
                if(ex.InnerException != null)
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

        private void DatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (DataCad.Date != null)
            {
                selectedDate = DataCad.Date.Value;
                
            }
        }
    }
}
