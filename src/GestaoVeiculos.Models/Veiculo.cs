namespace GestaoVeiculos.Models;

public abstract class Veiculo
{
    public int Codigo { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public Marca Marca { get; set; } = new Marca();

    public abstract string Tipo { get; }

    // Método abstrato para obter a descrição do veículo
    public abstract string ObterDescricao();
}