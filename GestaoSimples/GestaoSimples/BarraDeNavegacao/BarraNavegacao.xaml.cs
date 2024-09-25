using GestaoSimples.Janelas;
using GestaoSimples.Paginas;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
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

namespace GestaoSimples.BarraDeNavegacao
{
    public sealed partial class BarraNavegacao : UserControl
    {
        public BarraNavegacao()
        {
            this.InitializeComponent();
        }
        private Frame RetornaPai()
        {
            DependencyObject pai = VisualTreeHelper.GetParent(this.Parent);
            while(pai != null && !(pai is Frame)) 
            {
                pai = VisualTreeHelper.GetParent(pai);
            }
            return (Frame)pai;
        }

        private void ClickFornecedores(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Frame frame)
            {
                frame.Navigate(typeof(Fornecedores));
            }
            else
            {
                frame = RetornaPai();
                frame.Navigate(typeof(Fornecedores));
            }
        }

        private void ClickProdutos(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Frame frame)
            {
                frame.Navigate(typeof(Produtos));
            }
            else
            {
                frame = RetornaPai();
                frame.Navigate(typeof(Produtos));
            }
        }

        private void ClickVendas(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Frame frame)
            {
                frame.Navigate(typeof(Vendas));
            }
            else
            {
                frame = RetornaPai();
                frame.Navigate(typeof(Vendas));
            }
        }

        private void ClickClientes(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Frame frame)
            {
                frame.Navigate(typeof(Clientes));
            }
            else
            {
                frame = RetornaPai();
                frame.Navigate(typeof(Clientes));
            }
        }
        
        private void ClickCompras(object sender, RoutedEventArgs e)
        {

        }
        
        private void ClickUsuarios(object sender, RoutedEventArgs e)
        {

        }

        private void ClickVoltar(object sender, RoutedEventArgs e)
        {
            Frame frame = RetornaPai();
                
            if(frame.CanGoBack)
            {
                var nomePag = frame.Content.GetType().Name;
                if(nomePag == "Fornecedor" || nomePag == "Produto" || nomePag == "Venda" || nomePag == "Cliente")
                {
                    if (nomePag == "Fornecedor")
                        frame.Navigate(typeof(Fornecedores));
                    else if (nomePag == "Produto")
                        frame.Navigate(typeof(Produtos));
                    else if (nomePag == "Venda")
                        frame.Navigate(typeof(Vendas));
                    else if (nomePag == "Cliente")
                        frame.Navigate(typeof(Clientes));
                }
                else if (nomePag == "Fornecedores" || nomePag == "Produtos" || nomePag == "Vendas" || nomePag == "Clientes")
                {
                    // Para guias principais, voltar para o Menu Inicial
                    frame.Navigate(typeof(Menu));
                }
                else if (nomePag == "Menu") { }
            }
        }
    }
}
