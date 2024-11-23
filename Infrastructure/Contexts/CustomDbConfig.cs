using System.Data.Entity;

namespace Infrastructure.Contexts
{
    //public class CustomDbConfig : DbConfiguration
    //{

    //    public CustomDbConfig()
    //    {
    //        //SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
    //        SetDefaultConnectionFactory(GetSqlConn4DbName("localhost", "Trading"));
    //    }
    //    public SqlConnectionFactory GetSqlConn4DbName(string dataSource, string dbName)
    //    {
    //        var sqlConnStringBuilder = new SqlConnectionStringBuilder();
    //        sqlConnStringBuilder.DataSource = dataSource;
    //        //sqlConnStringBuilder.IntegratedSecurity = true;
    //        sqlConnStringBuilder.MultipleActiveResultSets = true;
    //        sqlConnStringBuilder.UserID = "sa";
    //        sqlConnStringBuilder.Password = "Gogogo123!"; // yeah, yeah, i know.
    //        sqlConnStringBuilder.PersistSecurityInfo = true;
    //        sqlConnStringBuilder.InitialCatalog = dbName;

    //        // NOW MY PROVIDER FACTORY OF CHOICE, switch providers here 
    //        return new SqlConnectionFactory(sqlConnStringBuilder.ConnectionString);
    //    }
    //}
}
