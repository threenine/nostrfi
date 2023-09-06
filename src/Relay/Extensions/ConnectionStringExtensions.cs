using Microsoft.Extensions.Options;
using Nostrfi.Relay;
using Nostrfi.Relay.Validations;

namespace Nostrfi.Extensions;

public static class ConnectionStringExtensions
{
    public static OptionsBuilder<ConnectionStrings> ValidateConnectionStrings(this IServiceCollection services)
    {
        return services.AddOptions<ConnectionStrings>()
            .BindConfiguration(nameof(ConnectionStrings))
            .Validate(c => c.Validate(), RelayResources.ConnectionStringsInvalid);
        
    }
    
}