CREATE TABLE IF NOT EXISTS lancamentos (
    id UUID PRIMARY KEY,
    descricao VARCHAR(250) NOT NULL,
    valor NUMERIC(18,2) NOT NULL CHECK (valor > 0),
    tipo INT NOT NULL,
    data_lancamento TIMESTAMPTZ NOT NULL,
    usuario_id UUID NOT NULL
);

CREATE TABLE IF NOT EXISTS saldos_consolidados (
    data DATE PRIMARY KEY,
    total_creditos NUMERIC(18,2) NOT NULL DEFAULT 0,
    total_debitos NUMERIC(18,2) NOT NULL DEFAULT 0,
    saldo_final NUMERIC(18,2) NOT NULL DEFAULT 0,
    ultima_atualizacao TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS lancamentos_queue (
    id BIGSERIAL PRIMARY KEY,
    lancamento_id UUID NOT NULL,
    payload JSONB NOT NULL,
    criado_em TIMESTAMPTZ NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_lancamentos_data ON lancamentos (data_lancamento);
CREATE INDEX IF NOT EXISTS idx_lancamentos_queue_lancamento ON lancamentos_queue (lancamento_id);
