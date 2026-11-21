using GestaoVeiculos.Models;

namespace GestaoVeiculos.Data;

public interface IVeiculoRepository
{
    void Inserir(Veiculo veiculo);
    void Alterar(Veiculo veiculo);
    void Excluir(int codigo);
    List<Veiculo> Listar();
    Veiculo? ObterPorCodigo(int codigo);
}