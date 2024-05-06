using GestaoSimples.Janelas;
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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.Parent is Frame frame)
            {
                frame.Navigate(typeof(Fornecedores));
            }
            else
            {
                frame = RetornaPai();
                frame.Navigate(typeof(Fornecedores));
            }
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

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Frame frame)
            {
                frame.GoBack();
            }
            else
            {
                frame = RetornaPai();
                frame.GoBack();
            }
        }
    }
}
