using Npgsql;

namespace CashFlow.Infrastructure;

public class ConnectionFactory(string connectionString)
{
    public NpgsqlConnection CreateConnection() => new(connectionString);
}
