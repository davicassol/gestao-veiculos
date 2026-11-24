using GestaoVeiculos.Data;
using GestaoVeiculos.Models;

namespace GestaoVeiculos.Business;

public class VeiculoService
{
    private const string MensagemMarcaInexistente = "A marca informada não existe.";

    private readonly IVeiculoRepository _repositorio;

    public VeiculoService(IVeiculoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public void Inserir(Veiculo veiculo)
    {
        try
        {
            _repositorio.Inserir(veiculo);
        }
        catch (Exception ex)
        {
            throw TradutorErroBanco.Traduzir(ex, MensagemMarcaInexistente);
        }
    }

    public void Alterar(Veiculo veiculo)
    {
        try
        {
            _repositorio.Alterar(veiculo);
        }
        catch (Exception ex)
        {
            throw TradutorErroBanco.Traduzir(ex, MensagemMarcaInexistente);
        }
    }

    public void Excluir(int codigo)
    {
        try
        {
            _repositorio.Excluir(codigo);
        }
        catch (Exception ex)
        {
            throw TradutorErroBanco.Traduzir(ex);
        }
    }

    public List<Veiculo> Listar()
    {
        try
        {
            return _repositorio.Listar();
        }
        catch (Exception ex)
        {
            throw TradutorErroBanco.Traduzir(ex);
        }
    }
}