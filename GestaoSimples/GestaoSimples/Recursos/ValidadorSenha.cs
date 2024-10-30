using System.Text.RegularExpressions;

namespace GestaoSimples.Recursos
{
    public class ValidadorSenha
    {
        public static bool ValidarSenha(string senha)
        {
            // Validação: pelo menos 1 letra minúscula, 1 letra maiúscula, 1 número e 1 símbolo
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$");

            return regex.IsMatch(senha);
        }
    }
}
