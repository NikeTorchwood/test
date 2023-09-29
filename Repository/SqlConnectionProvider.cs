using System.Data;
using Microsoft.Data.SqlClient;

namespace test.Repository;

public class SqlConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public SqlConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}