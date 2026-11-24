using GestaoVeiculos.Data;
using GestaoVeiculos.Models;

namespace GestaoVeiculos.Business;

public class MarcaService
{
    private const string MensagemMarcaEmUso =
        "Não é possível excluir esta marca porque existem veículos vinculados a ela.";

    private readonly IMarcaRepository _repositorio;

    public MarcaService(IMarcaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public void Inserir(Marca marca)
    {
        try
        {
            _repositorio.Inserir(marca);
        }
        catch (Exception ex)
        {
            throw TradutorErroBanco.Traduzir(ex);
        }
    }

    public void Alterar(Marca marca)
    {
        try
        {
            _repositorio.Alterar(marca);
        }
        catch (Exception ex)
        {
            throw TradutorErroBanco.Traduzir(ex);
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
            throw TradutorErroBanco.Traduzir(ex, MensagemMarcaEmUso);
        }
    }

    public List<Marca> Listar()
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