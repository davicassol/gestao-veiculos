namespace GestaoVeiculos.Models;

public class Moto : Veiculo
{
    public override string Tipo => "MOTO";

    public override string ObterDescricao()
        => $"Moto {Modelo} ({Ano}) - Placa {Placa}";
}