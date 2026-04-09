
using GestaoSimples.Janelas;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Relatorios : Page
{
    public Relatorios()
    {
        InitializeComponent();
    }

    private Frame RetornaPai()
    {
        DependencyObject pai = VisualTreeHelper.GetParent(this.Parent);
        while (pai != null && !(pai is Frame))
        {
            pai = VisualTreeHelper.GetParent(pai);
        }
        return (Frame)pai;
    }

    private void ClickRelatoriosCV(object sender, RoutedEventArgs e)
    {
        
        if (this.Parent is Frame frame)
        {
            frame.Navigate(typeof(RelatorioCV));
        }
        else
        {
            frame = RetornaPai();
            frame.Navigate(typeof(RelatorioCV));
        }
    }

    private void ClickRelatoriosItens(object sender, RoutedEventArgs e)
    {
        if (this.Parent is Frame frame)
        {
            frame.Navigate(typeof(RelatorioItens));
        }
        else
        {
            frame = RetornaPai();
            frame.Navigate(typeof(RelatorioItens));
        }
    }
}
