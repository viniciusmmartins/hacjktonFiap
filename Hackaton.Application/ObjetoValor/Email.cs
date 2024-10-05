namespace Hackaton.Application.ObjetoValor
{
    public class Email
    {
        public string Valor { get; set; }
        public Email(string email)
        {
            if(!email.Contains("@"))
            {
                throw new ArgumentException("Email invalido");
            }

            Valor = email;
        }
    }
}
