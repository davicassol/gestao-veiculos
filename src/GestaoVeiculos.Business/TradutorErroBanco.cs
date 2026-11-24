using Npgsql;

namespace GestaoVeiculos.Business;

public static class TradutorErroBanco
{
    //método para traduzir o erro e registra no log
    public static RegraNegocioException Traduzir(
        Exception erro,
        string? mensagemChaveEstrangeira = null)
    {
        RegraNegocioException traduzida = CriarExcecao(erro, mensagemChaveEstrangeira);
        LogErros.Registrar(erro, traduzida.Message);

        return traduzida;
    }

    private static RegraNegocioException CriarExcecao(
        Exception erro,
        string? mensagemChaveEstrangeira)
    {
        if (erro is PostgresException pg)
        {
            string mensagem;

            switch (pg.SqlState)
            {
                //valor duplicado
                case "23505":
                    mensagem = TraduzirErroDuplicidade(pg.ConstraintName);
                    break;

                //chave estrangeira
                case "23503":
                    mensagem = mensagemChaveEstrangeira
                        ?? "A operação não é permitida porque o registro está relacionado a outros dados.";
                    break;

                //campo obrigatório nulo
                case "23502":
                    mensagem = "Preencha todos os campos obrigatórios.";
                    break;

                //regra violada
                case "23514":
                    mensagem = TraduzirErroRegra(pg);
                    break;

                //texto maior que o limite da coluna
                case "22001":
                    mensagem = "Um dos campos excede o tamanho permitido. A placa deve ter 7 caracteres.";
                    break;

                default:
                    mensagem = "Ocorreu um erro ao acessar o banco de dados. Tente novamente.";
                    break;
            }

            return new RegraNegocioException(mensagem, erro);
        }

        //falha de comunicação com o banco
        if (erro is NpgsqlException)
        {
            return new RegraNegocioException(
                "Não foi possível conectar ao banco de dados. Verifique a conexão e tente novamente.",
                erro);
        }

        return new RegraNegocioException(
            "Ocorreu um erro inesperado. Tente novamente.",
            erro);
    }

    private static string TraduzirErroDuplicidade(string? nomeConstraint)
    {
        if (nomeConstraint == "uq_marca_nome")
        {
            return "Já existe uma marca com este nome.";
        }
        else if (nomeConstraint == "uq_veiculo_placa")
        {
            return "Já existe um veículo com esta placa.";
        }
        else
        {
            return "Já existe um registro com estes dados.";
        }
    }

    private static string TraduzirErroRegra(PostgresException pg)
    {
        if (pg.ConstraintName == "ck_marca_nome_preenchido")
        {
            return "O nome da marca é obrigatório.";
        }
        else if (pg.ConstraintName == "ck_veiculo_placa_preenchida")
        {
            return "A placa é obrigatória.";
        }
        else if (pg.ConstraintName == "ck_veiculo_modelo_preenchido")
        {
            return "O modelo é obrigatório.";
        }
        else if (pg.ConstraintName == "ck_veiculo_tipo")
        {
            return "Tipo de veículo inválido.";
        }
        else
        {
            //mensagem da trigger
            return pg.MessageText;
        }
    }
}