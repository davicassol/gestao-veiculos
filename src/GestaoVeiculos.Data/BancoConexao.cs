using Npgsql;

namespace GestaoVeiculos.Data;

public class ConexaoBanco
{
    private readonly string _connectionString;

    public ConexaoBanco(string connectionString)
    {
        _connectionString = connectionString;
    }

    public NpgsqlConnection CriarConexao()
    {
        return new NpgsqlConnection(_connectionString);
    }
}