using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contexts
{
    public class CustomDbConfig : DbConfiguration
    {
        public CustomDbConfig()
        {
            //SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
            SetDefaultConnectionFactory(GetSqlConn4DbName("MONSTER", "PikUpStixTrader"));
        }
        public SqlConnectionFactory GetSqlConn4DbName(string dataSource, string dbName)
        {
            var sqlConnStringBuilder = new SqlConnectionStringBuilder();
            sqlConnStringBuilder.DataSource = dataSource;
            //sqlConnStringBuilder.IntegratedSecurity = true;
            sqlConnStringBuilder.MultipleActiveResultSets = true;
            sqlConnStringBuilder.UserID = "twsUser";
            sqlConnStringBuilder.Password = "go123456"; // yeah, yeah, i know.
            sqlConnStringBuilder.PersistSecurityInfo = true;
            sqlConnStringBuilder.InitialCatalog = dbName;

            // NOW MY PROVIDER FACTORY OF CHOICE, switch providers here 
            return new SqlConnectionFactory(sqlConnStringBuilder.ConnectionString);
        }
    }
}
