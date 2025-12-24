using Reqnroll;
using Reqnroll.Assist;

namespace SpecificationExample.AcceptanceTests.Hooks;

[Binding]
public class ValueRetrieverHooks
{
    [BeforeTestRun]
    public static void RegisterValueRetrievers()
    {
        Service.Instance.ValueRetrievers.Register(new GuidValueRetriever());
    }
}
