using GestaoVeiculos.Models;
using Npgsql;
using System.Reflection.Metadata.Ecma335;

namespace GestaoVeiculos.Data;

public class MarcaRepository : IMarcaRepository
{
    private readonly ConexaoBanco _conexao;

    public MarcaRepository(ConexaoBanco conexao)
    {
        _conexao = conexao;
    }

    public void Inserir(Marca marca)
    {
        const string sql = "INSERT INTO marca (nome) VALUES (@nome) RETURNING codigo;";


        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome", marca.Nome);

        marca.Codigo = (int)cmd.ExecuteScalar()!;
             
    }

    public void Alterar(Marca marca)
    {
        const string sql = "UPDATE marca SET nome = @nome WHERE codigo = @codigo";

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome", marca.Nome);
        cmd.Parameters.AddWithValue("@codigo", marca.Codigo);
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int Codigo)
    {
        const string sql = "DELETE FROM marca WHERE codigo = @codigo";

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("codigo", Codigo);
        cmd.ExecuteNonQuery();

    }

    public List<Marca> Listar()
    {
        const string sql = "SELECT codigo, nome FROM marca ORDER BY nome";
        var marcas = new List<Marca>();

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            marcas.Add(new Marca
            {
                Codigo = reader.GetInt32(0),
                Nome = reader.GetString(1)
            });
        }

        return marcas;
    }

    public Marca? ObterPorCodigo(int codigo)
    {
        const string sql = "SELECT codigo, nome FROM marca WHERE codigo = @codigo;";

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@codigo", codigo);
        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
            return null;
        
        return new Marca
        {
            Codigo = reader.GetInt32(0),
            Nome = reader.GetString(1)
        };
    }

   
}
