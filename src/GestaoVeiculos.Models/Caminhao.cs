namespace GestaoVeiculos.Models;

public class Caminhao : Veiculo
{
    public override string Tipo => "CAMINHAO";

    public override string ObterDescricao()
        => $"Caminhao {Modelo} ({Ano}) - Placa {Placa}";
}