namespace Nostrfi;

internal static class ConnectionStringNames
{
    /// <summary>
    /// A default of localBuild is provided this can be any abitraty value as this will ensure
    /// the migrations are created using an ephemeral in memory database. If you want to use an actual database
    /// to create migrations then this value can be changed to the name of a Connection string.
    /// </summary>
    internal const string Postgre = "Postgre";
}

internal static class Constants
{
    internal const string CachePreFix = "EF_";
}