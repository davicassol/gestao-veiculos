using GestaoVeiculos.Models;
using Npgsql;

namespace GestaoVeiculos.Data;

public class VeiculoRepository : IVeiculoRepository
{
    private const string SelectBase = @"
        SELECT v.codigo, v.tipo, v.placa, v.modelo, v.ano,
                m.codigo AS marca_codigo, m.nome AS marca_nome
        FROM veiculo v
        INNER JOIN marca m ON m.codigo = v.marca_codigo";

    private readonly ConexaoBanco _conexao;

    public VeiculoRepository(ConexaoBanco conexao)
    {
        _conexao = conexao;
    }

    public void Inserir(Veiculo veiculo)
    {
        const string sql = @"
            INSERT INTO veiculo (tipo, placa, modelo, ano, marca_codigo)
            VALUES (@tipo, @placa, @modelo, @ano, @marca)
            RETURNING codigo;";

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        AdicionarParametros(cmd, veiculo);

        veiculo.Codigo = (int)cmd.ExecuteScalar()!;
    }
    
    public void Alterar(Veiculo veiculo)
    {
        const string sql = @"
            UPDATE veiculo
            SET tipo = @tipo, placa = @placa, modelo = @modelo,
                ano = @ano, marca_codigo = @marca
            WHERE codigo = @codigo";

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        AdicionarParametros(cmd, veiculo);
        cmd.Parameters.AddWithValue("@codigo", veiculo.Codigo);
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int codigo)
    {
        const string sql = "DELETE FROM veiculo WHERE codigo = @codigo";

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@codigo", codigo);
        cmd.ExecuteNonQuery();
    }

    public List<Veiculo> Listar()
    {
        string sql = SelectBase + " ORDER BY v.codigo;";
        var veiculos = new List<Veiculo>();

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            veiculos.Add(Mapear(reader));
        }

        return veiculos;
    }

    public Veiculo? ObterPorCodigo(int codigo)
    {
        string sql = SelectBase + " WHERE v.codigo = @codigo;";

        using var conn = _conexao.CriarConexao();
        conn.Open();
        using var cmd=  new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@codigo", codigo);
        using var reader = cmd.ExecuteReader();

        return reader.Read() ? Mapear(reader) : null;
    }

    private static void AdicionarParametros(NpgsqlCommand cmd, Veiculo veiculo)
    {
        cmd.Parameters.AddWithValue("@tipo", veiculo.Tipo);
        cmd.Parameters.AddWithValue("@placa", veiculo.Placa);
        cmd.Parameters.AddWithValue("@modelo", veiculo.Modelo);
        cmd.Parameters.AddWithValue("@ano", veiculo.Ano);
        cmd.Parameters.AddWithValue("@marca", veiculo.Marca.Codigo);
    }

    private static Veiculo Mapear(NpgsqlDataReader reader)
    {
        Veiculo veiculo = CriarPorTipo(reader.GetString(1));

        veiculo.Codigo = reader.GetInt32(0);
        veiculo.Placa = reader.GetString(2);
        veiculo.Modelo = reader.GetString(3);
        veiculo.Ano = reader.GetInt32(4);
        veiculo.Marca = new Marca
        {
            Codigo = reader.GetInt32(5),
            Nome = reader.GetString(6)
        };

        return veiculo;
    }

    private static Veiculo CriarPorTipo(string tipo)
    {
        return tipo switch
        {
            "CARRO" => new Carro(),
            "MOTO" => new Moto(),
            "CAMINHAO" => new Caminhao(),
            _ => throw new ArgumentException($"Tipo de veículo inválido: {tipo}")
        };
    }
}