namespace GestaoVeiculos.Models;

public class Carro : Veiculo
{
    public override string Tipo => "CARRO";

    public override string ObterDescricao()
        => $"Carro {Modelo} ({Ano}) - Placa {Placa}";
}