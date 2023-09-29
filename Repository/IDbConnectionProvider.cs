using System.Data;

namespace test.Repository;

public interface IDbConnectionProvider
{
    public IDbConnection GetConnection();
}