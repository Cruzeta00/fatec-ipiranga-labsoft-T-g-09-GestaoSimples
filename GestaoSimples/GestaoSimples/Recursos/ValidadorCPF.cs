using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoSimples.Recursos
{
    public class ValidadorCPF
    {
        public static bool Validar(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // O CPF deve ter 11 dígitos
            if (cpf.Length != 11)
                return false;

            // CPFs com todos os dígitos iguais são inválidos
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Cálculo dos dígitos verificadores
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string cpfBase = cpf.Substring(0, 9);
            string digitos = cpf.Substring(9, 2);

            int soma1 = cpfBase
                .Select((c, i) => (c - '0') * multiplicadores1[i])
                .Sum();

            int resto1 = soma1 % 11;
            int digito1 = resto1 < 2 ? 0 : 11 - resto1;

            int soma2 = (cpfBase + digito1)
                .Select((c, i) => (c - '0') * multiplicadores2[i])
                .Sum();

            int resto2 = soma2 % 11;
            int digito2 = resto2 < 2 ? 0 : 11 - resto2;

            return digitos == $"{digito1}{digito2}";
        }
    }
}
