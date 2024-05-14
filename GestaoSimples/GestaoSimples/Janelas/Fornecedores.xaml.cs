using GestaoSimples.Data;
using GestaoSimples.Modelos;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Janelas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Fornecedores : Page
    {
        public Fornecedores()
        {
            this.InitializeComponent();
        }

        private void botaoBuscar_Click(object sender, RoutedEventArgs e)
        {
            List<Fornecedor> fornecedores = new ();
            using (var contexto = new ContextoGestaoSimples())
            {
                fornecedores = contexto.Fornecedores.ToList();
            }
            fornecedorList.Source = fornecedores;
        }

        private void botaoDesativar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
