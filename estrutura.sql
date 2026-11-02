-- TABELA: marca

CREATE TABLE marca (
    codigo  SERIAL       PRIMARY KEY,
    nome    VARCHAR(100) NOT NULL,
    CONSTRAINT ck_marca_nome_preenchido CHECK (LENGTH(TRIM(nome)) > 0)
);

CREATE UNIQUE INDEX uq_marca_nome ON marca (LOWER(nome));

-- TABELA: veiculo

CREATE TABLE veiculo (
    codigo        SERIAL       PRIMARY KEY,
    tipo          VARCHAR(20)  NOT NULL,
    placa         VARCHAR(7)   NOT NULL,
    modelo        VARCHAR(50)  NOT NULL,
    ano           INTEGER      NOT NULL,
    marca_codigo  INTEGER      NOT NULL,

    CONSTRAINT fk_veiculo_marca
        FOREIGN KEY (marca_codigo) REFERENCES marca (codigo)
        ON DELETE RESTRICT,
    CONSTRAINT ck_veiculo_tipo
        CHECK (tipo IN ('CARRO', 'MOTO', 'CAMINHAO')),
    CONSTRAINT ck_veiculo_placa_preenchida
        CHECK (LENGTH(TRIM(placa)) > 0),
    CONSTRAINT ck_veiculo_modelo_preenchido
        CHECK (LENGTH(TRIM(modelo)) > 0)
);

CREATE UNIQUE INDEX uq_veiculo_placa ON veiculo (UPPER(placa));

-- TABELA: logtransacao

CREATE TABLE logtransacao (
    codigo          SERIAL      PRIMARY KEY,
    tipo_operacao   VARCHAR(10) NOT NULL,
    data_hora       TIMESTAMP   NOT NULL DEFAULT NOW(),

    CONSTRAINT ck_log_tipo_operacao
        CHECK (tipo_operacao IN ('INSERT', 'UPDATE', 'DELETE'))
);


-- TRIGGER DE VALIDAÇÃO: veiculo
-- Padroniza a placa e valida o ano (1950 até o ano atual)

CREATE OR REPLACE FUNCTION fn_validar_veiculo()
RETURNS TRIGGER AS $$
BEGIN
    -- Padroniza a placa: sem espaços nas pontas e em maiúsculas
    NEW.placa := UPPER(TRIM(NEW.placa));

    -- Regra: ano entre 1950 e o ano atual
    IF NEW.ano < 1950
       OR NEW.ano > EXTRACT(YEAR FROM CURRENT_DATE)::INT THEN
        RAISE EXCEPTION 'Ano inválido: deve estar entre 1950 e %.',
            EXTRACT(YEAR FROM CURRENT_DATE)::INT
            USING ERRCODE = '23514';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_veiculo_validar
BEFORE INSERT OR UPDATE ON veiculo
FOR EACH ROW EXECUTE FUNCTION fn_validar_veiculo();

-- TRIGGER DE LOG: veiculo
-- Registra INSERT, UPDATE e DELETE na tabela logtransacao

CREATE OR REPLACE FUNCTION fn_log_veiculo()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO logtransacao (tipo_operacao)
    VALUES (TG_OP);

    IF TG_OP = 'DELETE' THEN
        RETURN OLD;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_veiculo_log
AFTER INSERT OR UPDATE OR DELETE ON veiculo
FOR EACH ROW EXECUTE FUNCTION fn_log_veiculo();