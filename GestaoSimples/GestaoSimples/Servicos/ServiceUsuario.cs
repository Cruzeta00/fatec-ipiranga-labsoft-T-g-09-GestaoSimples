using GestaoSimples.Data;
using GestaoSimples.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Servicos
{
    public class ServiceUsuario
    {
        public List<Usuario> BuscarUsuarios()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Usuarios.ToList();
            }
        }

        public Usuario BuscarUsuario(int Id)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Usuarios.FirstOrDefault(x => x.Id == Id);
            }
        }

        public Usuario BuscarUsuarioPorLogin(string login)
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Usuarios.FirstOrDefault(x => x.Login == login);
            }
        }

        public int BuscarNovoUsuario()
        {
            using (var contexto = new ContextoGestaoSimples())
            {
                return contexto.Usuarios.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault() + 1;
            }
        }

        public int AdicionarUsuario(Usuario usu)
        {
            int retorno = -1;
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Usuarios.Add(usu);
                retorno = contexto.SaveChanges();
            }
            return retorno;
        }

        public int AtualizarUsuario(Usuario usu)
        {
            int retorno = -1;
            using (var contexto = new ContextoGestaoSimples())
            {
                contexto.Usuarios.Update(usu);
                retorno = contexto.SaveChanges();
            }
            return retorno;
        }
    }
}
