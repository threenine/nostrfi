using System.Data.Common;
using Microsoft.IdentityModel.Tokens;
using static Nostrfi.Relay.RelayResources;

namespace Nostrfi.Relay.Validations;

public class ConnectionStrings : Dictionary<string, string>
{
    public ConnectionStrings()
    {
        DbProviderFactories.RegisterFactory(ConnectionStringNames.Postgre, Npgsql.NpgsqlFactory.Instance);
    }

    public bool Validate()
    {
        // instantiate a connection to the logger object
        var logger = LoggerFactory.Create(cfg => cfg.AddConsole().AddDebug()).CreateLogger(nameof(ConnectionStrings));
        
        var errors = new List<Exception>();
        foreach (var (key, value) in this)
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(key);
                using var connection = factory.CreateConnection();
                if (connection == null)
                {
                    throw new Exception(string.Format(ConnectionStringsExceptionMessage, key));
                }
                connection.ConnectionString = value;
                connection.Open();
                logger.LogInformation(DatabaseConnectionSuccess, key);
            }
            catch (Exception e)
            {
                
                logger.LogError(DatabaseConnectionFailure, key);
                errors.Add(new Exception(string.Format(ConnectionStringInvalidDefined, key), e));
            }
        }
        return errors.IsNullOrEmpty();
    }
}