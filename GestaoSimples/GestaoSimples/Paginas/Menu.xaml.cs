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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Menu : Page
    {
        private readonly ServiceUsuario _servicoUsuario;
        public Menu()
        {
            this.InitializeComponent();

            _servicoUsuario = new ServiceUsuario();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Usuario usuLogado;

            usuLogado = _servicoUsuario.BuscarUsuarioPorLogin(e.Parameter.ToString());
            if (usuLogado != null && e.Parameter.ToString() == usuLogado.Login)
            {
                
            }
            else
            {
                ContentDialog mudarSenha = new ContentDialog()
                {
                    Title = "Mudança de Senha",
                    Content = "Recomendamos acessar a guia de Usuários e alterar a Senha ou Nome cadastrado no sistema para manter-lo seguro.",
                    CloseButtonText = "OK",
                };
                mudarSenha.XamlRoot = Frame.XamlRoot;
                await mudarSenha.ShowAsync();
            }
        }
    }
}
