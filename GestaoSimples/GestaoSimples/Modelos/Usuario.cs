using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome {  get; set; }
        public Cargo Cargo { get; set; }
    }

    public enum Cargo
    {
        ADMINISTRADOR = 0,
        VENDEDOR = 1
    }
}
