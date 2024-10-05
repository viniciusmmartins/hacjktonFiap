namespace Hackaton.Application.Extensions
{
    public static class StringExtensions
    {
        public static string CleanCpf(this string cpf)
        {
            return cpf.Replace("-", "").Replace(".", "");
        }
    }
}
