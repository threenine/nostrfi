using System.Data.Common;
using Microsoft.IdentityModel.Tokens;
using Nostrfi.Relay;

namespace Nostrfi.Validations;

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
                    throw new Exception(string.Format(RelayResources.ConnectionStringsExceptionMessage, key));
                }
                connection.ConnectionString = value;
                connection.Open();
            }
            catch (Exception e)
            {
                
                logger.LogError("Could not connect to {Database}", key);
                errors.Add(new Exception(string.Format(RelayResources.ConnectionStringInvalidDefined, key), e));
            }
        }
        return errors.IsNullOrEmpty();
    }
}