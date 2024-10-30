using GestaoSimples.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private async void HyperlinkButton_ClickLogin(object sender, RoutedEventArgs e)
        {
            string usuario = this.usuario.Text;
            string senha = this.senha.Password;

            using(var contexto = new ContextoGestaoSimples())
            {
                var Usuario = contexto.Usuarios.FirstOrDefault(u => u.Login == usuario && u.Senha == senha);
                if (Usuario != null)
                {
                    Frame.Navigate(typeof(Menu), this.usuario.Text, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                }
                else
                {
                    ContentDialog msgErro = new ContentDialog
                    {
                        Title = "Erro de Conex�o",
                        Content = "Usu�rio ou Senha inv�lido.",
                        CloseButtonText = "OK",
                    };
                    msgErro.XamlRoot = botaoLogin.XamlRoot;
                    await msgErro.ShowAsync();
                }
            }
            
        }

        private async void HyperlinkButton_ClickEsqueci(object sender, RoutedEventArgs e)
        {
            TextBox nome = new TextBox
            {
                PlaceholderText = "Digite o CPF cadastrado no seu perfil."
            };

            ContentDialog esqueciSenha = new ContentDialog
            {
                Title = "Esqueci a Senha",
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock {Text = "Para recuperar a senha, digite o CPF cadastrado no seu usu�rio."},
                        nome
                    }
                },
                CloseButtonText = "OK",
                
            };
            esqueciSenha.XamlRoot = botaoEsqueciSenha.XamlRoot;
            await esqueciSenha.ShowAsync();

            if(nome.Text != null)
            {
                using (var contexto = new ContextoGestaoSimples())
                {
                    var Usuario = contexto.Usuarios.FirstOrDefault(u => u.CPF == nome.Text);
                    if (Usuario != null)
                    {
                        Frame.Navigate(typeof(Menu), this.usuario.Text, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                    }
                    else
                    {
                        ContentDialog msgErro = new ContentDialog
                        {
                            Title = "Erro de Conex�o",
                            Content = "CPF de usu�rio inv�lido. Entre em contato com um ADMINISTRADOR para acessar o sistema.",
                            CloseButtonText = "OK",
                        };
                        msgErro.XamlRoot = botaoLogin.XamlRoot;
                        await msgErro.ShowAsync();
                    }
                }
            }
        }
    }
}
