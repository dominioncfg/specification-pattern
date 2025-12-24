using Reqnroll.Assist;

namespace SpecificationExample.AcceptanceTests.Hooks;

public class GuidValueRetriever : IValueRetriever
{
    private static readonly Dictionary<string, Guid> _guidCache = new();

    public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        return propertyType == typeof(Guid) && keyValuePair.Value.StartsWith("@");
    }

    public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        var key = keyValuePair.Value;

        if (!_guidCache.ContainsKey(key))
        {
            _guidCache[key] = Guid.NewGuid();
        }

        return _guidCache[key];
    }
}
