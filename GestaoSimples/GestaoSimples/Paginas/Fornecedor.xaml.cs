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
using System.IO;
using System.Linq;
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
    public sealed partial class Fornecedor : Page
    {
        private readonly ServiceFornecedor _servicoFornecedor;

        public Fornecedor()
        {
            this.InitializeComponent();

            _servicoFornecedor = new ServiceFornecedor();
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
                    DataCad.Text = forn.DataCadastro.ToString();
                    Class.Text = forn.Classificacao.ToString();
                }
            }
            catch(Exception ex) { }
        }

        private void botao_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
