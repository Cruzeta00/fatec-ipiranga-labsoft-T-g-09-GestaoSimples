using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using GestaoSimples.Servicos;
using GestaoSimples.Modelos;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples.Paginas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Usuarios : Page
    {
        private readonly ServiceUsuario _servicoUsuario;
        private List<Modelos.Usuario> listaUsuarios {  get; set; }
        public Usuarios()
        {
            this.InitializeComponent();

            _servicoUsuario = new ServiceUsuario();

            listaUsuarios = _servicoUsuario.BuscarUsuarios();
            UsuariosListView.ItemsSource = listaUsuarios;

            NenhumUsuario.Visibility = Visibility.Collapsed;

            if (UsuariosListView == null)
            {
                NenhumUsuario.Visibility = Visibility.Visible;
            }
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            string textoBuscado = Textobusca.Text.ToLower();

            if (listaUsuarios != null)
            {
                var listaFiltrada = listaUsuarios.Where(f => f.Login.ToLower().Contains(textoBuscado) ||
                                                       f.Nome.ToLower().Contains(textoBuscado) ||
                                                       f.Cargo.ToString().ToLower().Contains(textoBuscado)).ToList();

                UsuariosListView.ItemsSource = listaFiltrada;
            }
        }

        private void BotaoCriar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Usuario));
        }

        private void BotaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            if (UsuariosListView.SelectedItem != null)
            {
                var usuario = UsuariosListView.SelectedItem as Modelos.Usuario;

                Frame.Navigate(typeof(Usuario), usuario.Id);
            }
        }
    }
}
