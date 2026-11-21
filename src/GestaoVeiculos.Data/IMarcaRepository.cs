using GestaoVeiculos.Models;

namespace GestaoVeiculos.Data;

public interface IMarcaRepository
{
    void Inserir(Marca marca);
    void Alterar(Marca marca);
    void Excluir(int codigo);
    List<Marca> Listar();
    Marca? ObterPorCodigo(int codigo);
}