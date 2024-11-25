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
    public sealed partial class ItensCompra : Page
    {
        private Modelos.Compra compraAtual;
        private readonly ServiceCompra _servicoCompra;

        private List<Modelos.ItemCompra> listaItensCompra {  get; set; }
        public ItensCompra()
        {
            this.InitializeComponent();

            _servicoCompra = new ServiceCompra();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            compraAtual = (Modelos.Compra)e.Parameter;

            listaItensCompra = _servicoCompra.BuscarItensCompra(compraAtual.Id);
            ItensComprasListView.ItemsSource = listaItensCompra;
        }
    }
}
