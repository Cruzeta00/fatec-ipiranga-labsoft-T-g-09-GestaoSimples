﻿using GestaoSimples.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.IO;
using System.Windows;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoSimples
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                using (var dbContext = new ContextoGestaoSimples())
                {
                    dbContext.Database.EnsureCreated(); // Certifica-se de que o banco de dados foi criado

                    dbContext.VerificarEAdicionarUsuarioAdministrador();
                    dbContext.VerificaEAdicionarClienteAvulso();
                }

                var m_window = new GestaoSimples();
                Frame frame0 = new Frame();

                frame0.NavigationFailed += ErrodeNavegacao;
                frame0.Navigate(typeof(Login), args.Arguments);     

                m_window.Content = frame0;
                m_window.Activate();
            }
            catch (Exception ex)
            {
                File.WriteAllText("log.txt", ex.ToString());
            }
        }

        void ErrodeNavegacao(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Falha ao carregar a página " + e.SourcePageType.FullName);
        }

        private Window m_window;
    }
}
