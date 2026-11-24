namespace GestaoVeiculos.Business;

public static class LogErros
{
    private static readonly object _trava = new object();

    public static void Registrar(Exception erro, string? mensagemUsuario = null)
    {
        try
        {
            string pasta = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(pasta);

            string arquivo = Path.Combine(pasta, $"erros-{DateTime.Now:yyyy-MM-dd}.log");
            string texto = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]{Environment.NewLine}";

            if (mensagemUsuario != null)
            {
                texto += $"Mensagem exibida ao usuário: {mensagemUsuario}{Environment.NewLine}";
            }

            texto += $"{erro}{Environment.NewLine}";
            texto += new string('-', 80) + Environment.NewLine;

            lock (_trava)
            {
                File.AppendAllText(arquivo, texto);
            }
        }
        catch
        {

        }
    }
}