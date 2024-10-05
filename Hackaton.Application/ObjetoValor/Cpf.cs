using Hackaton.Application.Extensions;

namespace Hackaton.Application.ObjetoValor
{
    public class Cpf
    {
        public string Valor { get; set; }
        public Cpf(string cpf)
        {
            if(cpf.CleanCpf().Length != 11)
            {
                throw new ArgumentException("Cpf invalido");
            }

            Valor = cpf.CleanCpf();
        }
    }
}
