namespace GestaoVeiculos.Models;

public class Marca
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;

    public override string ToString() => Nome;
}