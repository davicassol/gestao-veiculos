namespace GestaoVeiculos.Business;

public class RegraNegocioException : Exception
{
    public RegraNegocioException(string mensagem) : base(mensagem)
    {
    }

    public RegraNegocioException(string mensagem, Exception excecaoOriginal)
        : base(mensagem, excecaoOriginal)
    {
    }
}