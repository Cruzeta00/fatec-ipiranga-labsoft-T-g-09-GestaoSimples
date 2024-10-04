using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Servicos
{
    public class SessaoUsuario
    {
        private static SessaoUsuario _instancia;
        public int UsuarioId{ get; set; }
        public string Login { get; set; }

        private SessaoUsuario() { }

        public static SessaoUsuario Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new SessaoUsuario();
                return _instancia;
            }
        }
    }
}
